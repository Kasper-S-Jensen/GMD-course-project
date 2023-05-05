using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletContainer;

    //bullet 
    public GameObject projectile;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //helped with debug
    public bool allowInvoke = true;

    //events
    public GameEvent OnPlayerAttack;
    private InputManager _input;

    //bools
    private bool shooting, readyToShoot;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
        readyToShoot = true;
    }


    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        shooting = _input.isFiring;
        //Shooting
        if (readyToShoot && shooting)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find the exact hit position using a raycast
        var ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 maxrangePoint;
        maxrangePoint =
            Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(80); //Just a point far away from the player

        //Calculate direction from attackPoint to maxrange
        var direction = maxrangePoint - attackPoint.position;

        //Instantiate bullet/projectile
        var currentBullet =
            Instantiate(projectile, attackPoint.position, Quaternion.identity,
                bulletContainer.transform); //store instantiated bullet in curreentBullet
        //Rotate bullet to shooot direction
        currentBullet.transform.forward = direction.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>()
            .AddForce(direction.normalized * shootForce, ForceMode.Impulse);
        //make sure bullet dont collide with player
        Physics.IgnoreCollision(currentBullet.GetComponent<Collider>(), GetComponent<Collider>());
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        OnPlayerAttack.Raise();


        //Invoke resetShot function (if not already invooked), with your timeBetweenShooting
        if (!allowInvoke)
        {
            return;
        }

        Invoke(nameof(ResetShot), timeBetweenShooting);
        allowInvoke = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
}