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
        obstacleBody.bodyType = RigidbodyType2D.Dynamic;
    }
}
