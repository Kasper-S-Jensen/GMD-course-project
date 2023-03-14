using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float speed = 50f;

    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpCooldown, airMultiplier;

    [Header("Ground checking")] [SerializeField]
    private float groundDrag, playerHeight;

    [SerializeField] private LayerMask groundElement;
    [SerializeField] private Transform orientation;
    private bool grounded;
    private Vector3 moveDirection;


    private InputActions PlayerActions;
    private InputAction PlayerAlternateFire;
    private InputAction PlayerFire;
    private InputAction playerJump;
    private InputAction playerMovement;

    private Vector2 rawInputMovement;
    private Rigidbody rb;
    private bool readyToJump = true;


    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        PlayerActions = new InputActions();

        playerMovement = PlayerActions.Player.Move;
        playerJump = PlayerActions.Player.Jump;
        PlayerFire = PlayerActions.Player.Fire;
        PlayerAlternateFire = PlayerActions.Player.AlternateFire;

        //subscriptions
        playerJump.performed += JumpOnPerformed;
        PlayerFire.performed += PlayerFireOnPerformed;
        PlayerAlternateFire.performed += PlayerAlternateFireOnPerformed;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundElement);

        MovementInput();
        VelocityLimiter();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void PlayerAlternateFireOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("RIGHT CLICK");
    }

    private void PlayerFireOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("LEFT CLICK");
    } // ReSharper disable Unity.PerformanceAnalysis
    private void MovementInput()
    {
        rawInputMovement = playerMovement.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * rawInputMovement.y + orientation.right * rawInputMovement.x;
        if (grounded)
            rb.AddForce(moveDirection.normalized * (speed * 10f), ForceMode.Force);
        else if (!grounded) rb.AddForce(moveDirection.normalized * (speed * 10f * airMultiplier), ForceMode.Force);
    }

    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("U tried to jump baby");
        if (playerJump.inProgress && readyToJump && grounded)
        {
            readyToJump = false;


            var velocity = rb.velocity;
            velocity = new Vector3(velocity.x, 0f, velocity.z);
            rb.velocity = velocity;

            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
            Debug.Log("U jumped baby");
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void VelocityLimiter()
    {
        var velocity = rb.velocity;
        var flatVel = new Vector3(velocity.x, 0f, velocity.z);

        if (flatVel.magnitude > speed)
        {
            var limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }


    private void OnEnable()
    {
        playerMovement.Enable();
        playerJump.Enable();
        PlayerFire.Enable();
        PlayerAlternateFire.Enable();
    }

    private void OnDisable()
    {
        playerMovement.Disable();
        playerJump.Disable();
        PlayerAlternateFire.Disable();
    }
}