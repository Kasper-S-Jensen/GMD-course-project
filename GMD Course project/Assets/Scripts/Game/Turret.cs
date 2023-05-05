using StarterAssets.Interfaces;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform Gem;
    public bool enemyInAttackRange;
    public float attackRange;

    public LayerMask whatIsEnemy;

    private Transform _player;

    private IEnemyAttackPlayer attack;
    // private UIController _uiController;

    // Start is called before the first frame update
    private void Awake()
    {
        attack = GetComponent<IEnemyAttackPlayer>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        /*EnemyRange(transform.position);
        if (enemyInAttackRange)
        {
           // Gem.LookAt(_player);
            attack.AttackPlayer();
        }*/
    }

    public void ActivateTurret(Component sender, object data)
    {
        var distance = Vector3.Distance(transform.position, _player.position);
        if (distance < 4 && UIController._score > 1000)
        {
            Gem.gameObject.SetActive(true);
            Debug.Log("TURRET ACTIVATED");
        }
    }

    private void EnemyRange(Vector3 position)
    {
        enemyInAttackRange = Physics.CheckSphere(position, attackRange, whatIsEnemy);
    }

    /*
    var targetPos = new Vector3(_player.position.x, turretMid.position.y, _player.position.z);
    turretMid.LookAt(targetPos);
    turretMid.rotation = Quaternion.Euler(0, turretMid.rotation.eulerAngles.y, 0);*/
} /*
        var lookAtTarget =
            new Vector3(_player.transform.position.x, turretMid.position.y, _player.transform.position.z);
        turretMid.LookAt(lookAtTarget);*/