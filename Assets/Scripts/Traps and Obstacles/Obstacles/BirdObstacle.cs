using UnityEngine;

/// <summary>
/// An evil bird that charges at the target
/// </summary>
public class BirdObstacle : AMovingObstacle
{
    public Transform Target { get; set; }

    protected override void OnCollisionEnter2D(Collision2D collision) => Destroy(transform.parent.gameObject);

    protected override void Move()
    {
        MovementDir = (Target.position - transform.position).normalized * obstacleSpeed;
        rigidBody.MovePosition((Vector2)transform.position + MovementDir * Time.fixedDeltaTime);
    }
}
