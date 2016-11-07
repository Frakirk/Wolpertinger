using UnityEngine;
using System.Collections;

public class GamepadInput : MonoBehaviour 
{
	public string player;

    public float defaultDrag;
    public float runSpeedAccel;
	public float runSpeedMax;
	public float jumpForce;
    public float wingForce;
    public int wingBoostMax;
    public GameManager gameManager;

    private GameObject wingBoostAura;
    private float jumpCharge;
    private float wingCharge;

	private float launchBuffer;             // launchBuffer prevents surface-contact checks for a short time after each jump. This is needed to prevent some unwanted jump exploits and behaviours.
	private float wingXVal, wingYVal;
    //private float wingFlap;

    private int wingBoostCount;
    private bool jumping;
    //private bool wingBoosting;
    private bool onGround;
	private int onWall;
	private bool onPlayer;
    private Rigidbody2D goomba;

    private float wjMoveDamper;             // wallJumpMoveDamper, prevents the player from accelerating horizontally after walljumping
	private float rsaStore;                 // stores the value of runSpeedAccel
	private float inputXAxis, inputYAxis, inputRTrigger;
	private Rigidbody2D rbody;
	private CircleCollider2D circleCol;

	void Start () 
	{
		rbody = GetComponent<Rigidbody2D>();
		circleCol = GetComponent<CircleCollider2D>();
        //gameManager = (GameObject.Find("GameManager")).GetComponent<GameManager>();
		onGround = false;
        jumping = false;
        //wingBoosting = false;
        wingBoostCount = 0;
	}

	void Update () 
	{
        if (Input.GetButtonDown(player + "Start"))
            gameManager.PauseGame();

        if (gameManager.gamePaused)
            return;

		inputXAxis = Input.GetAxis (player + "LeftAxisX");       // between -1 and +1
		inputYAxis = Input.GetAxis (player + "LeftAxisY");        // between -1 and +1
        if (player == "P3" || player == "P4")
        {
            inputXAxis += Input.GetAxisRaw(player + "KbAxisX");
            inputYAxis += Input.GetAxisRaw(player + "KbAxisY");
            if (Input.GetButton(player + "KbAxisX") && Input.GetButton(player + "KbAxisY"))
            {
                inputXAxis *= 0.7071f;
                inputYAxis *= 0.7071f;
            }
        }
        //inputRTrigger = Input.GetAxis(player + "RightTrigger");  // between 0 and +1

		if (inputXAxis > 0)                                                         // check if the player is holding the stick right
		{
			if (rbody.velocity.x < runSpeedMax)                                         // check if they are moving slower than their max speed
			{
				rbody.velocity += new Vector2 (inputXAxis * runSpeedAccel, 0);              // accelerate the player
				if (rbody.velocity.x > runSpeedMax)                                         // check if they are now moving faster than their max speed
					rbody.velocity = new Vector2 (runSpeedMax, rbody.velocity.y);               // set their speed to the maximum
			}
		}
		else if (inputXAxis < 0)                                                    // check if the player is holding the stick left
		{
			if (rbody.velocity.x > -runSpeedMax)                                        // check if they are moving slower than their max speed
			{
				rbody.velocity += new Vector2(inputXAxis * runSpeedAccel, 0);               // accelerate the player
                if (rbody.velocity.x < -runSpeedMax)                                        // check if they are now moving faster than their max speed
					rbody.velocity = new Vector2 (-runSpeedMax, rbody.velocity.y);              // set their speed to the maximum
			}
		}

        //check what surfaces the player is in contact with
        if (launchBuffer == 0)  // check if the launchBuffer isn't active
        {
            CheckIfOnGround();      // sets onGround
            CheckIfOnPlayer();      // sets onPayer
            CheckIfOnWall();        // sets onWall
        }

        CheckJumpButton();

        CheckBoostButton();

        if (launchBuffer > 0)                   // check if there is time remaining on launchBuffer
		{
			launchBuffer -= Time.deltaTime;         // remove time from launchBuffer
			if (launchBuffer < 0)                   // check if launchBuffer has run out of time
				launchBuffer = 0;                       // set launchBuffer to zero
        }

		if (wjMoveDamper > 0)               // check if there is time remaining on wjMoveDamper
		{
			wjMoveDamper -= Time.deltaTime;     // remove time from wjMoveDamper
			if (wjMoveDamper < 0)               // check if wjMoveDamper has run out of time
			{
				wjMoveDamper = 0;                   // set wjMoveDamper to zero
				runSpeedAccel = rsaStore;           // set runSpeedAccel to its original value
			}
		}

		rbody.drag = defaultDrag;                                                   // set the player's drag value to it's base value
        if (onWall != 0)                                                            // check if the player is on a wall
        {
            if ((onWall > 0 && inputXAxis > 0) || (onWall < 0 && inputXAxis < 0))       // check if the analogue stick is pointing towards the wall the player is on
                rbody.drag = Mathf.Abs(inputXAxis) * 20;                                    // set the drag value according to how far to the side the analogue stick is held
        }

        //Debug.Log(rbody.velocity.y);
        if (rbody.velocity.y > 20)
            rbody.velocity = new Vector2(rbody.velocity.x, 20);
    }
    //-----------------------------------END OF Update()---------------------------------------------------
    
