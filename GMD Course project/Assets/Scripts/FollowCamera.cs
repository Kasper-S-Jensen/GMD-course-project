using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;
    // Start is called before the first frame update


    // Update is called once per frame
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}