using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        //     _animator.SetFloat("Speed", gameObject..velocity.magnitude);
    }
}