using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;


	
public class PlayerController : MonoBehaviour 
{

	public enum WeaponType
	{
		UNARMED = 0,
		TWOHANDSWORD = 1,
		TWOHANDSPEAR = 2,
		TWOHANDAXE = 3,
		TWOHANDBOW = 4,
		TWOHANDCROSSBOW = 5,
		STAFF = 6,
		ARMED = 7,
		RELAX = 8,
		RIFLE = 9
	}

	#region Variables

	//Components
	Rigidbody rb;
	protected Animator animator;
	public GameObject target;
	private Vector3 targetDashDirection;
	CapsuleCollider capCollider;
	ParticleSystem FXSplash;
	public Camera sceneCamera;
	public Vector3 waistRotationOffset;

	//jumping variables
	public float gravity = -9.8f;
	bool canJump;
	bool isJumping = false;
	bool isGrounded;
	public float jumpSpeed = 12;
	public float doublejumpSpeed = 12;
	bool doJump = false;
	bool doublejumping = true;
	bool canDoubleJump = false;
	bool isDoubleJumping = false;
	bool doublejumped = false;
	bool isFalling;
	bool startFall;
	float fallingVelocity = -1f;

	// Used for continuing momentum while in air
	public float inAirSpeed = 8f;
	float maxVelocity = 2f;
	float minVelocity = -2f;

	//rolling variables
	public float rollSpeed = 8;
	bool isRolling = false;
	public float rollduration;

	//movement variables
	bool isMoving = false;
	bool canMove = true;
	public float walkSpeed = 1.35f;
	float moveSpeed;
	public float runSpeed = 6f;
	float rotationSpeed = 40f;

	//skill variables
	public bool isCasting = false;
	float castTime = 0f;
	float castTimeMax = 40f;
	float spellCastTime = 0f;
	bool isCastingC = false;
	bool isCastingV = false;
	public bool canCast = false;
	public bool isCharging = false;
	GameObject castBarPrefab;
	GameObject castBar;
	GameObject NoST;

  
	float x;
	float z;
	float dv;
	float dh;
	Vector3 inputVec;
	Vector3 newVelocity;
    bool weaponToggle = true;

	//Weapon and Shield
	private WeaponType weapon;
	int rightWeapon = 0;
	int leftWeapon = 0;
	bool isRelax = false;

	//isStrafing/action variables
	bool canAction = true;
	bool isStrafing = false;
	bool isDead = false;
	bool isBlocking = false;
	bool blockGui;
	public float knockbackMultiplier = 1f;
	bool isKnockback;
	bool isSitting = false;
	bool isAiming = false;

	//Swimming variables
	public bool isSwimming = false;
	public float inWaterSpeed = 8f;

	//Weapon Models
	public GameObject twohandaxe;
	public GameObject twohandsword;
	public GameObject twohandspear;
	public GameObject twohandbow;
	public GameObject twohandcrossbow;
	public GameObject staff;
	public GameObject swordL;
	public GameObject swordR;
	public GameObject maceL;
	public GameObject maceR;
	public GameObject daggerL;
	public GameObject daggerR;
	public GameObject itemL;
	public GameObject itemR;
	public GameObject shield;
	public GameObject pistolL;
	public GameObject pistolR;
	public GameObject rifle;

	public GameObject firstWep;
	public GameObject secondWep;
	#endregion

	#region MarkAdded

	Player player;
	public GameObject spellFirePillar;
	public GameObject spellTransmutationFire;
	GameObject currentWeapon;

	#endregion

	#region Initialization

	void Awake() 
	{
		//set the animator component
		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody>();
		capCollider = GetComponent<CapsuleCollider>();
		//FXSplash = transform.GetChild(2).GetComponent<ParticleSystem>();
		//hide all weapons
		if(twohandaxe != null)
		{
			twohandaxe.SetActive(false);
		}
		if(twohandbow != null)
		{
			twohandbow.SetActive(false);
		}
		if(twohandcrossbow != null)
		{
			twohandcrossbow.SetActive(false);
		}
		if(twohandspear != null)
		{
			twohandspear.SetActive(false);
		}
		if(twohandsword != null)
		{
			twohandsword.SetActive(false);
		}
		if(staff != null)
		{
			staff.SetActive(false);
		}
		if(swordL != null)
		{
			swordL.SetActive(false);
		}
		if(swordR != null)
		{
			swordR.SetActive(false);
		}
		if(maceL != null)
		{
			maceL.SetActive(false);
		}
		if(maceR != null)
		{
			maceR.SetActive(false);
		}
		if(daggerL != null)
		{
			daggerL.SetActive(false);
		}
		if(daggerR != null)
		{
			daggerR.SetActive(false);
		}
		if(itemL != null)
		{
			itemL.SetActive(false);
		}
		if(itemR != null)
		{
			itemR.SetActive(false);
		}
		if(shield != null)
		{
			shield.SetActive(false);
		}
		if(pistolL != null)
		{
			pistolL.SetActive(false);
		}
		if(pistolR != null)
		{
			pistolR.SetActive(false);
		}
		if(rifle != null)
		{
			rifle.SetActive(false);
		}

		player = GetComponent<Player> ();
		sceneCamera = FindObjectOfType<Camera> ();
		if (secondWep != null)
			secondWep.SetActive (false);
		currentWeapon = firstWep;
		player.currentWeapon = currentWeapon.GetComponent<Weapon>();
		weapon = WeaponType.ARMED;
		rightWeapon = 19;
		animator.SetInteger("RightWeapon", 9);
		animator.SetBool("Armed", true);
		animator.SetInteger("Weapon", 7);

		castBarPrefab = (GameObject)Resources.Load("ChargeBar");
		NoST = (GameObject)Resources.Load ("NoST");

	}

	#endregion
	
	#region UpdateAndInput
	
