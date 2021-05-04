using UnityEngine;

public class RollingObstacleTrap : ObstacleTrap
{
    protected override void TriggerTrap(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        obstacleBody.gameObject.SetActive(true);
        SnapToScreenBounds(true, true);
        obstacleBody.bodyType = RigidbodyType2D.Dynamic;
        obstacleBody.transform.GetComponent<AMovingObstacle>().MovementDir = (collision.transform.position - obstacleBody.transform.position).normalized;
    }

    protected override void SnapToScreenBounds(bool matchX, bool matchY)
    {
        if (!matchX && !matchY) return;

        Vector2 newPos = obstacleBody.transform.position;
        Vector2 direction = (obstacleBody.transform.position - transform.position).normalized;

        RaycastHit2D hitScreenEdge = Physics2D.Raycast(obstacleBody.transform.position, obstacleBody.transform.right, Mathf.Infinity, 1 << 11 | 1 << 8);
        //RaycastHit2D hitTerrain = Physics2D.Raycast(obstacleBody.transform.position, obstacleBody.transform.right, Mathf.Infinity, 1 << 11 | 1 << 8);

        if (matchX)
        {
            newPos.x = hitScreenEdge.point.x;
            //if (hitTerrain.transform.gameObject.layer == 8)
            //    newPos.y = hitTerrain.point.y;
        }

        if (matchY)
        {
            newPos.y = hitScreenEdge.point.y;
        }

        obstacleBody.transform.position = newPos;
    }
}
