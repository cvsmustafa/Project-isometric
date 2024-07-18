using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyDistances enemyDistances;
    private float waitTime = 0.3f; // Bekleme s�resi
    private float timer; // Zamanlay�c�
    public GameObject player;
    protected NavMeshAgent agent;
    public LayerMask obstacleMask;

    private float checkInterval = 0.5f; // Kontrol aral��� (saniye cinsinden)
    private float checkTimer = 0; // Kontrol zamanlay�c�s�

    bool playerDetected = false; // Kontrol de�i�keni
    bool hasDetectedPlayerOnce = false; // Kontrol de�i�keni

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

        agent.speed = enemyDistances.speed; // NavMeshAgent'in h�z�n� ayarlar
    }

    protected virtual void Update()
    {

        float sqrStopDistance = enemyDistances.stopDistance * enemyDistances.stopDistance;

        #region fonksiyonlar
        Look();

        checkTimer += Time.deltaTime; // Zamanlay�c�y� art�r
        if (checkTimer >= checkInterval && !hasDetectedPlayerOnce) // Zamanlay�c� kontrol aral���n� a�t���nda
        {
            Enemyfind();
            checkTimer = 0; // Zamanlay�c�y� s�f�rla
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

                timer -= Time.deltaTime; // Zamanlay�c�y� azalt
                if (timer <= 0) // Zamanlay�c� bitti�inde
                {
                    agent.SetDestination(player.transform.position); // D��man�n hedefini oyuncunun konumuna ayarla
                    state = State.Flee;
                }
                break;

            case State.Flee:
                // Oyuncudan ka�
                Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
                Vector3 newTarget = transform.position + fleeDirection * enemyDistances.fleeDistance;

                agent.SetDestination(newTarget);
                if ((transform.position - player.transform.position).sqrMagnitude > sqrStopDistance)
                {
                    state = State.Idle;
                }
                break;

        }
        // D��man�n Animator bile�enini al
        Animator animator = GetComponent<Animator>();

        // H�z vekt�r�n� d��man�n yerel koordinatlar�na d�n��t�r
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

        // Yerel h�z vekt�r�n�n x ve z bile�enlerini kullanarak MoveX ve MoveY de�erlerini ayarla
        animator.SetFloat("MoveX", localVelocity.x);
        animator.SetFloat("MoveY", localVelocity.z);
    }

    public void Look() // s�rekli d��mana bak�yor
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
            float dotProduct = Vector3.Dot(transform.forward, dir); //vekt�rlerinin noktasal �arp�m�n� hesapl�yor b�ylece daha optimize �al���yor
            bool IsItOpen = Physics.Linecast(transform.position + Vector3.up, player.transform.position + Vector3.up, obstacleMask);

            if (dotProduct > Mathf.Cos(60 * Mathf.Deg2Rad) && !IsItOpen)
            {
                playerDetected = true;

                hasDetectedPlayerOnce = true; // Oyuncu alg�land���nda bu metodu sonland�r�yor
            }

        }
    }
}