	void Update()
	{
		//input abstraction for easier asset updates using outside control schemes
		bool inputJump = Input.GetButtonDown("Jump");
		bool inputLightHit = Input.GetButtonDown("LightHit");
		bool inputDeath = Input.GetButtonDown("Death");
		bool inputUnarmed = Input.GetButtonDown("Unarmed");
		bool inputShield = Input.GetButtonDown("Shield");
		bool inputAttackL = Input.GetButtonDown("AttackL");
		bool inputAttackR = Input.GetButtonDown("AttackR");
		bool inputCastL = Input.GetButtonDown("CastL");
		bool inputCastR = Input.GetButtonDown("CastR");
		float inputSwitchUpDown = Input.GetAxisRaw("SwitchUpDown");
		float inputSwitchLeftRight = Input.GetAxisRaw("SwitchLeftRight");
		bool inputStrafe = Input.GetKey(KeyCode.LeftShift);
		float inputTargetBlock = Input.GetAxisRaw("TargetBlock");
		float inputDashVertical = Input.GetAxisRaw("DashVertical");
		float inputDashHorizontal = Input.GetAxisRaw("DashHorizontal");
		float inputHorizontal = Input.GetAxisRaw("Horizontal");
		float inputVertical = Input.GetAxisRaw("Vertical");
		//Camera relative movement
		Transform cameraTransform = sceneCamera.transform;
		//Forward vector relative to the camera along the x-z plane   
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		//Right vector relative to the camera always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		//directional inputs
//		dv = inputDashVertical;
//		dh = inputDashHorizontal;
//		if(!isRolling && !isAiming)
//		{
//			targetDashDirection = dh * right + dv * -forward;
//		}
//		x = inputHorizontal;
//		z = inputVertical;
//		inputVec = x * right + z * forward;

		if (GetComponent<Player> ().playerNo == 1) {
			x = Input.GetAxisRaw ("Horizontal");
			z = Input.GetAxisRaw ("Vertical");
			if (!isRolling && !isAiming)
				targetDashDirection = transform.forward;
		} else {
			x = Input.GetAxisRaw ("HorizontalPlayer2");
			z = Input.GetAxisRaw ("VerticalPlayer2");
			if(!isRolling && !isAiming)
				targetDashDirection = transform.forward;
		}
		inputVec =  x * right + z * forward;


		//make sure there is animator on character
		if(animator)
		{
			if(canMove && !isBlocking && !isDead)
			{
				//if ((GetComponent<Player> ().playerNo == 1 && isKeyboard ()) || (GetComponent<Player> ().playerNo == 2 && !isKeyboard ()))
					MovementInput ();
				//else
				//	inputVec = new Vector3 (0, 0, 0);
			}
			//Rolling();
			Jumping();
            //if(Input.GetButtonDown("LightHit") && canAction && isGrounded && !isBlocking && !isDead)
            //{
            //    GetHit();
            //}
            //if(Input.GetButtonDown("Death") && canAction && isGrounded && !isBlocking)
            //{
            //    if(!isDead)
            //    {
            //        StartCoroutine(_Death());
            //    } 
            //    else
            //    {
            //        StartCoroutine(_Revive());
            //    }
            //}
			if(((Input.GetButtonDown("AttackL") && player.playerNo == 1) || (Input.GetButtonDown("AButtonCtrl1") && player.playerNo == 2)) 
				&& canAction && isGrounded && !isBlocking && !isDead)
			{
				
				if (CheckStamina (6f)) {
					GetComponent<Player> ().Stamina -= 6f;
					Attack(2);
				} else {
					Camera camera = FindObjectOfType<Camera> ();
					Vector3 screenPos = camera.WorldToScreenPoint (transform.position);
					GameObject noSTIcon = (GameObject)Instantiate (NoST, screenPos, Quaternion.identity);
					noSTIcon.transform.SetParent (GameObject.Find ("Canvas").transform);

					noSTIcon.GetComponent<NoSTIcon> ().player = transform;

				}
			}
			if(((Input.GetButtonDown("AttackR")&& player.playerNo == 1) || (Input.GetButtonDown("BButtonCtrl1") && player.playerNo == 2))
				&& canAction && isGrounded && !isBlocking && !isDead)
			{
				
				if (CheckStamina(10f)) {
					GetComponent<Player> ().Stamina -= 10f;
					Rolling ();
				} else {
					Camera camera = FindObjectOfType<Camera> ();
					Vector3 screenPos = camera.WorldToScreenPoint (transform.position);
					GameObject noSTIcon = (GameObject)Instantiate (NoST, screenPos, Quaternion.identity);
					noSTIcon.transform.SetParent (GameObject.Find ("Canvas").transform);

					noSTIcon.GetComponent<NoSTIcon> ().player = transform;

				}
			}
            //if(Input.GetButtonDown("CastL") && canAction && isGrounded && !isBlocking && !isStrafing && !isDead)
            //{
            //    AttackKick(1);
            //}
            //if(Input.GetButtonDown("CastR") && canAction && isGrounded && !isBlocking && !isStrafing && !isDead)
            //{
            //    AttackKick(2);
            //}

			if (((Input.GetButtonDown ("SkillC") && player.playerNo == 1) || (Input.GetButtonDown ("XButtonCtrl1") && player.playerNo == 2))
			    && canAction && isGrounded && !isBlocking && !isDead && !isCasting) {
				if (player.skillCType != Skills.None) {
					if (CheckStamina (player.spellStaminaDrain * player.skillCTime ())) {
						
							isCasting = true;
							isStrafing = true;
							isAiming = false;
							animator.SetBool ("Strafing", true);
							canAction = false;

							isCastingC = true;
							spellCastTime = player.skillCTime ();


							Camera camera = FindObjectOfType<Camera> ();
							Vector3 screenPos = camera.WorldToScreenPoint (transform.position);
							GameObject bar = (GameObject)Instantiate (castBarPrefab, screenPos, Quaternion.identity);
							bar.transform.SetParent (GameObject.Find ("Canvas").transform);

							castBar = bar;
						
					} else {
						Camera camera = FindObjectOfType<Camera> ();
						Vector3 screenPos = camera.WorldToScreenPoint (transform.position);
						GameObject noSTIcon = (GameObject)Instantiate (NoST, screenPos, Quaternion.identity);
						noSTIcon.transform.SetParent (GameObject.Find ("Canvas").transform);

						noSTIcon.GetComponent<NoSTIcon> ().player = transform;

					}
				}
			} else if (((Input.GetButtonUp ("SkillC") && player.playerNo == 1) || (Input.GetButtonUp ("XButtonCtrl1") && player.playerNo == 2)) && isCastingC) {
				if (canCast) {
					
					CastAttack (1);
					player.skillC ();
				} else {
					StartCoroutine(_LockCasting(0, 0f));
				}
			}

			if (((Input.GetButtonDown("SkillV") && player.playerNo == 1) || (Input.GetButtonDown("YButtonCtrl1") && player.playerNo == 2))
				&& canAction && isGrounded && !isBlocking && !isDead && !isCasting) 
			{
				if (player.skillVType != Skills.None) {
					if (CheckStamina(player.spellStaminaDrain * player.skillVTime())) {
						
							isCasting = true;
							isStrafing = true;
							isAiming = false;
							isCharging = true;
							animator.SetBool ("Strafing", true);

							isCastingV = true;
							spellCastTime = player.skillVTime();

							Camera camera = FindObjectOfType<Camera>();
							Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
							GameObject bar = (GameObject)Instantiate(castBarPrefab, screenPos, Quaternion.identity);
							bar.transform.SetParent(GameObject.Find("Canvas").transform);

							castBar = bar;
						
					} else {
						Camera camera = FindObjectOfType<Camera> ();
						Vector3 screenPos = camera.WorldToScreenPoint (transform.position);
						GameObject noSTIcon = (GameObject)Instantiate (NoST, screenPos + new Vector3(0,999,0), Quaternion.identity);
						noSTIcon.transform.SetParent (GameObject.Find ("Canvas").transform);

						noSTIcon.GetComponent<NoSTIcon> ().player = transform;

					}
				}
			} else if (((Input.GetButtonUp ("SkillV") && player.playerNo == 1) || (Input.GetButtonUp ("YButtonCtrl1") && player.playerNo == 2)) && isCastingV) {

				if (canCast) {
					CastAttack (1);
					player.skillV ();
				} else {
					StartCoroutine(_LockCasting(0, 0f));
				}

			}

			if (inputCastL && canAction && isGrounded && isBlocking) {
				StartCoroutine (_BlockBreak ());
			}
            if (((Input.GetButtonDown("SwapWep") && player.playerNo == 1) ||
				(weaponToggle && DPadYAxis() && player.playerNo == 2)) && canAction && isGrounded && !isBlocking && !isDead)
            {

				//player.SwapWeapon ();
				if (currentWeapon.GetComponent<Weapon>().type == Weapon.Types.Staff)
					StartCoroutine(_SwitchWeapon(19));
				else 
					StartCoroutine(_SwitchWeapon(20));
			}

			//if strafing while rifle, aim
			if((inputStrafe || inputTargetBlock > .1 && canAction) && weapon == WeaponType.RIFLE)
			{  
				isAiming = true;
			}

            if (DPadYAxis() && weaponToggle)
                weaponToggle = false;
            if (DPadYAxis() == false && !weaponToggle)
                weaponToggle = true;
            
			//if strafing
			/*if(Input.GetKey(KeyCode.LeftShift) || Input.GetAxisRaw("TargetBlock") > .1 && canAction)
			{  
				isStrafing = true;
				isAiming = false;
				animator.SetBool("Strafing", true);
				if(inputCastL && canAction && isGrounded && !isBlocking)
				{
					CastAttack(1);
				}
				if(inputCastR && canAction && isGrounded && !isBlocking)
				{
					CastAttack(2);
				}
			}
			else
			{
				isAiming = false;
				isStrafing = false;
				animator.SetBool("Strafing", false);
			}*/

//			if (Input.GetKeyDown (KeyCode.LeftShift) && canAction) {
//
//				isStrafing = !isStrafing;
//				animator.SetBool("Strafing", isStrafing);
//
//				if (isStrafing) {
//					target = transform.GetComponent<Player> ().FindTarget ();
//					if (target == null) {
//						isStrafing = false;
//
//					}
//				} else {
//					transform.GetComponent<Player> ().ResetTarget ();
//					target = null;
//				}
//			}

			if (isCasting) {

				Camera camera = FindObjectOfType<Camera>();
				Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
				castBar.transform.position = screenPos + new Vector3 (-20, 35, 0);

				if (castTime < castTimeMax) {
					castTime += Time.deltaTime * castTimeMax/spellCastTime;
					castBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (castTime, castBar.GetComponent<RectTransform> ().rect.height);
					player.RecoverStamina (Time.deltaTime * -player.spellStaminaDrain);
				}
				else if (castTime > castTimeMax) {
					castTime = castTimeMax;
					castBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (castTime, castBar.GetComponent<RectTransform> ().rect.height);
					canCast = true;
					isCharging = false;
					castBar.GetComponent<Image> ().color = Color.green;
				}




			}
			}
			else
			{
				Debug.Log("ERROR: There is no animator for character.");
			}
	}



