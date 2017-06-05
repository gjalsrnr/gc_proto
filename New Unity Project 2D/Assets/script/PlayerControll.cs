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

    public GameObject Bullet;
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

    private Dir dir;

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
    }

    // Use this for initialization
    void Start () {
        state = PLAYERSTATE.IDLE;
        //state = PLAYERSTATE.JUMP;
    }
	
	// Update is called once per frame
	void Update () {
        DefineOfState();
        if(state == PLAYERSTATE.MOVE)
            this.transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.Self);
        if(state == PLAYERSTATE.FIRE)
        {
            /*Vector3 pos;
            pos.x = 3;
            pos.y = 3;*/
            Instantiate(Bullet, this.transform.position, this.transform.rotation);//this.transform.position, this.transform.rotation);
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
            if (moveSpeed > 0)
            {
                moveSpeed = moveSpeed * -1;
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
            if (moveSpeed < 0)
            {
                moveSpeed = moveSpeed * -1;
              //  bullet.Speed = bullet.Speed * -1;
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
