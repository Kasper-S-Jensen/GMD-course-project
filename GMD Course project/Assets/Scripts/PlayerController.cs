using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float jumpHeight = 5f;

    [SerializeField] private Transform orientation;

    public float groundDrag, playerHeight;
    public LayerMask groundElement;
    private bool grounded;
    private Vector3 moveDirection;

    private InputActions PlayerActions;
    private InputAction PlayerAlternateFire;
    private InputAction PlayerFire;
    private InputAction playerJump;
    private InputAction playerMovement;

    private Vector2 rawInputMovement;
    private Rigidbody rb;

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
        SpeedControl();
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        Debug.Log(rb.velocity.magnitude);
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
        rb.AddForce(moveDirection.normalized * (speed * 10f), ForceMode.Force);
    }

    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("U jumped baby");
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    private void SpeedControl()
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