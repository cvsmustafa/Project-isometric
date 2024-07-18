using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDistances", menuName = "EnemyAI/EnemyDistances", order = 1)]
public class EnemyDistances : ScriptableObject
{
    public float noticeDistance = 15f; // fark etme mesafesi
    public float stopDistance = 10f;
    public float fleeDistance = 1f;
    public float speed = 3.5f; // Hýz deðeri
}