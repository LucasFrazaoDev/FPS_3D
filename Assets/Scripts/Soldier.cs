using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent navMesh;
    private GameObject player;

    public float atkDistance = 10f; // distancia para atacar
    public float followDistance = 20f; // distancia para seguir
    public float atkProbability;

    public int damage;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navMesh= GetComponent<NavMeshAgent>();
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
                    shoot= true;
                }

                navMesh.SetDestination(player.transform.position);
            }

            if (!follow || shoot)
            {
                navMesh.SetDestination(transform.position);
            }

            anim.SetBool("Shoot", shoot);
            anim.SetBool("Run", follow);
        }
    }
}
