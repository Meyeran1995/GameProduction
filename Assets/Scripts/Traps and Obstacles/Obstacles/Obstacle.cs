using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    [SerializeField] [Range(0.1f, 1f)] protected float obstacleMass;

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.mass = obstacleMass;
    }

    protected void OnBecameInvisible() => Destroy(gameObject);
}
