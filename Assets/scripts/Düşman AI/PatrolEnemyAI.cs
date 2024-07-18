using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(enemydistance))]
public class PatrolEnemyAI : MonoBehaviour
{
    public EnemyDistances enemyDistances;
    private float waitTime = 0.3f;
    private float timer;
    public GameObject player;
    protected NavMeshAgent agent;
    public LayerMask obstacleMask;

    private float checkInterval = 0.5f;
    private float checkTimer = 0;

    bool playerDetected = false;
    bool hasDetectedPlayerOnce = false;

    public Transform[] waypoints; // Devriye noktalarý
    private int destPoint = 0; // Þu anki hedef nokta

    protected enum State //durumlar
    {
        Patrol, // Devriye durumu eklendi
        Idle,
        Follow,
        Wait,
        Flee
    }
    protected State state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = enemyDistances.speed;
        agent.autoBraking = false; // Düþmanýn hedefe yaklaþýrken yavaþlamasýný engeller

        GoToNextWaypoint(); // Ýlk hedef noktaya git
    }

    void GoToNextWaypoint()
    {
        // Eðer devriye noktasý yoksa çýk
        if (waypoints.Length == 0)
            return;

        // Hedefi, þu anki devriye noktasýna ayarla
        agent.destination = waypoints[destPoint].position;

        // Bir sonraki devriye noktasýna geç (döngüsel)
        destPoint = (destPoint + 1) % waypoints.Length;
    }

    protected virtual void Update()
    {
        float sqrStopDistance = enemyDistances.stopDistance * enemyDistances.stopDistance;
        Look();
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval && !hasDetectedPlayerOnce)
        {
            Enemyfind();
            checkTimer = 0;
        }

        switch (state)
        {
            case State.Patrol:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GoToNextWaypoint();
                if (playerDetected)
                {
                    float sqrDistanceToPlayer = (transform.position - player.transform.position).sqrMagnitude;
                    if (sqrDistanceToPlayer < enemyDistances.noticeDistance * enemyDistances.noticeDistance)
                    {
                        state = State.Follow;
                    }
                }
                break;

            case State.Idle:
                if (playerDetected)
                {
                    float sqrDistanceToPlayer = (transform.position - player.transform.position).sqrMagnitude;
                    if (sqrDistanceToPlayer < enemyDistances.noticeDistance * enemyDistances.noticeDistance)
                    {
                        state = State.Follow;
                    }
                }
                break;

            case State.Follow:
                agent.SetDestination(player.transform.position);
                if ((transform.position - player.transform.position).sqrMagnitude < sqrStopDistance)
                {
                    agent.SetDestination(transform.position);
                    timer = waitTime;
                    state = State.Wait;
                }
                break;
            case State.Wait:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    agent.SetDestination(player.transform.position);
                    state = State.Flee;
                }
                break;

            case State.Flee:
                Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
                Vector3 newTarget = transform.position + fleeDirection * enemyDistances.fleeDistance;

                agent.SetDestination(newTarget);
                if ((transform.position - player.transform.position).sqrMagnitude > sqrStopDistance)
                {
                    state = State.Follow; // Flee durumundan sonra Follow durumuna geç
                }
                break;
        }
        // Düþmanýn Animator bileþenini al
        Animator animator = GetComponent<Animator>();

        // Hýz vektörünü düþmanýn yerel koordinatlarýna dönüþtür
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

        // Yerel hýz vektörünün x ve z bileþenlerini kullanarak MoveX ve MoveY deðerlerini ayarla
        animator.SetFloat("MoveX", localVelocity.x);
        animator.SetFloat("MoveY", localVelocity.z);

    }
    public void Look()
    {
        if (playerDetected)
        {
            transform.LookAt(player.transform.position);
        }
    }

    public void Enemyfind()
    {
        {
            var dir = (player.transform.position - transform.position).normalized;
            Debug.DrawRay(transform.position, transform.forward * 4, Color.black);
            Debug.DrawLine(transform.position, player.transform.position);
            float dotProduct = Vector3.Dot(transform.forward, dir); bool IsItOpen = Physics.Linecast(transform.position + Vector3.up, player.transform.position + Vector3.up, obstacleMask);

            if (dotProduct > Mathf.Cos(60 * Mathf.Deg2Rad) && !IsItOpen)
            {
                playerDetected = true;

                hasDetectedPlayerOnce = true;
            }

        }
    }
}