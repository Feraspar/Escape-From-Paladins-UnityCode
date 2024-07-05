using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [HideInInspector]public GameObject player;
    private NavMeshAgent agent;
    public Transform[] WayPoints;
    public int Current_Patch;
    public float defaultSpeed = 4f;
    public float chaseSpeed = 15f;

    public Transform targetObj;

    private float waitTimer = 5f;

    public enum AI_State { Patrol, Stay, Chase, Attack, Check };
    public AI_State AI_Enemy;

    public bool StayEnemy;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (AI_Enemy == AI_State.Patrol)
        {
            PatrolBehaviour();
        }

        if (AI_Enemy == AI_State.Stay)
        {
            StayBehaviour();
        }

        if (AI_Enemy == AI_State.Chase)
        {
            ChaseBehaviour();
        }

        if (AI_Enemy == AI_State.Check)
        {
            InteractChase();
        }

        if (AI_Enemy == AI_State.Attack)
        {
            AttackBehaviour();
        }
    }  

    public void StayBehaviour()
    {
        agent.Stop();
        // Countdown the wait timer
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0 && !StayEnemy)
        {
            Current_Patch++;
            Current_Patch = Current_Patch % WayPoints.Length;
            // Wait time is over, switch back to Patrol state
            AI_Enemy = AI_State.Patrol;
            waitTimer = Random.Range(2, 10);

        }
        gameObject.GetComponent<Animator>().SetBool("isPatroll", false);
        gameObject.GetComponent<Animator>().SetBool("isChase", false);
    }

    public void PatrolBehaviour()
    {
        agent.speed = defaultSpeed;
        agent.Resume();

        agent.SetDestination(WayPoints[Current_Patch].transform.position);

        float Patch_Dist = Vector3.Distance(WayPoints[Current_Patch].transform.position, gameObject.transform.position);

        if (Patch_Dist <= 0.3f)
        {
            // Arrived at the patrol point, switch to Stay state
            //waitTimer = patrolWaitTime;
            AI_Enemy = AI_State.Stay;
            // Move to the next patrol point

        }
        gameObject.GetComponent<Animator>().SetBool("isChase", false);
        gameObject.GetComponent<Animator>().SetBool("isAttack", false);
        gameObject.GetComponent<Animator>().SetBool("isPatroll", true);
    }

    public void ChaseBehaviour()
    {
        agent.Resume();
        agent.speed = chaseSpeed;

        Vector3 directionToPlayer = player.transform.position - transform.position;
        Vector3 newPosition = player.transform.position - directionToPlayer.normalized * 1.1f;

        agent.SetDestination(newPosition);

        float Dist_Player = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (Dist_Player < 3f)
        {
            gameObject.GetComponent<Animator>().transform.LookAt(player.transform.position);
            AI_Enemy = AI_State.Attack;
        }

        gameObject.GetComponent<Animator>().SetBool("isPatroll", false);
        gameObject.GetComponent<Animator>().SetBool("isAttack", false);
        gameObject.GetComponent<Animator>().SetBool("isChase", true);
    }

    public void AttackBehaviour()
    {
        gameObject.GetComponent<Animator>().SetBool("isChase", false);
        gameObject.GetComponent<Animator>().SetBool("isAttack", true);
    }

    public void InteractChase()
    {
        agent.Resume();
        agent.speed = 5;

        //Vector3 directionToPlayer = targetObj.transform.position - transform.position;
        //Vector3 newPosition = player.transform.position - directionToPlayer.normalized * 1.1f;

        agent.SetDestination(targetObj.transform.position);

        float Dist_Player = Vector3.Distance(gameObject.transform.position, player.transform.position);
        float Dist_Target = Vector3.Distance(gameObject.transform.position, targetObj.transform.position);

        if (Dist_Player < 2f)
        {
            gameObject.GetComponent<Animator>().transform.LookAt(player.transform.position);
            AI_Enemy = AI_State.Attack;
        }

        if (Dist_Target < 3f)
        {
            AI_Enemy = AI_State.Stay;
        }

        gameObject.GetComponent<Animator>().SetBool("isPatroll", false);
        gameObject.GetComponent<Animator>().SetBool("isAttack", false);
        gameObject.GetComponent<Animator>().SetBool("isChase", true);
    }
}
