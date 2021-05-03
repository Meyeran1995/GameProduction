using UnityEngine;

public class RollingObstacleTrap : ObstacleTrap
{
    protected override void TriggerTrap(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        obstacleBody.gameObject.SetActive(true);
        obstacleBody.bodyType = RigidbodyType2D.Dynamic;
        obstacleBody.transform.GetComponent<AMovingObstacle>().MovementDir = (collision.transform.position - obstacleBody.transform.position).normalized;
    }
}