    void CheckJumpButton()
    {
        if (Input.GetButtonDown(player + "BtnA"))                       // check if the jump button has been pressed
        {

            if (onWall != 0)                                           // check if the player is jumping off the side of a wall
            {
                rbody.velocity += new Vector2(onWall * -7.5f, jumpForce * 1.5f);  // apply a force that pushes the player upwards and horizontally away from the wall
                launchBuffer = 0.2f;                                            // set the launch buffer for 0.2s, preventing the checks that set onGround/onPlayer/onWall
                rsaStore = runSpeedAccel;                                       // store the value of runSpeedAccel
                runSpeedAccel = 0;                                              // set runSpeedAccel to 0, preventing the player from accelerating horizontally
                wjMoveDamper = 0.2f;                                            // set the amount of time until runSpeedAccel is returned to its original value
                jumping = true;                                                 // 'jumping' will be true until the jump button is released
                jumpCharge = 0.35f;                                             // sets the maximum amount of time that the player will gain extra height for holding the jump button
            }
            else if (onGround)                                                   // check if the player is jumping from the ground
            {
                rbody.velocity += new Vector2(0, jumpForce);                    // apply a vertical force to make the player rise
                launchBuffer = 0.2f;                                            // set the launch buffer for 0.2s, preventing the checks that set onGround/onPlayer/onWall
                jumping = true;                                                 // 'jumping' will be true until the jump button is released
                jumpCharge = 0.35f;                                             // sets the maximum amount of time that the player will gain extra height for holding the jump button
            }
            else if (onPlayer)                                              // check if the player is jumping off the top of another player
            {
                rbody.velocity += new Vector2(0, jumpForce * 0.75f);            // apply a reduced vertical force to make the player rise
                launchBuffer = 0.2f;                                            // set the launch buffer for 0.2s, preventing the checks that set onGround/onPlayer/onWall                                      
                goomba.velocity += new Vector2(0, jumpForce * -0.5f);           // apply a force downwards on the player that has been jumped on
                jumping = true;                                                 // 'jumping' will be true until the jump button is released
                jumpCharge = 0.35f;                                             // sets the maximum amount of time that the player will gain extra height for holding the jump button
            }
            onGround = false;                                               // player is not grounded,
            onPlayer = false;                                               // player is not on top of another player,
            onWall = 0;                                                     // player is not on a wall.
        }
        else if (Input.GetButtonUp(player + "BtnA"))                    // check if the jump button has been released
        {
            jumping = false;                                                // set jumping to false, to indicate that the jump button is not being pressed
        }
        else if (jumping)                                               // check if the jump button is still pressed
        {
            if (jumpCharge > 0)                                             // check if the player is should still be gaining force from the jump
            {
                rbody.velocity += new Vector2(0, jumpForce * 3 * Time.deltaTime);   // add the extra jump force for keeping the button pressed
                jumpCharge -= Time.deltaTime;                                   // reduce the time remaining on jumpCharge
            }
        }
    }

