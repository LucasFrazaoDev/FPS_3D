using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Properties")]
    public float range = 100f; // Alcance maximo
    public int totalBullets = 30; // Balas por pente
    public int currentBullets; // Numero de balas no pente atual
    public int bulletsLeft = 150;
    public float fireRate = 0.1f;
    private float fireTimer;
    private bool isReloading;
    public int damage;

    [Header("Components")]
    public Transform shootPoint;
    public ParticleSystem fireEffect;
    public GameObject hitEffect;
    public GameObject bulletImpact;
    private Animator anim;
    public AudioClip shootSound;
    private AudioSource audioSource;

    [Header("Aim")]
    public Vector3 aimPos;
    public float aimSpeed;
    private Vector3 originalPos;

    public enum ShootMode
    {
        Auto,
        Semi
    }

    public ShootMode shootMode;
    private bool shootInput;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        originalPos = transform.localPosition;
        currentBullets = totalBullets;
    }

    // Update is called once per frame
    void Update()
    {
        switch (shootMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
                break;
            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
                break;
            default:
                break;
        }

        if (shootInput)
        {
            if (currentBullets > 0)
            {
                Fire();
            }
            else if (bulletsLeft > 0)
            {
                DoReload();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentBullets < totalBullets && bulletsLeft > 0)
            {
                DoReload();
            }
            
        }

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        ToAim();
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");
    }

    private void Fire()
    {
        if (fireTimer < fireRate || isReloading || currentBullets <= 0)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            GameObject hitParticle = Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject bullet = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            bullet.transform.SetParent(hit.transform);

            Destroy(hitParticle, 1f);
            Destroy(bullet, 3f);

            if (hit.transform.GetComponent<ObjectHealth>())
            {
                hit.transform.GetComponent<ObjectHealth>().ApplyDamage(damage);
            }
        }

        anim.CrossFadeInFixedTime("Fire", 0.01f);
        fireEffect.Play();
        PlayShootSound();
        currentBullets--;
        fireTimer = 0f;
    }

    public void ToAim()
    {
        if (Input.GetButton("Fire2") && !isReloading)
        {
            transform.localPosition= Vector3.Lerp(transform.localPosition, aimPos, Time.deltaTime * aimSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,originalPos, Time.deltaTime * aimSpeed);
        }
    }

    private void DoReload()
    {
        if (isReloading)
        {
            return;
        }

        anim.CrossFadeInFixedTime("Reload", 0.01f);
    }

    public void Reload()
    {
        if (bulletsLeft <= 0)
        {
            return;
        }

        int bulletsToLoad = totalBullets - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
    }

    private void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound);
    }
}
