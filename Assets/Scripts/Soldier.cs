using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent navMesh;
    private GameObject player;
    private AudioSource audioSource;

    public ParticleSystem fireEffect;
    public AudioClip shootAudio;

    public float atkDistance = 10f; // distancia para atacar
    public float followDistance = 20f; // distancia para seguir
    public float atkProbability;

    public int damage = 10;
    public int health = 100;
    public float fireRate = 0.5f;
    private float fireTimer;
    private bool isDead;

    public Transform shootpoint;
    public float range = 100f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navMesh= GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (navMesh.enabled)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            bool shoot = false;
            bool follow = (dist < followDistance);

            if (follow)
            {
                if (dist < atkDistance)
                {
                    shoot = true;
                    Fire();
                }

                navMesh.SetDestination(player.transform.position);
                transform.LookAt(player.transform);
            }

            if (!follow || shoot)
            {
                navMesh.SetDestination(transform.position);
            }

            anim.SetBool("Shoot", shoot);
            anim.SetBool("Run", follow);
        }

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }

    public void Fire()
    {
        if (fireTimer < fireRate)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(shootpoint.position, shootpoint.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<PlayerHealth>())
            {
                hit.transform.GetComponent<PlayerHealth>().ApplyDamage(damage);
            }
        }

        fireEffect.Play();
        ShootFX();

        fireTimer = 0;
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;

        if (health <= 0 && !isDead)
        {
            navMesh.enabled = false;
            anim.SetBool("Shoot", false);
            anim.SetBool("Run", false);
            anim.SetTrigger("Die");
            isDead = true;
        }
    }

    public void ShootFX()
    {
        audioSource.PlayOneShot(shootAudio);
    }
}