    void CheckBoostButton()
    {
        if (Input.GetButtonDown(player + "RightBumper") && wingBoostCount < wingBoostMax)                       // check if the jump button has been pressed
        {
            wingXVal = inputXAxis * wingForce;
            wingYVal = -inputYAxis * wingForce;
            if (wingYVal > 0)
                wingYVal *= 1.25f;
            rbody.velocity = new Vector2(wingXVal, wingYVal);
            launchBuffer = 0.2f;
            wingCharge = 0.25f;
            wingBoostCount++;
            if (wingBoostAura != null)
                Destroy(wingBoostAura);
            wingBoostAura = (GameObject)Instantiate(Resources.Load("WingBoostAura"), transform.position, transform.rotation);
            (wingBoostAura.GetComponent<WingBoostAura>()).parent = gameObject;
        }
        else if (wingCharge > 0)                                             // check if the player is should still be gaining force from the jump
        {
            wingXVal = inputXAxis * wingForce * 5f * Time.deltaTime;
            wingYVal = -inputYAxis * wingForce * 5f * Time.deltaTime;
            rbody.velocity += new Vector2(wingXVal, wingYVal);   // add the extra jump force for keeping the button pressed
            wingCharge -= Time.deltaTime;                                     // reduce the time remaining on jumpCharge
            if (wingCharge <= 0)
            {
                wingCharge = 0;
                Destroy(wingBoostAura);
            }
        }
    }

    void CheckIfOnGround()                                                                  // checks if the player is on the ground
    {
        Vector2 groundCheckVec;                                                                 // vector to check a position below the player
        groundCheckVec = new Vector2(rbody.position.x, rbody.position.y - 0.15f);                // the position of the vector is only 0.1 below the centre of the player
        if (Physics2D.OverlapCircle(groundCheckVec, 0.45f, 1 << 8, 0f, 0f) != null)             // using a circle collider slightly smaller than the player, check for collisions on the wall layer
        {
            onGround = true;                                                                        // the player is ground
            wingBoostCount = 0;
        }
        else                                                                                    // else
            onGround = false;                                                                       // the player is ungrounded
	}

	void CheckIfOnWall()                                                                    // checks if the player is on a wall
	{
        onWall = 0;                                                                             // sets onWall to 0, indicating that the player is not on a wall. If the next two checks fail, this is the final value.
        Vector2 wallCheckVec;                                                                   // vector to check a position to the side of the player
        wallCheckVec = new Vector2(rbody.position.x + (circleCol.radius)*-1, rbody.position.y); // the position of the vector is to the player's left
        if (Physics2D.OverlapCircle(wallCheckVec, 0.15f, 1 << 8, 0f, 0f) != null)               // using a small circle collider, check for a wall to the player's left (WALL CHECK 1)
        {
            onWall = -1;                                                                            // set onWall to -1, indicating a wall to the left
            wingBoostCount = 0;
        }
        wallCheckVec = new Vector2(rbody.position.x + (circleCol.radius), rbody.position.y);    // the position of the vector is now to the player's right
        if (Physics2D.OverlapCircle(wallCheckVec, 0.15f, 1 << 8, 0f, 0f) != null)               // circle collider check to the right (WALL CHECK 2)
        {
            onWall = 1;                                                                             // set onWall to 1, indicating a wall to the right
            wingBoostCount = 0;
        }
    }
	
	void CheckIfOnPlayer()                                                                  // checks if the player is on another player
	{
        Vector2 playerCheckVec;                                                                 // vector to check a position below the player
        playerCheckVec = new Vector2(rbody.position.x, rbody.position.y - 0.1f);                // same position as the groundCheckVec
		transform.position += new Vector3(9999, 9999, 9999);                                    // move the player way off into nowhere, this is needed so that their ID isn't returned in the following collision check
		var goombaList = Physics2D.OverlapCircleAll(playerCheckVec, 0.45f, 1, 0f, 0f);          // check for collisions with player objects, and put all returned IDs into a list
		if (goombaList.Length > 0)                                                              // check if any IDs were returned
		{
			onPlayer = true;                                                                        // the player is on another player
			goomba = goombaList[0].GetComponent<Rigidbody2D>();                                     // keep a reference to the first colliding player on the list (couldn't think of a better variable name sorry) 
            wingBoostCount = 0;
        }
		else                                                                                    // else
			onPlayer = false;                                                                       // the player is not on another player
		transform.position -= new Vector3(9999, 9999, 9999);                                    //bring the player back from the far off place they were sent to
	}
}

