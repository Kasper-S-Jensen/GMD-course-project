using TMPro;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    public GameObject container;

    //bullet 
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    public TextMeshProUGUI ammunitionDisplay;

    //bug fixing :D
    public bool allowInvoke = true;
    private InputManager _input;

    private int bulletsLeft, bulletsShot;


    //bools
    private bool shooting, readyToShoot, reloading;

    private void Awake()
    {
        _input = GetComponent<InputManager>();

        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }


    private void Update()
    {
        MyInput();

        ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    } // ReSharper disable Unity.PerformanceAnalysis
    private void MyInput()
    {
        shooting = _input.isFiring;

        //Reloading 
        if (_input.isReloading && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

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
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        var directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        var x = Random.Range(-spread, spread);
        var y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        var directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        //Instantiate bullet/projectile
        var currentBullet =
            Instantiate(bullet, attackPoint.position, Quaternion.identity,
                container.transform); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>()
            .AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        Physics.IgnoreCollision(currentBullet.GetComponent<Collider>(), GetComponent<Collider>());
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        //  if (muzzleFlash != null)
        //    Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }


    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
    }

    private void ReloadFinished()
    {
        //Fill magazine
        bulletsLeft = magazineSize;
        reloading = false;
    }
}