using UnityEngine;

public class ObstacleTrap : ATrap
{
    [SerializeField] protected Rigidbody2D obstacleBody;

    protected virtual void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        obstacleBody.gameObject.SetActive(false);
    }

    protected override void TriggerTrap(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        obstacleBody.gameObject.SetActive(true);
        SnapToScreenBounds(false);
        obstacleBody.bodyType = RigidbodyType2D.Dynamic;
    }

    protected virtual void SnapToScreenBounds(bool matchX = true, bool matchY = true)
    {
        if(!matchX && !matchY) return;

        Vector2 newPos = obstacleBody.transform.position;
        Vector2 direction = (obstacleBody.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, 1 << 11);

        if (matchX)
        {
           newPos.x = hit.point.x + 1f;
        }

        if (matchY)
        {
            newPos.y = hit.point.y;
        }

        obstacleBody.transform.position = newPos;
    }
}
