using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Mouse Cursor Settings")] private readonly bool cursorLocked = true;

    // Input actions
    private InputActions playerControls;

    // Movement
    public Vector2 moveInput { get; set; }

    // Loking
    public Vector2 lookInput { get; set; }

    // Firing
    public bool isFiring { get; set; }

    // Reload
    public bool isReloading { get; set; }

    // Jumping
    public bool isJumping { get; set; }

    // Sprinting
    public bool isSprinting { get; set; }

    private void Awake()
    {
        playerControls = new InputActions();

        // Movement
        playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Looking
        playerControls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // Firing
        playerControls.Player.Fire.performed += ctx => isFiring = true;
        playerControls.Player.Fire.canceled += ctx => isFiring = false;

        // Relaoding
        playerControls.Player.Reload.performed += ctx => isReloading = true;
        playerControls.Player.Reload.canceled += ctx => isReloading = false;

        // Jumping
        playerControls.Player.Jump.performed += ctx => isJumping = true;
        playerControls.Player.Jump.canceled += ctx => isJumping = false;

        // Sprinting
        playerControls.Player.Sprint.performed += SprintOnperformed;
        playerControls.Player.Sprint.canceled += SprintOncanceled;
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SprintOncanceled(InputAction.CallbackContext obj)
    {
        isSprinting = false;
        Debug.Log("SPRINT CANCELLED");
    }

    private void SprintOnperformed(InputAction.CallbackContext obj)
    {
        isSprinting = true;
        Debug.Log("SPRINT STARTED");
    }


    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}