// ------------------------------------------------------- OLD CHARGE-JUMP CODE ---------------------------------
//      if (Input.GetButtonDown(player + "BtnA"))
//{
//	jumpCharge = 1;
//}
//else if (Input.GetButton(player + "BtnA"))
//{
//	if (jumpCharge < 1.5)
//	{
//		jumpCharge += Time.deltaTime;
//		if (jumpCharge > 1.5)
//		{
//			jumpCharge = 1.5f;
//		}
//	}
//}
//else if (Input.GetButtonUp(player + "BtnA"))
//{
//	if (onGround)
//	{
//		rbody.velocity += new Vector2(0, jumpForce * jumpCharge);
//		launchBuffer = 0.2f;
//	}
//	else if (onPlayer)
//	{
//		rbody.velocity += new Vector2(0, jumpForce * jumpCharge * 0.75f);
//		launchBuffer = 0.2f;
//		wingBoostCount = 0;
//		goombaCollider.GetComponent<Rigidbody2D>().velocity += new Vector2(0, jumpForce * jumpCharge * -0.5f);
//	}
//	else if (onWall)
//	{
//		rbody.velocity += new Vector2(inputXAxis * -jumpCharge * 5f, jumpForce * jumpCharge);
//		launchBuffer = 0.2f;
//		rsaStore = runSpeedAccel;
//		runSpeedAccel = 0;
//		wjMoveDamper = 0.2f;
//	}
//	onGround = false;
//	onPlayer = false;
//	onWall = false;
//}

//------------------------------------------------- OLD WING-BOOST CODE---------------------------------------
//      if (wingBoostCount < wingBoostMax)
//{
//	if (Input.GetButtonDown(player + "BtnX"))
//	{
//		wingCharge = 1;
//	}
//	else if (Input.GetButton(player + "BtnX"))
//	{
//		if (wingCharge < 1.5)
//		{
//			wingCharge += Time.deltaTime;
//			if (wingCharge > 1.5)
//			{
//				wingCharge = 1.5f;
//			}
//		}
//	}
//	else if (Input.GetButtonUp(player + "BtnX"))
//	{
//		rbody.drag = 0;
//		wingXVal = inputXAxis * wingForce * wingCharge;
//		wingYVal = inputYAxis * wingForce * wingCharge;
//		if (wingYVal > 0)
//			wingYVal *= 1.25f;
//		rbody.velocity += new Vector2(wingXVal, wingYVal);
//		launchBuffer = 0.2f;
//		wingForceDuration = 0.5f;
//		wingBoostCount++;
//	}
//}

//if (wingForceDuration > 0)
//{
//	wingForceDuration -= Time.deltaTime;
//	rbody.velocity += new Vector2(wingXVal * wingForceDuration * 0.1f, wingYVal * wingForceDuration * 0.1f);
//	if (wingForceDuration < 0)
//		wingForceDuration = 0;
//}

//if (inputRTrigger > wingFlap)                               // check whether the trigger has been pressed further since last frame
//{
//    float pushForce;
//    pushForce = -Mathf.Sqrt((inputRTrigger - wingFlap) * 3);    // magic math soup to get the force to apply this frame. Scales with how far the trigger has been pulled
//    if (pushForce < -0.66f)                                     // checks if the trigger was pulled too fast
//        pushForce = -0.66f;                                         // sets the force to the max allowed value
//    wingXVal = pushForce * inputXAxis * runSpeedAccel;          // sets the horizontal component of the force vector to be added
//    wingYVal = pushForce;                                       // sets the vertical component's base value
//    if (inputYAxis < 0)                                         // check if the analogue stick is pointing downward
//        wingYVal *= inputYAxis;                                     // adjust the vertical component to the stick's downward angle 
//    if (wingYVal < 0)                                           // check if the vartical component will make the player rise
//    {
//        if (rbody.velocity.y < 0)                                   // check if the player is falling
//            wingYVal *= 6;                                              // increase the vertical force significantly
//        else
//            wingYVal *= 3;                                              // increase the vertical force less
//    }
//    rbody.velocity -= new Vector2(wingXVal, wingYVal);          // apply the force vector to the player
//}
//wingFlap = inputRTrigger;                                   // set wingFlap to this frame's input value, so that next frame's check on line 72 is accurate