	void MovementInput()
	{
		if (GetComponent<Player> ().playerNo == 1) {
			x = Input.GetAxisRaw ("Horizontal");
			z = Input.GetAxisRaw ("Vertical");
		} else {
			x = Input.GetAxisRaw ("HorizontalPlayer2");
			z = Input.GetAxisRaw ("VerticalPlayer2");

		}
		inputVec = new Vector3(x, 0, z);
	}

    bool DPadYAxis()
    {
        float yAxis = Input.GetAxis("DPadYCtrl1");

        if (yAxis > 0)
        {
            weaponToggle = false;
            return true;
        }
        else
        {
            weaponToggle = true;
            return false;
        }
    }
	#endregion

	#region Fixed/Late Updates
	
	void FixedUpdate()
	{
		if(!isSwimming)
		{
			CheckForGrounded();
			//apply gravity force
			rb.AddForce(0, gravity, 0, ForceMode.Acceleration);
			AirControl();
			//check if character can move
			if(canMove && !isBlocking)
			{
				moveSpeed = UpdateMovement();  
			}
			//check if falling
			if(rb.velocity.y < fallingVelocity)
			{
				isFalling = true;
				animator.SetInteger("Jumping", 2);
				canJump = false;
			} 
			else
			{
				isFalling = false;
			}
		} 
		else
		{
			WaterControl();
			moveSpeed = UpdateMovement();
		}
	}

	//get velocity of rigid body and pass the value to the animator to control the animations
	void LateUpdate()
	{
		bool inputAiming = Input.GetButtonDown("Aiming");
		//Get local velocity of charcter
		float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
		float velocityZel = transform.InverseTransformDirection(rb.velocity).z;
		//Update animator with movement values
		animator.SetFloat("Velocity X", velocityXel / runSpeed);
		animator.SetFloat("Velocity Z", velocityZel / runSpeed);
		//if character is alive and can move, set our animator
		if(!isDead && canMove)
		{
			if(moveSpeed > 0)
			{
				animator.SetBool("Moving", true);
				isMoving = true;
			}
			else
			{
				animator.SetBool("Moving", false);
				isMoving = false;
			}
		}
		if(inputAiming)
		{
			if(!isAiming)
			{
				isAiming = true;
			}
			else
			{
				isAiming = false;
			}
		}
	}
	
	#endregion

	#region UpdateMovement

