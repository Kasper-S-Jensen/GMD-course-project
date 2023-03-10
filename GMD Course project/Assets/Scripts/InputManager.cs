using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput.OnFootActions _onFootActions;
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    private void Awake()
    {
        _playerInput = new PlayerInput();
        _onFootActions = _playerInput.OnFoot;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnEnable()
    {
        _onFootActions.Enable();
    }


    private void OnDisable()
    {
        _onFootActions.Disable();
    }
}