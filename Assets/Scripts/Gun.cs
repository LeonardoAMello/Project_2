using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    public Camera cam;
    public float damage = 10f;
    public float meleeDamage = 10f;
    public float range = 100f;
    public float meleeRange = 1.5f;
    public int maxAmmo = 30;
    public int ammo = 30;
    public int ammoQtd = 30;
    public int maxAmmoInInventory = 30;
    public float reloadingTime = 2f;
    public bool automatic;
    public float fireRate = 2f;
    private float timeNextShoot;
    public float imprecision = .1f;
    public float currentImprecision;

    public GameObject impactEffect_Shoot;
    public GameObject impactEffect_Melee;
    public ParticleSystem muzzleFlash;
    public AudioSource shotSound;
    public Text ammoIndicator;

    void Start()
    {
        shotSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        ammoIndicator.text = "Ammo: " + ammo + "/" + ammoQtd;
        if (!Input.GetButton("Running"))
        {

            if (automatic)
            {
                if (Input.GetButton("Fire1") && Time.time > timeNextShoot)
                {
                    if (ammo > 0)
                        Shoot();
                    else if (ammoQtd > 0)
                        Reload();
                }
            }
            else if (Input.GetButtonDown("Fire1") && Time.time > timeNextShoot)
            {
                if (ammo > 0)
                    Shoot();
                else if (ammoQtd > 0)
                    Reload();
            }

            if (Input.GetButtonDown("Reload") && ammoQtd > 0)
                Reload();
        }
    }

    private void Shoot()
    {
        timeNextShoot = Time.time + 1 / fireRate;
        ammo--;
        muzzleFlash.Play();
        shotSound.Play();
        RaycastHit hit;

        Vector3 direction = cam.transform.forward;
        if (!GetComponentInParent<Animator>().GetBool("Scoped"))
            direction += Random.insideUnitSphere * imprecision;

        if (Physics.Raycast(cam.transform.position, direction, out hit, range))
        {
            Destroy(Instantiate(impactEffect_Shoot, hit.point, Quaternion.LookRotation(hit.normal)), 1f);

            EnemyController enemy = hit.transform.GetComponent<EnemyController>();

            if (enemy != null)
            {
                int reward = enemy.TakeDamage(damage);
                if (reward > 0)
                {
                    GetComponentInParent<FirstPersonController>().ReceiveMoney(reward);
                    Destroy(enemy.gameObject);
                }
            }
        }
    }

    private void Reload()
    {
        if (ammo != maxAmmo)
        {
            ammoQtd -= maxAmmo - ammo; // ammo used

            if (ammoQtd >= 0)
            {
                ammo = maxAmmo;
            }
            else
            {
                int ammoMissing;
                ammoMissing = System.Math.Abs(ammoQtd);
                ammoQtd = 0;
                ammo = maxAmmo - ammoMissing;
            }

            GetComponentInParent<WeaponHolder>().ReloadAnimation(reloadingTime);
        }
    }

    public void Melee()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, meleeRange))
        {
            Destroy(Instantiate(impactEffect_Melee, hit.point, Quaternion.LookRotation(hit.normal)), 1f);


            EnemyController enemy = hit.transform.GetComponent<EnemyController>();

            if (enemy != null)
            {
                int reward = enemy.TakeDamage(meleeDamage);
                if (reward > 0)
                {
                    GetComponentInParent<FirstPersonController>().ReceiveMoney(reward);
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
}
