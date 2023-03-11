using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 5f;

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

    // Update is called once per frame
    private void FixedUpdate()
    {
        Movement();
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

    private void PlayerAlternateFireOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("RIGHT CLICK");
    }

    private void PlayerFireOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("LEFT CLICK");
    } // ReSharper disable Unity.PerformanceAnalysis
    private void Movement()
    {
        rawInputMovement = playerMovement.ReadValue<Vector2>();
        Debug.Log(rawInputMovement);
        rb.AddForce(new Vector3(rawInputMovement.x, 0f, rawInputMovement.y) * speed);
    }

    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("U jumped baby");
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }
}