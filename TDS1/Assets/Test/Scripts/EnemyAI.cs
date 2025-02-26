using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour {
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGrpund, whatIsPlayer;

    //Передвижение
    public Vector3 walkPoint;
    bool WalkPaintSet;
    public float walkPaintRange;

    //Атака
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //Состояния   
    public float sightRange, attackRange;
    public bool playerInSIghtRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Проверяем находиться ли игрок в радиусе атаки
        playerInSIghtRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSIghtRange && !playerInAttackRange) ChasePlayer();
        if (playerInSIghtRange && !playerInAttackRange) AttackPlayer();

    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {

    }
}

