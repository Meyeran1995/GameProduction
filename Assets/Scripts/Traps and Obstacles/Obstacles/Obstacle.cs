using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    [SerializeField] [Tooltip("Mass for the object \n\nOnly relevant for falling or rolling obstacles, does nothing for bird obstacles!")] [Range(0.1f, 10f)] protected float obstacleMass;
    [SerializeField] [Tooltip("Available sprites to be randomly selected at runtime \n\nOnly relevant for falling or rolling obstacles, does nothing for bird obstacles!")] private ObstacleSpriteConfig spriteConfig;

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = spriteConfig.GetRandomSprite();
        gameObject.AddComponent<CircleCollider2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.mass = obstacleMass;
    }

    protected void OnBecameInvisible() => Destroy(gameObject);

    protected void OnValidate()
    {
        if (spriteConfig == null) return;

        GetComponent<SpriteRenderer>().sprite = spriteConfig.GetFirstSprite();
    }
}
