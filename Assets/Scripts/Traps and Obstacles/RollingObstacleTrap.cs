using UnityEngine;

public class RollingObstacleTrap : ObstacleTrap
{
    protected override void TriggerTrap(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        obstacleBody.gameObject.SetActive(true);
        SnapToScreenBounds();
        obstacleBody.bodyType = RigidbodyType2D.Dynamic;
        obstacleBody.transform.GetComponent<AMovingObstacle>().MovementDir = (collision.transform.position - obstacleBody.transform.position).normalized;
    }

    protected override void SnapToScreenBounds(bool matchX = true, bool matchY = true)
    {
        if (!matchX && !matchY) return;

        Vector2 newPos = obstacleBody.transform.position;

        RaycastHit2D hitScreenEdge = Physics2D.Raycast(obstacleBody.transform.position, obstacleBody.transform.right, Mathf.Infinity, 1 << 11);

        if (matchX)
        {
            newPos.x = hitScreenEdge.point.x;
        }

        if (matchY)
        {
            newPos.y += 5f;
            hitScreenEdge = Physics2D.Raycast(newPos, Vector2.down, Mathf.Infinity, 1 << 8);
            newPos.y = hitScreenEdge.point.y;
        }

        obstacleBody.transform.position = newPos;
    }
}
