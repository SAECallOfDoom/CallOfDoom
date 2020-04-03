using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController charController;

    [SerializeField]
    private Camera fpsCam;

    [SerializeField]
    private GameObject playerGun;

    Vector3 impaact = Vector3.zero;
    float mass = 3.0f;

    private float sprintFuel = 100f;

    //Movement fields
    #region MOVEMENT
    [SerializeField]
    private float movementSpeed = 10.0f;

    [SerializeField]
    private float sprintMultiplier;

    [SerializeField]
    private float sprintFuelUsagePerSecond;

    [SerializeField]
    private float sprintFuelChargePerSecond;

    private float sprintMovementSpeed;

    [SerializeField]
    private Transform groundCheck;

    private float groundDistance = 0.4f;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private LayerMask obstacleMask;

    [SerializeField]
    private ParticleSystem sprintParticle;

    private bool isGrounded;

    private bool isSprinting;


    #endregion

    //Physic fields
    #region PHYSICS

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float jumpForce;

    //player velocity
    public Vector3 velocity;

    #endregion

    //Crouching fields
    #region CROUCHING
    [SerializeField]
    private float crouchForce;
    private Vector3 crouchPlayerScale = new Vector3(1.0f,0.5f,1.0f);
    private Vector3 playerScale;
    private Vector3 gunScale;
    private Vector3 gunCrouchScale = new Vector3(1.0f,2.0f,1.0f);


    #endregion

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }
    void Start()
    {
        //get player scale
        playerScale = transform.localScale;

        gunScale = playerGun.transform.localScale;
        sprintMovementSpeed = sprintMultiplier * movementSpeed;
    }
    
    // Update is called once per frame
    void Update()
    {

        MyGravity();
        Movement();
        MyInput();
        if (!isSprinting && sprintFuel <= 100f)
            RechargeSprintFuel();

        LerpImpact();
    }

    private void MyInput()
    {

        if (Input.GetButton("Jump") && IsPlayerGrounded())
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftControl) && IsPlayerGrounded())   
            Crouching();

        if (isSprinting =  Input.GetKey(KeyCode.LeftShift) && IsPlayerGrounded())
            Sprint();
            
        
    }

    private void Movement()
    {
        //Movement Inputs
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        //Movement vector
        Vector3 move = transform.forward * verticalInput + transform.right * horizontalInput;

        //Apply movement
        charController.Move(move * movementSpeed * Time.deltaTime);


        if (!IsPlayerGrounded())         //
            movementSpeed = 5f;          //
                                         // Probably needs a better logic, limits movement when in air
        else                             //
            movementSpeed = 10.0f;       //
    }

    private void MyGravity()
    {
        //if grounded reset the y velocity 
        if (IsPlayerGrounded() && velocity.y < 0)
            velocity.y = -2f; // bit less then 0 to force the player to ground

        velocity.y += gravity * Time.deltaTime;

        //Activate gravity
        charController.Move(velocity * Time.deltaTime);
    }
    private void Jump()
    {
       //Physic formula to calculate jump(sqrt(h * -2 * gravity))
        velocity.y = Mathf.Sqrt(jumpForce * -2.0f * gravity);   
    }
    private void Crouching()
    {
        StartCoroutine(AddForce(-crouchForce, 0.5f,true)); 
    }

    private void StopCrouching()
    {
        playerGun.transform.localScale = gunScale;
        transform.localScale = playerScale;
    }

    private bool IsPlayerGrounded()
    {
        //Creates an invisible sphere at the bottom of the player and returns true if colliding with grounnd
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask | obstacleMask);
        return isGrounded;
    }

    private void Sprint()
    {
        if (sprintFuel >= 0)
        {
            sprintParticle.gameObject.SetActive(true);
            movementSpeed = sprintMovementSpeed;
            sprintFuel -= sprintFuelUsagePerSecond * Time.deltaTime;
            isSprinting = true;
        }

    }

    private void RechargeSprintFuel()
    {
        sprintParticle.gameObject.SetActive(false);
        sprintParticle.Stop();
        sprintFuel += sprintFuelChargePerSecond * Time.deltaTime;
    }
    public IEnumerator AddForce(float _forceAmount, float _waitForSeconds)
    {
        Vector3 oldVelocity = velocity;
        velocity = fpsCam.transform.forward * -_forceAmount;
        yield return new WaitForSeconds(_waitForSeconds);
        velocity = oldVelocity; // CAUSING UNWANTED BEHAVIOUR WITH CROUCHING AND SHOOTING SAME TIME
        //velocity = Vector3.zero;
    }

    public IEnumerator AddForce(float _forceAmount, float _waitForSeconds, bool _isCrouching)
    {
        playerGun.transform.localScale = gunCrouchScale;
        //Player duck
        transform.localScale = crouchPlayerScale;
        Vector3 oldVelocity = velocity;
        velocity = fpsCam.transform.forward * -_forceAmount;
        yield return new WaitForSeconds(_waitForSeconds);
        velocity = Vector3.zero;
        //velocity = oldVelocity;  CAUSING UNWANTED BEHAVIOUR WITH CROUCHING AND SHOOTING SAME TIME
            StopCrouching();
    }

    public void AddImpact(Vector3 _dir,float force)
    {
        _dir.Normalize();
        if (_dir.y < 0)
            _dir.y = -_dir.y;

        impaact += _dir.normalized * force / mass;      
    }

    private void LerpImpact()
    {
        if (impaact.magnitude > 0.2f)
            charController.Move(impaact * Time.deltaTime);

        impaact = Vector3.Lerp(impaact, Vector3.zero, 5 * Time.deltaTime);
    }


}
