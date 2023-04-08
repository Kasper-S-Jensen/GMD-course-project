using UnityEngine;

public class GunFire : MonoBehaviour
{
    public GameObject container;

    //bullet 
    public GameObject projectile;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, timeBetweenShots;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //bug fixing :D
    public bool allowInvoke = true;
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
        var ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f,
            0)); //Just a ray through the middle of your current view
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        targetPoint =
            Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        var direction = targetPoint - attackPoint.position;

        //Instantiate bullet/projectile
        var currentBullet =
            Instantiate(projectile, attackPoint.position, Quaternion.identity,
                container.transform); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = direction.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>()
            .AddForce(direction.normalized * shootForce, ForceMode.Impulse);
        Physics.IgnoreCollision(currentBullet.GetComponent<Collider>(), GetComponent<Collider>());
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        //  if (muzzleFlash != null)
        //    Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);


        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (!allowInvoke)
        {
            return;
        }

        Invoke(nameof(ResetShot), timeBetweenShooting);
        allowInvoke = false;
    }

    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }
}