	//rotate character towards direction moved
	void RotateTowardsMovementDir()
	{
		if(inputVec != Vector3.zero && !isStrafing && !isRolling && !isBlocking)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
		}
	}

	float UpdateMovement()
	{
		  
		Vector3 motion = inputVec;
		if(isGrounded)
		{
			//reduce input for diagonal movement
			if(motion.magnitude > 1)
			{
				motion.Normalize();
			}
			if(canMove && !isBlocking)
			{
				//set speed by walking / running
				if(isStrafing && !isAiming)
				{
					newVelocity = motion * walkSpeed;
				}
				else
				{
					newVelocity = motion * runSpeed;
				}
				//if rolling use rolling speed and direction
				if(isRolling)
				{
					//force the dash movement to 1
					targetDashDirection.Normalize();
					newVelocity = rollSpeed * targetDashDirection;
				}
			}
		}
		else
		{
			if(!isSwimming)
			{
				//if we are falling use momentum
				newVelocity = rb.velocity;
			} 
			else
			{
				newVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			}
		}
		if(!isStrafing || !canMove || !isAiming)
		{
			RotateTowardsMovementDir();
		}
		if(isAiming)
		{
			Aiming();
			animator.SetBool("Aiming", true);
		}
		else
		{
			animator.SetBool("Aiming", false);
		}
		if(isStrafing && !isRelax)
		{
			//make character point at target
			Quaternion targetRotation;
			//Vector3 targetPos = target.transform.position;
			Vector3 targetPos = transform.position + transform.forward;
      	targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x,0,transform.position.z));
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,(rotationSpeed * Time.deltaTime) * rotationSpeed);
		}
		//if we are falling use momentum
		newVelocity.y = rb.velocity.y;
		rb.velocity = newVelocity;
		//return a movement value for the animator
		return inputVec.magnitude;
	}

	#endregion

	#region Aiming

	void Aiming()
	{
		Debug.Log("Aiming");
		for(int i =0; i<Input.GetJoystickNames().Length; i++) 
		{
			if(!Input.GetJoystickNames()[i].Contains("Controller"))
			{
				//no joysticks, use mouse aim
				Plane characterPlane = new Plane(Vector3.up, transform.position);
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Vector3 mousePosition = new Vector3(0,0,0);
				float hitdist = 0.0f;
				if(characterPlane.Raycast(ray, out hitdist))
				{
					mousePosition = ray.GetPoint(hitdist);
				}
				mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
				Vector3 relativePos = transform.position - mousePosition;
				Quaternion rotation = Quaternion.LookRotation(-relativePos);
				transform.rotation = rotation;
			}
			else
			{
				//if the right joystick is moved, use that for facing
				float inputDashVertical = Input.GetAxisRaw("DashVertical");
				float inputDashHorizontal = Input.GetAxisRaw("DashHorizontal");
				if(Mathf.Abs(inputDashHorizontal) > .1 || Mathf.Abs(inputDashVertical) > .1)
				{
					Vector3 joyDirection = new Vector3(inputDashHorizontal, 0 , -inputDashVertical);
					joyDirection = joyDirection.normalized;
					Quaternion joyRotation = Quaternion.LookRotation(joyDirection);
					transform.rotation = joyRotation;
				}
			}
		}
	}

	#endregion

	#region Swimming

	void OnTriggerEnter(Collider collide)
	{
		//If entering a water volume
		if(collide.gameObject.layer == 4)
		{
			isSwimming = true;
			canAction = false;
			rb.useGravity = false;
			animator.SetTrigger("SwimTrigger");
			animator.SetBool("Swimming", true);
			animator.SetInteger("Weapon", 0);
			StartCoroutine(_WeaponVisibility(leftWeapon, 0, false));
			StartCoroutine(_WeaponVisibility(rightWeapon, 0, false));
			animator.SetInteger("RightWeapon", 0);
			animator.SetInteger("LeftWeapon", 0);
			animator.SetInteger("LeftRight", 0);
			FXSplash.Emit(30);
		}
	}

	void OnTriggerExit(Collider collide)
	{
		//If leaving a water volume
		if(collide.gameObject.layer == 4)
		{
			isSwimming = false;
			canAction = true;
			rb.useGravity = true;
			animator.SetInteger("Jumping", 2);
			animator.SetBool("Swimming", false);
			capCollider.radius = .5f;
		}
	}

	void WaterControl()
	{
		AscendDescend();
		Vector3 motion = inputVec;
		//dampen vertical water movement
		Vector3 dampenVertical = new Vector3(rb.velocity.x, (rb.velocity.y * .985f), rb.velocity.z);
		rb.velocity = dampenVertical;
		Vector3 waterDampen = new Vector3((rb.velocity.x * .98f), rb.velocity.y, (rb.velocity.z * .98f));
		//If swimming, don't dampen movement, and scale capsule collider
		if(moveSpeed < .1f)
		{
			rb.velocity = waterDampen;
			capCollider.radius = .5f;
		} 
		else
		{
			capCollider.radius = 1.5f;
		}
		rb.velocity = waterDampen;
		//clamp diagonal movement so its not faster
		motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
		rb.AddForce(motion * inWaterSpeed, ForceMode.Acceleration);
		//limit the amount of velocity we can achieve to water speed
		float velocityX = 0;
		float velocityZ = 0;
		if(rb.velocity.x > inWaterSpeed)
		{
			velocityX = GetComponent<Rigidbody>().velocity.x - inWaterSpeed;
			if(velocityX < 0)
			{
				velocityX = 0;
			}
			rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
		}
		if(rb.velocity.x < minVelocity)
		{
			velocityX = rb.velocity.x - minVelocity;
			if(velocityX > 0)
			{
				velocityX = 0;
			}
			rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
		}
		if(rb.velocity.z > inWaterSpeed)
		{
			velocityZ = rb.velocity.z - maxVelocity;
			if(velocityZ < 0)
			{
				velocityZ = 0;
			}
			rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
		}
		if(rb.velocity.z < minVelocity)
		{
			velocityZ = rb.velocity.z - minVelocity;
			if(velocityZ > 0)
			{
				velocityZ = 0;
			}
			rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
		}
	}

	void AscendDescend()
	{
		if(doJump)
		{
			//swim down with left control
			if(isStrafing)
			{
				animator.SetBool("Strafing", true);
				animator.SetTrigger("JumpTrigger");
				rb.velocity -=  inWaterSpeed * Vector3.up;
			} 
			else
			{
				animator.SetTrigger("JumpTrigger");
				rb.velocity +=  inWaterSpeed * Vector3.up;
			}
		}
	}

	#endregion

	#region Jumping

	//checks if character is within a certain distance from the ground, and markes it IsGrounded
	void CheckForGrounded()
	{
		float distanceToGround;
		float threshold = .45f;
		RaycastHit hit;
		Vector3 offset = new Vector3(0,.4f,0);
		if(Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
		{
			distanceToGround = hit.distance;
			if(distanceToGround < threshold)
			{
				isGrounded = true;
				canJump = true;
				startFall = false;
				doublejumped = false;
				canDoubleJump = false;
				isFalling = false;
				if(!isJumping) 
				{
					animator.SetInteger("Jumping", 0);
				}
			}
			else
			{
				isGrounded = false;
			}
		}
	}

	void Jumping()
	{
		if(isGrounded)
		{
			if(canJump && doJump)
			{
				StartCoroutine(_Jump());
			}
		}
		else
		{    
			canDoubleJump = true;
			canJump = false;
			if(isFalling)
			{
				//set the animation back to falling
				animator.SetInteger("Jumping", 2);
				//prevent from going into land animation while in air
				if(!startFall)
				{
					animator.SetTrigger("JumpTrigger");
					startFall = true;
				}
			}
			if(canDoubleJump && doublejumping && Input.GetButtonDown("Jump") && !doublejumped && isFalling)
			{
				// Apply the current movement to launch velocity
				rb.velocity += doublejumpSpeed * Vector3.up;
				animator.SetInteger("Jumping", 3);
				doublejumped = true;
			}
		}
	}

	IEnumerator _Jump()
	{
		isJumping = true;
		animator.SetInteger("Jumping", 1);
		animator.SetTrigger("JumpTrigger");
		// Apply the current movement to launch velocity
		rb.velocity += jumpSpeed * Vector3.up;
		canJump = false;
		yield return new WaitForSeconds(.5f);
		isJumping = false;
	}

	void AirControl()
	{
		if(!isGrounded)
		{
			MovementInput();
			Vector3 motion = inputVec;
			motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?0.7f:1;
			rb.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
			//limit the amount of velocity we can achieve
			float velocityX = 0;
			float velocityZ = 0;
			if(rb.velocity.x > maxVelocity)
			{
				velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
				if(velocityX < 0)
				{
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.x < minVelocity)
			{
				velocityX = rb.velocity.x - minVelocity;
				if(velocityX > 0)
				{
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.z > maxVelocity)
			{
				velocityZ = rb.velocity.z - maxVelocity;
				if(velocityZ < 0)
				{
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
			if(rb.velocity.z < minVelocity)
			{
				velocityZ = rb.velocity.z - minVelocity;
				if(velocityZ > 0)
				{
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
		}
	}

	#endregion

	#region MiscMethods

	//0 = No side
	//1 = Left
	//2 = Right
	//3 = Dual
	void Attack(int attackSide)
	{
		if(canAction)
		{
			if(weapon == WeaponType.UNARMED || weapon == WeaponType.ARMED)
			{
				int maxAttacks = 3;
				int attackNumber = 0;
				if(attackSide == 1 || attackSide == 3)
				{
					attackNumber = Random.Range(0, maxAttacks);
				}
				else if(attackSide == 2)
				{
					attackNumber = Random.Range(3, maxAttacks + 3);
				}
				if(isGrounded)
				{
					if(attackSide != 3)
					{
						animator.SetTrigger("Attack" + (attackNumber + 1).ToString() + "Trigger");
						if(leftWeapon == 12 || leftWeapon == 14 || rightWeapon == 13 || rightWeapon == 15)
						{
							StartCoroutine(_LockMovementAndAttack(0, .7f));
						} 
						else
						{
							StartCoroutine(_LockMovementAndAttack(0, .7f));
							StartCoroutine(_AttackTrigger(0.1f, 0.6f));
						}
					}
					else
					{
						animator.SetTrigger("AttackDual" + (attackNumber + 1).ToString() + "Trigger");
						StartCoroutine(_LockMovementAndAttack(0, .75f));
					}
				}
			}
			//2 handed weapons
			else
			{
				int maxAttacks = 6;
				if(isGrounded)
				{
					int attackNumber = Random.Range(0, maxAttacks);
					if(isGrounded)
					{
						animator.SetTrigger("Attack" + (attackNumber + 1).ToString() + "Trigger");
						if(weapon == WeaponType.TWOHANDSWORD)
						{
							StartCoroutine(_LockMovementAndAttack(0, .85f));
						}
						else if(weapon == WeaponType.TWOHANDSPEAR)
						{
							StartCoroutine(_LockMovementAndAttack(0, 1.1f));
						}
						else if(weapon == WeaponType.TWOHANDAXE)
						{
							StartCoroutine(_LockMovementAndAttack(0, 1.5f));
						}
						else
						{
							StartCoroutine(_LockMovementAndAttack(0, .75f));
						}
					}
				}
			}
		}
	}

	void AttackKick(int kickSide)
	{
		if(isGrounded)
		{
			if(kickSide == 1)
			{
				animator.SetTrigger("AttackKick1Trigger");
			}
			else
			{
				animator.SetTrigger("AttackKick2Trigger");
			}
			StartCoroutine(_LockMovementAndAttack(0, .8f));
		}
	}

	//0 = No side
	//1 = Left
	//2 = Right
	//3 = Dual
	void CastAttack(int attackSide)
	{
		if(weapon == WeaponType.UNARMED || weapon == WeaponType.STAFF || weapon == WeaponType.ARMED)
		{
			int maxAttacks = 3;
			if(attackSide == 1)
			{
				int attackNumber = Random.Range(0, maxAttacks);
				if(isGrounded)
				{
					animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, .8f));
				}
			}
			if(attackSide == 2)
			{
				int attackNumber = Random.Range(3, maxAttacks + 3);
				if(isGrounded)
				{
					animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, .8f));
				}
			}
			if(attackSide == 3)
			{
				int attackNumber = Random.Range(0, maxAttacks);
				if(isGrounded)
				{
					animator.SetTrigger("CastDualAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, 1f));
				}
			}

			StartCoroutine(_LockCasting(0, .8f));
		} 
	}

	void Blocking()
	{
		if(blockGui || Input.GetAxisRaw("TargetBlock") < -.1 && canAction && isGrounded)
		{
			if(!isBlocking)
			{
				animator.SetTrigger("BlockTrigger");
			}
			isBlocking = true;
			canJump = false;
			animator.SetBool("Blocking", true);
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			inputVec = Vector3.zero;
		}
		else
		{
			isBlocking = false;
			canJump = true;
			animator.SetBool("Blocking", false);
		}
	}

	public void GetHit()
	{
		if(weapon != WeaponType.RIFLE)
		{
			int hits = 5;
			int hitNumber = Random.Range(0, hits);
			animator.SetTrigger("GetHit" + (hitNumber + 1).ToString()+ "Trigger");
			StartCoroutine(_LockMovementAndAttack(.1f, .2f));
			//apply directional knockback force
			if(hitNumber <= 1)
			{
				StartCoroutine(_Knockback(-transform.forward, 1, 0));
			} 
			else if(hitNumber == 2)
			{
				StartCoroutine(_Knockback(transform.forward, 1, 0));
			}
			else if(hitNumber == 3)
			{
				StartCoroutine(_Knockback(transform.right, 1, 0));
			}
			else if(hitNumber == 4)
			{
				StartCoroutine(_Knockback(-transform.right, 1, 0));
			}
		}
		else
		{
			animator.SetTrigger("GetHit1Trigger");
		}
	}

	IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount)
	{
		isKnockback = true;
		StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
		yield return new WaitForSeconds(.1f);
		isKnockback = false;
	}

	IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount)
	{
		while(isKnockback)
		{
			rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount))
				* (knockbackMultiplier * 10)), ForceMode.Impulse);
			yield return null;
		}
	}

	IEnumerator _Death()
	{
		animator.SetTrigger("Death1Trigger");
		StartCoroutine(_LockMovementAndAttack(.1f, 1.5f));
		isDead = true;
		animator.SetBool("Moving", false);
		inputVec = new Vector3(0, 0, 0);
		yield return null;
	}

	public void PlayerDeath() {

		StartCoroutine (_Death ());

	}

	IEnumerator _Revive()
	{
		animator.SetTrigger("Revive1Trigger");
		isDead = false;
		yield return null;
	}

	public void PlayerRevive() {

		StartCoroutine (_Revive ());
	}

	public bool CheckStamina(float stam) {

		if (stam < player.Stamina)
			return true;
		else
			return false;

	}

	#endregion

	#region Rolling

	void Rolling()
	{
		if(!isRolling && isGrounded && !isAiming)
		{
			if(Input.GetAxis("DashVertical") > .5 || Input.GetAxis("DashVertical") < -.5 || Input.GetAxis("DashHorizontal") > .5 || Input.GetAxis("DashHorizontal") < -.5)
			{
				//StartCoroutine(_DirectionalRoll(Input.GetAxis("DashVertical"), Input.GetAxis("DashHorizontal")));

			}
			StartCoroutine(_DirectionalRoll(x, z));

		}
	}

	public IEnumerator _DirectionalRoll(float x, float v)
	{
		//check which way the dash is pressed relative to the character facing
		float angle = Vector3.Angle(targetDashDirection,-transform.forward);
		float sign = Mathf.Sign(Vector3.Dot(transform.up,Vector3.Cross(targetDashDirection,transform.forward)));
		// angle in [-179,180]
		float signed_angle = angle * sign;
		//angle in 0-360
		float angle360 = (signed_angle + 180) % 360;
		//deternime the animation to play based on the angle
		if(angle360 > 315 || angle360 < 45)
		{
			StartCoroutine(_Roll(1));
		}
		if(angle360 > 45 && angle360 < 135)
		{
			StartCoroutine(_Roll(2));
		}
		if(angle360 > 135 && angle360 < 225)
		{
			StartCoroutine(_Roll(3));
		}
		if(angle360 > 225 && angle360 < 315)
		{
			StartCoroutine(_Roll(4));
		}
		yield return null;
	}

	IEnumerator _Roll(int rollNumber)
	{
		if(rollNumber == 1)
		{
			animator.SetTrigger("RollForwardTrigger");
		}
		if(rollNumber == 2)
		{
			animator.SetTrigger("RollRightTrigger");
		}
		if(rollNumber == 3)
		{
			animator.SetTrigger("RollBackwardTrigger");
		}
		if(rollNumber == 4)
		{
			animator.SetTrigger("RollLeftTrigger");
		}
		isRolling = true;
		player.canBeHit = false;
		canAction = false;
		yield return new WaitForSeconds(rollduration);
		isRolling = false;
		player.canBeHit = true;
		canAction = true;
	}

	#endregion


	#region _Coroutines

	//method to keep character from moveing while attacking, etc
	public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
	{
		yield return new WaitForSeconds(delayTime);
		canAction = false;
		canMove = false;
		animator.SetBool("Moving", false);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		inputVec = new Vector3(0, 0, 0);
		animator.applyRootMotion = true;


		yield return new WaitForSeconds(lockTime);
		canAction = true;
		canMove = true;
		animator.applyRootMotion = false;

	}

	public IEnumerator _LockCasting(float delayTime, float lockTime) {

		yield return new WaitForSeconds(delayTime);
		canCast = false;
		canAction = false;
		yield return new WaitForSeconds(lockTime);

		isCasting = false;
		isStrafing = false;
		isAiming = false;
		animator.SetBool ("Strafing", false);

		isCastingC = false;
		isCastingV = false;

		Destroy (castBar);
		castTime = 0;
		//castTimeMax = 0;

		isCharging = false;
		canAction = true;

	}

	IEnumerator _AttackTrigger(float delayTime, float triggerTime)
	{
		yield return new WaitForSeconds(delayTime);
		player.currentWeapon.AttackTrigger (1);
		yield return new WaitForSeconds(triggerTime);
		player.currentWeapon.AttackTrigger (0);
	}

	//for controller weapon switching
	void SwitchWeaponTwoHand(int upDown)
	{
		canAction = false;
		int weaponSwitch = (int)weapon;
		if(upDown == 0)
		{
			weaponSwitch--;
			if(weaponSwitch < 1)
			{
				StartCoroutine(_SwitchWeapon(6));
			} 
			else
			{
				StartCoroutine(_SwitchWeapon(weaponSwitch));
			}
		}
		if(upDown == 1)
		{
			weaponSwitch++;
			if(weaponSwitch > 6)
			{
				StartCoroutine(_SwitchWeapon(1));
			} 
			else
			{
				StartCoroutine(_SwitchWeapon(weaponSwitch));
			}
		}
	}

	//for controller weapon switching
	void SwitchWeaponLeftRight(int upDown)
	{
		int weaponSwitch = 0;
		canAction = false;
		if(upDown == 0)
		{
			weaponSwitch = leftWeapon;
			if(weaponSwitch < 16 && weaponSwitch != 0 && leftWeapon != 7)
			{
				weaponSwitch += 2;
			}
			else
			{
				weaponSwitch = 8;
			}
		}
		if(upDown == 1)
		{
			weaponSwitch = rightWeapon;
			if(weaponSwitch < 17 && weaponSwitch != 0)
			{
				weaponSwitch += 2;
			}
			else
			{
				weaponSwitch = 9;
			}
		}
		StartCoroutine(_SwitchWeapon(weaponSwitch));
	}

	//function to switch weapons
	//weaponNumber 0 = Unarmed
	//weaponNumber 1 = 2H Sword
	//weaponNumber 2 = 2H Spear
	//weaponNumber 3 = 2H Axe
	//weaponNumber 4 = 2H Bow
	//weaponNumber 5 = 2H Crowwbow
	//weaponNumber 6 = 2H Staff
	//weaponNumber 7 = Shield
	//weaponNumber 8 = L Sword
	//weaponNumber 9 = R Sword
	//weaponNumber 10 = L Mace
	//weaponNumber 11 = R Mace
	//weaponNumber 12 = L Dagger
	//weaponNumber 13 = R Dagger
	//weaponNumber 14 = L Item
	//weaponNumber 15 = R Item
	//weaponNumber 16 = L Pistol
	//weaponNumber 17 = R Pistol
	//weaponNumber 18 = Rifle
	//weaponNumber 19 = firstWep
	//weaponNumber 20 = secondWep
	public IEnumerator _SwitchWeapon(int weaponNumber)
	{	
		//character is unarmed
		if(weapon == WeaponType.UNARMED)
		{
			StartCoroutine(_UnSheathWeapon(weaponNumber));
		}
		//character has 2 handed weapon
		else if(weapon == WeaponType.STAFF || weapon == WeaponType.TWOHANDAXE || weapon == WeaponType.TWOHANDBOW || 
			weapon == WeaponType.TWOHANDCROSSBOW || weapon == WeaponType.TWOHANDSPEAR ||
			weapon == WeaponType.TWOHANDSWORD || weapon == WeaponType.RIFLE)
		{
			StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
			yield return new WaitForSeconds(1.1f);
			if(weaponNumber > 0)
			{
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switch to unarmed
			else
			{
				weapon = WeaponType.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
		}
		//character has 1 or 2 1hand weapons and/or shield
		else if(weapon == WeaponType.ARMED)
		{
			
			//character is switching to 2 hand weapon or unarmed, put put away all weapons
			if(weaponNumber < 7 || weaponNumber == 18)
			{
				//check left hand for weapon
				if(leftWeapon != 0)
				{
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f);
					if(rightWeapon != 0)
					{
						StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
						//and right hand weapon
						if(weaponNumber != 0)
						{
							StartCoroutine(_UnSheathWeapon(weaponNumber));
						}
					}
					if(weaponNumber != 0)
					{
						StartCoroutine(_UnSheathWeapon(weaponNumber));
					}
				}
				//check right hand for weapon if no left hand weapon
				if(rightWeapon != 0)
				{
					StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f);
					if(weaponNumber != 0)
					{
						StartCoroutine(_UnSheathWeapon(weaponNumber));
					}
				}
			}
			//using 1 handed weapon(s)
			else if(weaponNumber == 7)
			{
				if(leftWeapon > 0)
				{
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f);
				}
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switching left weapon, put away left weapon if equipped
			else if((weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16))
			{
				if(leftWeapon > 0)
				{
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f);
				}
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switching right weapon, put away right weapon if equipped
			else if((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 ||
				weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19 || weaponNumber == 20))
			{
//				if(rightWeapon > 0)
//				{
//					StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
//					yield return new WaitForSeconds(1.05f);
//				}
//				StartCoroutine(_UnSheathWeapon(weaponNumber));
				StartCoroutine(_ChangeWeapon(rightWeapon, weaponNumber));
			}
		}
		yield return null;
	}

	public IEnumerator _SheathWeapon(int weaponNumber, int weaponDraw)
	{
		if((weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16))
		{
			animator.SetInteger("LeftRight", 1);
		}
		else if((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19 || weaponNumber == 20))
		{
			animator.SetInteger("LeftRight", 2);
		}
		if(weaponDraw == 0)
		{
			//if switching to unarmed, don't set "Armed" until after 2nd weapon sheath
			if(leftWeapon == 0 && rightWeapon != 0)
			{
				animator.SetBool("Armed", false);
			}
			if(rightWeapon == 0 && leftWeapon != 0)
			{
				animator.SetBool("Armed", false);
			}
		}
		animator.SetTrigger("WeaponSheathTrigger");
		yield return new WaitForSeconds(.1f);
		if(weaponNumber < 7 || weaponNumber == 18)
		{
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
			rightWeapon = 0;
			animator.SetInteger("RightWeapon", 0);
			animator.SetBool("Shield", false);
			animator.SetBool("Armed", false);
		}
		else if(weaponNumber == 7)
		{
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
			animator.SetBool("Shield", false);
		}
		else if((weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 ||
			weaponNumber == 14 || weaponNumber == 16))
		{
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
		}
		else if((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 ||
			weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19 || weaponNumber == 20))
		{
			rightWeapon = 0;
			animator.SetInteger("RightWeapon", 0);
		}
		//if switched to unarmed
		if(leftWeapon == 0 && rightWeapon == 0)
		{
			animator.SetBool("Armed", false);
		}
		if(leftWeapon == 0 && rightWeapon == 0)
		{
			animator.SetInteger("LeftRight", 0);
			animator.SetInteger("Weapon", 0);
			animator.SetBool("Armed", false);
			weapon = WeaponType.UNARMED;
		}
		StartCoroutine(_WeaponVisibility(weaponNumber, .4f, false));
		StartCoroutine(_LockMovementAndAttack(0, 1));
		yield return null;
	}
		
	public IEnumerator _UnSheathWeapon(int weaponNumber)
	{
		animator.SetInteger("Weapon", -1);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		//two handed weapons
		if(weaponNumber < 7 || weaponNumber == 18)
		{
			leftWeapon = weaponNumber;
			animator.SetInteger("LeftRight", 3);
			if(weaponNumber == 0)
			{
				weapon = WeaponType.UNARMED;
			}
			if(weaponNumber == 1)
			{
				weapon = WeaponType.TWOHANDSWORD;
				StartCoroutine(_WeaponVisibility(weaponNumber, .4f, true));
			}
			else if(weaponNumber == 2)
			{
				weapon = WeaponType.TWOHANDSPEAR;
				StartCoroutine(_WeaponVisibility(weaponNumber, .5f, true));
			}
			else if(weaponNumber == 3)
			{
				weapon = WeaponType.TWOHANDAXE;
				StartCoroutine(_WeaponVisibility(weaponNumber, .5f, true));
			}
			else if(weaponNumber == 4)
			{
				weapon = WeaponType.TWOHANDBOW;
				StartCoroutine(_WeaponVisibility(weaponNumber, .55f, true));
			}
			else if(weaponNumber == 5)
			{
				weapon = WeaponType.TWOHANDCROSSBOW;
				StartCoroutine(_WeaponVisibility(weaponNumber, .5f, true));
			}
			else if(weaponNumber == 6)
			{
				weapon = WeaponType.STAFF;
				StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
			}
			else if(weaponNumber == 18)
			{
				weapon = WeaponType.RIFLE;
				StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
			}
		}
		//one handed weapons
		else
		{
			if(weaponNumber == 7)
			{
				leftWeapon = 7;
				animator.SetInteger("LeftWeapon", 7);
				animator.SetInteger("LeftRight", 1);
				StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
				animator.SetBool("Shield", true);
			}
			else if(weaponNumber == 8 || weaponNumber == 10 || 
				weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16)
			{
				animator.SetInteger("LeftRight", 1);
				animator.SetInteger("LeftWeapon", weaponNumber);
				StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
				leftWeapon = weaponNumber;
				weaponNumber = 7;
			}
			else if(weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 ||
				weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19 || weaponNumber == 20)
			{
				animator.SetInteger("LeftRight", 2);
				if (weaponNumber < 19)
				animator.SetInteger("RightWeapon", weaponNumber);
				else if (weaponNumber == 19) {
					if (firstWep.GetComponent<Weapon>().type == Weapon.Types.Staff)
						animator.SetInteger("RightWeapon", 11);
					else
						animator.SetInteger("RightWeapon", 9);
				}
				else {
					if (secondWep.GetComponent<Weapon>().type == Weapon.Types.Staff)
						animator.SetInteger("RightWeapon", 11);
					else
						animator.SetInteger("RightWeapon", 9);
				}

				rightWeapon = weaponNumber;
				StartCoroutine(_WeaponVisibility(weaponNumber, .6f, true));
				weaponNumber = 7;
				//set shield to false for animator, will reset later
				if(leftWeapon == 7)
				{
					animator.SetBool("Shield", false);
				}
			}
		}
		if(weapon == WeaponType.RIFLE)
		{
			animator.SetInteger("Weapon", 8);
		} 
		else
		{
			animator.SetInteger("Weapon", weaponNumber);
		}
		animator.SetTrigger("WeaponUnsheathTrigger");
		StartCoroutine(_LockMovementAndAttack(0, 1.1f));
		yield return new WaitForSeconds(.1f);
		if(leftWeapon == 7)
		{
			animator.SetBool("Shield", true);
		}
		if((leftWeapon > 6 || rightWeapon > 6) && weapon != WeaponType.RIFLE)
		{
			animator.SetBool("Armed", true);
			weapon = WeaponType.ARMED;
		}
		//For dual blocking
		if(rightWeapon == 9 || rightWeapon == 11 || rightWeapon == 13 || rightWeapon == 15 ||
			rightWeapon == 17 || weaponNumber == 19 || weaponNumber == 20)
		{
			if(leftWeapon == 8 || leftWeapon == 10 || leftWeapon == 12 || leftWeapon == 14 || leftWeapon == 16)
			{
				yield return new WaitForSeconds(.1f);
				animator.SetInteger("LeftRight", 3);
			}
		}
		if(leftWeapon == 8 || leftWeapon == 10 || leftWeapon == 12 || leftWeapon == 14 || leftWeapon == 16)
		{
			if(rightWeapon == 9 || rightWeapon == 11 || rightWeapon == 13 || rightWeapon == 15 ||
				rightWeapon == 17 || weaponNumber == 19 || weaponNumber == 20)
			{
				yield return new WaitForSeconds(.1f);
				animator.SetInteger("LeftRight", 3);
			}
		}
		yield return null;
	}

	public IEnumerator _ChangeWeapon(int currWeapon, int weaponNumber) {

		animator.SetInteger("LeftRight", 2);
		animator.SetTrigger("WeaponSheathTrigger");
		yield return new WaitForSeconds(.1f);
		rightWeapon = 0;
		animator.SetInteger("RightWeapon", 0);
		StartCoroutine(_WeaponVisibility(currWeapon, .3f, false));
		StartCoroutine(_LockMovementAndAttack(0, 0.6f));
//		animator.SetInteger("Weapon", -1);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		animator.SetInteger("LeftRight", 2);
		if (weaponNumber == 19) {
			if (firstWep.GetComponent<Weapon>().type == Weapon.Types.Staff)
				animator.SetInteger("RightWeapon", 11);
			else
				animator.SetInteger("RightWeapon", 9);
		}
		else {
			if (secondWep.GetComponent<Weapon>().type == Weapon.Types.Staff)
				animator.SetInteger("RightWeapon", 11);
			else
				animator.SetInteger("RightWeapon", 9);
		}

		rightWeapon = weaponNumber;
		StartCoroutine(_WeaponVisibility(weaponNumber, .4f, true));
		weaponNumber = 7;

		animator.SetInteger("Weapon", weaponNumber);
		animator.SetBool("Armed", true);
		weapon = WeaponType.ARMED;
//
//		yield return new WaitForSeconds(.1f);
//		animator.SetInteger("LeftRight", 3);
		yield return null;
	}


	public IEnumerator _WeaponVisibility(int weaponNumber, float delayTime, bool visible)
	{
		yield return new WaitForSeconds(delayTime);
		if(weaponNumber == 1) 
		{
			twohandsword.SetActive(visible);
		}
		if(weaponNumber == 2) 
		{
			twohandspear.SetActive(visible);
		}
		if(weaponNumber == 3) 
		{
			twohandaxe.SetActive(visible);
		}
		if(weaponNumber == 4) 
		{
			twohandbow.SetActive(visible);
		}
		if(weaponNumber == 5) 
		{
			twohandcrossbow.SetActive(visible);
		}
		if(weaponNumber == 6) 
		{
			staff.SetActive(visible);
		}
		if(weaponNumber == 7) 
		{
			shield.SetActive(visible);
		}
		if(weaponNumber == 8) 
		{
			swordL.SetActive(visible);
		}
		if(weaponNumber == 9) 
		{
			swordR.SetActive(visible);
		}
		if(weaponNumber == 10) 
		{
			maceL.SetActive(visible);
		}
		if(weaponNumber == 11) 
		{
			maceR.SetActive(visible);
		}
		if(weaponNumber == 12) 
		{
			daggerL.SetActive(visible);
		}
		if(weaponNumber == 13) 
		{
			daggerR.SetActive(visible);
		}
		if(weaponNumber == 14) 
		{
			itemL.SetActive(visible);
		}
		if(weaponNumber == 15) 
		{
			itemR.SetActive(visible);
		}
		if(weaponNumber == 16) 
		{
			pistolL.SetActive(visible);
		}
		if(weaponNumber == 17) 
		{
			pistolR.SetActive(visible);
		}
		if(weaponNumber == 18) 
		{
			rifle.SetActive(visible);
		}
		if(weaponNumber == 19) 
		{
			firstWep.SetActive(visible);
			currentWeapon = firstWep;
			player.currentWeapon = currentWeapon.GetComponent<Weapon> ();
			player.nextWeapon = secondWep.GetComponent<Weapon> ();
		}
		if(weaponNumber == 20) 
		{
			secondWep.SetActive(visible);
			currentWeapon = secondWep;
			player.currentWeapon = currentWeapon.GetComponent<Weapon> ();
			player.nextWeapon = firstWep.GetComponent<Weapon> ();
		}
		yield return null;
	}

	public IEnumerator _BlockHitReact()
	{
		int hits = 2;
		int hitNumber = Random.Range(0, hits);
		animator.SetTrigger("BlockGetHit" + (hitNumber + 1).ToString()+ "Trigger");
		StartCoroutine(_LockMovementAndAttack(.1f, .4f));
		StartCoroutine(_Knockback(-transform.forward, 3, 3));
		yield return null;
	}

	public IEnumerator _BlockBreak()
	{
		animator.applyRootMotion = true;
		animator.SetTrigger("BlockBreakTrigger");
		yield return new WaitForSeconds(1f);
		animator.applyRootMotion = false;
	}
	
	

	#endregion

	/*#region GUI

	void OnGUI()
	{
		if(!isDead)
		{
			if(weapon == WeaponType.RELAX || weapon != WeaponType.UNARMED)
			{
				if(GUI.Button(new Rect(1115, 310, 100, 30), "Unarmed"))
				{
					animator.SetBool("Relax", false);
					isRelax = false;
					StartCoroutine(_SwitchWeapon(0));
					weapon = WeaponType.UNARMED;
					canAction = true;
					animator.SetTrigger("RelaxTrigger");
				}
				if(!isSitting && !isMoving && weapon == WeaponType.RELAX)
				{
					if(GUI.Button(new Rect(1115, 345, 100, 30), "Sit"))
					{
						canAction = false;
						isSitting = true;
						canMove = false;
						animator.SetTrigger("Sit-SitdownTrigger");
					}
				}
				if(isSitting && !isMoving && weapon == WeaponType.RELAX)
				{
					if(GUI.Button(new Rect(1115, 345, 100, 30), "Stand"))
					{
						canAction = false;
						isSitting = false;
						animator.SetTrigger("Sit-StandupTrigger");
						canMove = true;
					}
				}
			}
			if(canAction && !isRelax)
			{
				if(isGrounded)
				{
					//bow and crossbow can't block
					if(weapon != WeaponType.TWOHANDBOW && weapon != WeaponType.TWOHANDCROSSBOW)
					{
						//if character is not blocking
						blockGui = GUI.Toggle(new Rect(25, 215, 100, 30), blockGui, "Block");
					}
					//get hit
					if(blockGui)
					{
						if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit"))
						{
							StartCoroutine(_BlockHitReact());
						}
						if(GUI.Button(new Rect(30, 270, 100, 30), "Block Break"))
						{
							StartCoroutine(_BlockBreak());
						}
					} 
					else if(!isBlocking)
					{
						if(!isBlocking)
						{
							if(GUI.Button(new Rect(25, 15, 100, 30), "Roll Forward"))
							{
								targetDashDirection = transform.forward;
								StartCoroutine(_Roll(1));
							}
							if(GUI.Button(new Rect(130, 15, 100, 30), "Roll Backward"))
							{
								targetDashDirection = -transform.forward;
								StartCoroutine(_Roll(3));
							}
							if(GUI.Button(new Rect(25, 45, 100, 30), "Roll Left"))
							{
								targetDashDirection = -transform.right;
								StartCoroutine(_Roll(4));
							}
							if(GUI.Button(new Rect(130, 45, 100, 30), "Roll Right"))
							{
								targetDashDirection = transform.right;
								StartCoroutine(_Roll(2));
							}
							//ATTACK LEFT
							if(GUI.Button(new Rect(25, 85, 100, 30), "Attack L"))
							{
								Attack(1);
							}
							//ATTACK RIGHT
							if(GUI.Button(new Rect(130, 85, 100, 30), "Attack R"))
							{
								Attack(2);
							}
							if(weapon == WeaponType.UNARMED) 
							{
								if(GUI.Button (new Rect (25, 115, 100, 30), "Left Kick")) 
								{
									AttackKick (1);
								}
								if(GUI.Button (new Rect (130, 115, 100, 30), "Right Kick")) 
								{
									AttackKick (2);
								}
							}
							if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit"))
							{
								GetHit();
							}
							if(weapon == WeaponType.UNARMED && !isMoving)
							{
								if(GUI.Button(new Rect(1115, 310, 100, 30), "Relax"))
								{
									animator.SetBool("Relax", true);
									isRelax = true;
									weapon = WeaponType.RELAX;
									canAction = false;
									animator.SetTrigger("RelaxTrigger");
								}
							}
							if(weapon != WeaponType.TWOHANDSWORD)
							{
								if(GUI.Button(new Rect(1115, 350, 100, 30), "2 Hand Sword"))
								{
									StartCoroutine(_SwitchWeapon(1));
								}
							}
							if(weapon != WeaponType.TWOHANDSPEAR)
							{
								if(GUI.Button(new Rect(1115, 380, 100, 30), "2 Hand Spear"))
								{
									StartCoroutine(_SwitchWeapon(2));
								}
							}
							if(weapon != WeaponType.TWOHANDAXE)
							{
								if(GUI.Button(new Rect(1115, 410, 100, 30), "2 Hand Axe"))
								{
									StartCoroutine(_SwitchWeapon(3));
								}
							}
							if(weapon != WeaponType.TWOHANDBOW)
							{
								if(GUI.Button(new Rect(1115, 440, 100, 30), "2 Hand Bow"))
								{
									StartCoroutine(_SwitchWeapon(4));
								}
							}
							if(weapon != WeaponType.TWOHANDCROSSBOW)
							{
								if(GUI.Button(new Rect(1115, 470, 100, 30), "Crossbow"))
								{
									StartCoroutine(_SwitchWeapon(5));
								}
							}
							if(weapon != WeaponType.RIFLE)
							{
								if(GUI.Button(new Rect(1000, 470, 100, 30), "Rifle"))
								{
									StartCoroutine(_SwitchWeapon(18));
								}
							}
							if(weapon != WeaponType.STAFF)
							{
								if(GUI.Button(new Rect(1115, 500, 100, 30), "Staff"))
								{
									StartCoroutine(_SwitchWeapon(6));
								}
							}
							if(leftWeapon != 7)
							{
								if(GUI.Button(new Rect(1115, 700, 100, 30), "Shield"))
								{
									StartCoroutine(_SwitchWeapon(7));
								}
							}
							if(leftWeapon != 8)
							{
								if(GUI.Button(new Rect(1065, 540, 100, 30), "Left Sword"))
								{
									StartCoroutine(_SwitchWeapon(8));
								}
							}
							if(rightWeapon != 9)
							{
								if(GUI.Button(new Rect(1165, 540, 100, 30), "Right Sword"))
								{
									StartCoroutine(_SwitchWeapon(9));
								}
							}
							if(leftWeapon != 10)
							{
								if(GUI.Button(new Rect(1065, 570, 100, 30), "Left Mace"))
								{
									StartCoroutine(_SwitchWeapon(10));
								}
							}
							if(rightWeapon != 11)
							{
								if(GUI.Button(new Rect(1165, 570, 100, 30), "Right Mace"))
								{
									StartCoroutine(_SwitchWeapon(11));
								}
							}
							if(leftWeapon != 12)
							{
								if(GUI.Button(new Rect(1065, 600, 100, 30), "Left Dagger"))
								{
									StartCoroutine(_SwitchWeapon(12));
								}
							}
							if(leftWeapon != 13)
							{
								if(GUI.Button(new Rect(1165, 600, 100, 30), "Right Dagger"))
								{
									StartCoroutine(_SwitchWeapon(13));
								}
							}
							if(leftWeapon != 14)
							{
								if(GUI.Button(new Rect(1065, 630, 100, 30), "Left Item"))
								{
									StartCoroutine(_SwitchWeapon(14));
								}
							}
							if(leftWeapon != 15)
							{
								if(GUI.Button(new Rect(1165, 630, 100, 30), "Right Item"))
								{
									StartCoroutine(_SwitchWeapon(15));
								}
							}
							if(leftWeapon != 16)
							{
								if(GUI.Button(new Rect(1065, 660, 100, 30), "Left Pistol"))
								{
									StartCoroutine(_SwitchWeapon(16));
								}
							}
							if(leftWeapon != 17)
							{
								if(GUI.Button(new Rect(1165, 660, 100, 30), "Right Pistol"))
								{
									StartCoroutine(_SwitchWeapon(17));
								}
							}
						}
					}
				}
				if(canJump || canDoubleJump)
				{
					if(isGrounded)
					{
						if(GUI.Button(new Rect(25, 165, 100, 30), "Jump"))
						{
							if(canJump && isGrounded)
							{
								StartCoroutine(_Jump());
							}
						}
					} 
					else
					{
						if(GUI.Button(new Rect(25, 165, 100, 30), "Double Jump"))
						{
							if(canDoubleJump && !isDoubleJumping)
							{
								StartCoroutine(_Jump());
							}
						}
					}
				}
				if(!blockGui && !isBlocking && isGrounded)
				{
					if(GUI.Button(new Rect(30, 270, 100, 30), "Death"))
					{
						StartCoroutine(_Death());
					}
				}
			}
		}
		if(isDead)
		{
			if(GUI.Button(new Rect(30, 270, 100, 30), "Revive"))
			{
				StartCoroutine(_Revive());
			}
		}
	}

	#endregion*/
}
