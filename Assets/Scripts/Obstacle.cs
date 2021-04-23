using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] [Range(0.1f, 1f)] private float obstacleMass;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.mass = obstacleMass;
    }
}
