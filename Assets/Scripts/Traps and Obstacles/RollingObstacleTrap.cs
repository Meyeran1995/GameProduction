using UnityEngine;

public class RollingObstacleTrap : ObstacleTrap
{
    [SerializeField] [Tooltip("How fast is the obstacle going to move?")] protected float obstacleSpeed;

    protected override void TriggerTrap(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        obstacleBody.gameObject.SetActive(true);
        obstacleBody.bodyType = RigidbodyType2D.Dynamic;
        obstacleBody.transform.GetComponent<RollingObstacle>().SetMovementDir((collision.transform.position - obstacleBody.transform.position).normalized * obstacleSpeed);
    }
}
