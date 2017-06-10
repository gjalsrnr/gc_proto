using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    RIGHT,
    LEFT
}

public enum PLAYERSTATE
{
    IDLE,
    MOVE,
    JUMP,
    FIRE,
    MOVEFIRE,
    DASH,
    MOVEJUMP
}

public class PlayerControll : MonoBehaviour {
    private Bullet bullet;
    private Rigidbody2D rbody2D;
    //private PLAYERSTATE state;
    public PLAYERSTATE state;
    public float jumpPower = 5.0f;
    public bool collisionGround = false;
    private Collider2D col2d;
    public GameObject Bullet;
    Quaternion rotate;
    //차후에 총알 사이즈가 정해지면 총알로 옮겨갈 것.
    private float bulletGap = 2;

    Vector3 mousePos;
    //private bool isJump;
    public PLAYERSTATE GetState
    {
        get { return state; }
        set
        {
            state = value;

            switch(state)
            {
                case PLAYERSTATE.MOVE:
                    this.transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.Self);
                    break;
                case PLAYERSTATE.MOVEFIRE:
                    break;
                case PLAYERSTATE.FIRE:
                    break;
                case PLAYERSTATE.JUMP:
                    //rbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    break;
            }
        }
    }

    public Dir dir;

    public Dir GetDirection
    {
        get { return dir; }
        set { dir = value; }
    }

    public float moveSpeed = 3.0f;
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }

    void Awake()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();   
    }

    // Use this for initialization
    void Start () {
        state = PLAYERSTATE.IDLE;
        //state = PLAYERSTATE.JUMP;
    }
	
	// Update is called once per frame
	void Update () {

        float gunAngle = GetToMouseAngle(this.transform.position.x, this.transform.position.y);// + 90;

        //float gunAngle = playerToMouseAngle;// + 90;


        Debug.Log(GetToMouseAngle(this.transform.position.x, this.transform.position.y));// + 90);//GetToMouseAngle(this.transform.position.x, this.transform.position.y));
        
        DefineOfState();
        if(state == PLAYERSTATE.MOVE)
            this.transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.Self);
        if(state == PLAYERSTATE.FIRE)
        {
            Vector3 pos;
            pos.x = bulletGap; 
            pos.y = 0;
            pos.z = 0;

            //Quaternion.Euler(0, gunAngle, 0)
            Instantiate(Bullet, this.transform.position + pos, Quaternion.Euler(0, 0, gunAngle));//this.transform.rotation);//this.transform.position, this.transform.rotation);
            state = PLAYERSTATE.IDLE;
        }
    }

    void FixedUpdate()
    {
        if(state == PLAYERSTATE.JUMP)
        {
            if(collisionGround)
               rbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //    state = PLAYERSTATE.IDLE;
        }     
    }

    public void DefineOfState()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.A))
        {
            dir = Dir.LEFT;
            if (bulletGap > 0)
            {
                bulletGap = (-2 - col2d.bounds.size.x / 2);
                //float gunAngle = GetToMouseAngle(this.transform.position.x, this.transform.position.y) + 90;
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            //state = PLAYERSTATE.MOVE;

            if (state != PLAYERSTATE.JUMP)
                state = PLAYERSTATE.MOVE;
            else
                state = PLAYERSTATE.MOVEJUMP;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.A))
        {
            dir = Dir.RIGHT;
            if (bulletGap < 0)
            {
                bulletGap = (2 + col2d.bounds.size.x / 2);
                //float gunAngle = GetToMouseAngle(this.transform.position.x, this.transform.position.y) + 90;
                //this.transform.rotation = Quaternion.Euler(0, 0, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (state != PLAYERSTATE.JUMP)
                state = PLAYERSTATE.MOVE;
            else
                state = PLAYERSTATE.MOVEJUMP;
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if(state != PLAYERSTATE.JUMP && state != PLAYERSTATE.MOVEJUMP)
            state = PLAYERSTATE.IDLE;
        }

        //if (state != PLAYERSTATE.JUMP) //&& state != PLAYERSTATE.MOVEJUMP)
        //{ 
          if(Input.GetKeyDown(KeyCode.Space))
            {
               if(state != PLAYERSTATE.JUMP)
                {
                //collisionGround = false;
                    state = PLAYERSTATE.JUMP;
                    Debug.Log("점프");
                }
            }
        //}

        if (Input.GetKeyDown(KeyCode.A))
        {
              state = PLAYERSTATE.FIRE;
            //Instantiate(Bullet, this.transform.position, this.transform.rotation);
            //Instantiate(Bullet, this.transform.position, this.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            state = PLAYERSTATE.MOVEFIRE;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.RightArrow))
        {
            state = PLAYERSTATE.MOVEFIRE;
        }
        //Fire 푸는건 애니메이션이 다 돌았을 때 풀어야 함. 따라서 해당 스프라이트가 얼마 간격으로 움직이나가 필요.
    }

    private float GetToMouseAngle(float x1, float y1)
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float x =  mousePos.x - x1;
        float y =  mousePos.y - y1;

        //float rad = Mathf.Atan2(x, y) + Mathf.PI;

        float dis = Mathf.Sqrt(x * x + y * y);

        float rad = Mathf.Acos(x / dis);

        if (y > 0)
            rad = Mathf.PI * 2 - rad;

        float angle = rad * Mathf.Rad2Deg;

        return angle;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
        {
            collisionGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
        {
            collisionGround = true;
            if (state == PLAYERSTATE.JUMP)
            {
                state = PLAYERSTATE.IDLE;
            }
        }
    }
}
