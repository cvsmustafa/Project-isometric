using UnityEngine;
using UnityEngine.AI;

public class enemydistance : MonoBehaviour
{
    public float avoidDistance = 3f; // Düþmanlarýn birbirlerinden kaçýnacaklarý mesafe
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Diðer düþmanlardan kaçýnma
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, avoidDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider.gameObject != this.gameObject && hitCollider.gameObject.CompareTag("Enemy"))
            {
                Vector3 fleeDirection = (transform.position - hitCollider.transform.position).normalized;
                Vector3 newTarget = transform.position + fleeDirection * avoidDistance;

                agent.SetDestination(newTarget);
            }
        }

    }
}