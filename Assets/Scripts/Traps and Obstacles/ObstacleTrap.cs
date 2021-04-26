using UnityEngine;

public class ObstacleTrap : ATrap
{
    [SerializeField] private Rigidbody2D obstacleBody;

    protected override void TriggerTrap(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            obstacleBody.bodyType = RigidbodyType2D.Dynamic;
    }
}
