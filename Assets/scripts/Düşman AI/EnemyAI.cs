using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyDistances enemyDistances;
    private float waitTime = 0.3f; // Bekleme süresi
    private float timer; // Zamanlayýcý
    public GameObject player;
    protected NavMeshAgent agent;
    public LayerMask obstacleMask;

    private float checkInterval = 0.5f; // Kontrol aralýðý (saniye cinsinden)
    private float checkTimer = 0; // Kontrol zamanlayýcýsý

    bool playerDetected = false; // Kontrol deðiþkeni
    bool hasDetectedPlayerOnce = false; // Kontrol deðiþkeni

    protected enum State //durumlar
    {
        Idle,
        Follow,
        Wait,
        Flee

    }
    protected State state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = enemyDistances.speed; // NavMeshAgent'in hýzýný ayarlar
    }

    protected virtual void Update()
    {

        float sqrStopDistance = enemyDistances.stopDistance * enemyDistances.stopDistance;

        #region fonksiyonlar
        Look();

        checkTimer += Time.deltaTime; // Zamanlayýcýyý artýr
        if (checkTimer >= checkInterval && !hasDetectedPlayerOnce) // Zamanlayýcý kontrol aralýðýný aþtýðýnda
        {
            Enemyfind();
            checkTimer = 0; // Zamanlayýcýyý sýfýrla
        }
        #endregion

        switch (state)
        {
            case State.Idle:
                // Oyuncuyu fark etme

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
                // Oyuncuyu takip et, belirli bir mesafeye gelince dur
                agent.SetDestination(player.transform.position);
                if ((transform.position - player.transform.position).sqrMagnitude < sqrStopDistance)
                {
                    agent.SetDestination(transform.position);

                    timer = waitTime;
                    state = State.Wait;
                }
                break;
            case State.Wait:

                timer -= Time.deltaTime; // Zamanlayýcýyý azalt
                if (timer <= 0) // Zamanlayýcý bittiðinde
                {
                    agent.SetDestination(player.transform.position); // Düþmanýn hedefini oyuncunun konumuna ayarla
                    state = State.Flee;
                }
                break;

            case State.Flee:
                // Oyuncudan kaç
                Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
                Vector3 newTarget = transform.position + fleeDirection * enemyDistances.fleeDistance;

                agent.SetDestination(newTarget);
                if ((transform.position - player.transform.position).sqrMagnitude > sqrStopDistance)
                {
                    state = State.Idle;
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

    public void Look() // sürekli düþmana bakýyor
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
            float dotProduct = Vector3.Dot(transform.forward, dir); //vektörlerinin noktasal çarpýmýný hesaplýyor böylece daha optimize çalýþýyor
            bool IsItOpen = Physics.Linecast(transform.position + Vector3.up, player.transform.position + Vector3.up, obstacleMask);

            if (dotProduct > Mathf.Cos(60 * Mathf.Deg2Rad) && !IsItOpen)
            {
                playerDetected = true;

                hasDetectedPlayerOnce = true; // Oyuncu algýlandýðýnda bu metodu sonlandýrýyor
            }

        }
    }
}