using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    private Vector3 rawInputMovement;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.AddForce(rawInputMovement*speed);
    }

    public void OnMove(InputValue movementValue)
    {
        var inputMovement = movementValue.Get<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    private void OnFire(InputValue fireValue)
    {
        Debug.Log("LEFT CLICK");
    }
    
    private void OnAlternateFire(InputValue fireValue)
    {
        Debug.Log("RIGHT CLICK");
    }
    
    private void OnLook(InputValue lookValue)
    {
        Debug.Log("We looked! "+lookValue.Get());
    }
}