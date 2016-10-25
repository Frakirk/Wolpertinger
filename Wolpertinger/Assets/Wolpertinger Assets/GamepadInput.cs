using UnityEngine;
using System.Collections;

public class GamepadInput : MonoBehaviour 
{
	public string player;

	public float runSpeedAccel;
	public float runSpeedMax;
	public float jumpForce;
    public float wingForce;
    public int wingBoostMax;
    public float defaultDrag;

    private int wingBoostCount;
    private float wingCharge;
    private float jumpCharge;
    private float launchBuffer;
    private float wingForceDuration;
    private float wingXVal, wingYVal;

    private bool onGround;
	private bool onWall;
    private bool onPlayer;
    private Collider2D goombaCollider;

    private Vector2 wallCheckVec;
    private float wjMoveDamper;
    private float rsaStore;
    private float inputXAxis, inputYAxis;
	private Rigidbody2D rbody;
    private CircleCollider2D circleCol;

    void Start () 
	{
        wingBoostCount = 0;
		rbody = GetComponent<Rigidbody2D>();
        circleCol = GetComponent<CircleCollider2D>();
		onGround = false;
        wingCharge = 0f;
	}

	void Update () 
	{
		inputXAxis = Input.GetAxis (player + "LeftAxisX");
        inputYAxis = Input.GetAxis(player + "LeftAxisY");
		// Check which direction the player is trying to move in, and if they are moving 
		// less than the max run speed, accelerate them
		if (inputXAxis > 0)
        {
			if (rbody.velocity.x < runSpeedMax)
            {
				rbody.velocity += new Vector2 (inputXAxis * runSpeedAccel, 0);
                if (rbody.velocity.x > runSpeedMax)
                { 
					rbody.velocity = new Vector2 (runSpeedMax, rbody.velocity.y);
				}
			}
		// same acceleration code, but for running to the left rather than right
		}
        else if (inputXAxis < 0)
        {
			if (rbody.velocity.x > -runSpeedMax)
            {
                rbody.velocity += new Vector2(inputXAxis * runSpeedAccel, 0);
                if (rbody.velocity.x < -runSpeedMax)
                {
					rbody.velocity = new Vector2 (-runSpeedMax, rbody.velocity.y);
				}
			}
		}

        if (launchBuffer <= 0)
        {
            checkIfOnGround();
            checkIfOnPlayer();
            checkIfOnWall();
        }

        if (Input.GetButtonDown(player + "BtnA"))
        {
            jumpCharge = 1;
        }
        else if (Input.GetButton(player + "BtnA"))
        {
            if (jumpCharge < 1.5)
            {
                jumpCharge += Time.deltaTime;
                if (jumpCharge > 1.5)
                {
                    jumpCharge = 1.5f;
                }
            }
        }
        else if (Input.GetButtonUp(player + "BtnA"))
        {
            if (onGround)
            {
                rbody.velocity += new Vector2(0, jumpForce * jumpCharge);
                launchBuffer = 0.2f;
            }
            else if (onPlayer)
            {
                rbody.velocity += new Vector2(0, jumpForce * jumpCharge * 0.75f);
                launchBuffer = 0.2f;
                wingBoostCount = 0;
                goombaCollider.GetComponent<Rigidbody2D>().velocity += new Vector2(0, jumpForce * jumpCharge * -0.5f);
            }
            else if (onWall)
            {
                rbody.velocity += new Vector2(inputXAxis * -jumpCharge * 5f, jumpForce * jumpCharge);
                launchBuffer = 0.2f;
                rsaStore = runSpeedAccel;
                runSpeedAccel = 0;
                wjMoveDamper = 0.2f;
            }
            onGround = false;
            onPlayer = false;
            onWall = false;
        }

        if (wingBoostCount < wingBoostMax)
        {
            if (Input.GetButtonDown(player + "BtnX"))
            {
                wingCharge = 1;

            }
            else if (Input.GetButton(player + "BtnX"))
            {
                if (wingCharge < 1.5)
                {
                    wingCharge += Time.deltaTime;
                    if (wingCharge > 1.5)
                    {
                        wingCharge = 1.5f;
                    }
                }
            }
            else if (Input.GetButtonUp(player + "BtnX"))
            {
                rbody.drag = 0;
                wingXVal = inputXAxis * wingForce * wingCharge;
                wingYVal = inputYAxis * wingForce * wingCharge;
                if (wingYVal > 0)
                    wingYVal *= 1.25f;
                rbody.velocity += new Vector2(wingXVal, wingYVal);
                launchBuffer = 0.2f;
                wingForceDuration = 0.5f;
                wingBoostCount++;
            }
        }

        if (launchBuffer > 0)
        {
            launchBuffer -= Time.deltaTime;
            if (launchBuffer < 0) {
                launchBuffer = 0;
            }
        }
        if (wingForceDuration > 0)
        {
            wingForceDuration -= Time.deltaTime;
            rbody.velocity += new Vector2(wingXVal * wingForceDuration * 0.1f, wingYVal * wingForceDuration * 0.1f);
            if (wingForceDuration < 0)
            {
                wingForceDuration = 0;
            }
        }
        if (wjMoveDamper > 0)
        {
            wjMoveDamper -= Time.deltaTime;
            if (wjMoveDamper < 0)
            {
                wjMoveDamper = 0;
                runSpeedAccel = rsaStore;
            }
        }

        rbody.drag = defaultDrag;
        if (onWall)
            rbody.drag = 20;
        else if (Input.GetButton(player + "BtnX"))
            rbody.drag = wingCharge * 10;

    }

    void checkIfOnGround()
    {
        // a collision test to check if the player is on the ground
        wallCheckVec = new Vector2(rbody.position.x, rbody.position.y - 0.1f);
        if (Physics2D.OverlapCircle(wallCheckVec, 0.45f, 1 << 8, 0f, 0f) != null)
        {
            onGround = true;
            wingBoostCount = 0;
        }
        else
        {
            onGround = false;
        }
    }

    void checkIfOnWall()
    {
        wallCheckVec = new Vector2(rbody.position.x + (circleCol.radius)*inputXAxis, rbody.position.y);
        if (Physics2D.OverlapCircle(wallCheckVec, 0.25f, 1 << 8, 0f, 0f) != null)
        {
            onWall = true;
            wingBoostCount = 0;
        }
        else
        {
            onWall = false;
        }
    }
    
    void checkIfOnPlayer()
    {
        wallCheckVec = new Vector2(rbody.position.x, rbody.position.y - 0.1f);
        transform.position += new Vector3(9999, 9999, 9999);
        var goombaList = Physics2D.OverlapCircleAll(wallCheckVec, 0.45f, 1, 0f, 0f);
        if (goombaList.Length > 0)
        {
            onPlayer = true;
            goombaCollider = goombaList[0];
        }
        else
        {
            onPlayer = false;
        }
        transform.position -= new Vector3(9999, 9999, 9999);
    }
}