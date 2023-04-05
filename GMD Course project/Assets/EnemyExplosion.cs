using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
}