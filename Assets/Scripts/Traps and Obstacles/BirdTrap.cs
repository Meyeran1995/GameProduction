using UnityEngine;

public class BirdTrap : RollingObstacleTrap
{
    protected override void TriggerTrap(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        obstacleBody.gameObject.SetActive(true);
        obstacleBody.transform.GetComponent<BirdObstacle>().SetMovementDir((collision.transform.position - obstacleBody.transform.position).normalized * obstacleSpeed);
    }
}
