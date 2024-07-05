using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float radius;
    [HideInInspector] public float originalRadius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask ObstacleMask;
    public bool canSeePlayer;
    private bool hasPlayedDetectedSound;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        originalRadius = radius;
        hasPlayedDetectedSound = true;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FixedUpdate()
    {
        float Dist_Player = Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<EnemyScript>().player.transform.position);

        if (canSeePlayer == true || Dist_Player < 2)
        {
            PlayDetectedSound();
            gameObject.GetComponent<EnemyScript>().AI_Enemy = EnemyScript.AI_State.Chase;

        }           
        else if (gameObject.GetComponent<EnemyScript>().StayEnemy == false)
        {
            gameObject.GetComponent<EnemyScript>().AI_Enemy = EnemyScript.AI_State.Patrol;
            hasPlayedDetectedSound = true;
        }
    }



    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstacleMask))
                {
                    canSeePlayer = true;
                }

                else
                {
                    canSeePlayer = false;
                }
                    
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    public void PlayDetectedSound()
    {
        if (hasPlayedDetectedSound)
        {
            FindObjectOfType<AudioManager>().Play("Detected");
            hasPlayedDetectedSound = false;
        }
    }
}
