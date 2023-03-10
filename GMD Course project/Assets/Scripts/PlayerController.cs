using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private float _speed = 5f;
    private Vector3 rawInputMovement;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.AddForce(rawInputMovement);
    }

    public void OnMovement(InputValue movementValue)
    {
        var inputMovement = movementValue.Get<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }
}