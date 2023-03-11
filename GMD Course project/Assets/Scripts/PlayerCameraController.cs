using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    public Transform orientation;

    [SerializeField] private float sensX = 80, sensY = 80;

    private InputAction cameraActions;

    private InputActions playerActions;

    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    private void Awake()
    {
        playerActions = new InputActions();

        cameraActions = playerActions.Player.Look;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        CameraMovement();
    }

    private void OnEnable()
    {
        cameraActions.Enable();
    }

    private void OnDisable()
    {
        cameraActions.Disable();
    }

    private void CameraMovement()
    {
        var mouseX = cameraActions.ReadValue<Vector2>().x * Time.deltaTime * sensX;
        var mouseY = cameraActions.ReadValue<Vector2>().y * Time.deltaTime * sensY;


        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}