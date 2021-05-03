using UnityEngine;

public class RollingObstacle : AMovingObstacle
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) return;

        base.OnCollisionEnter2D(collision);
        this.enabled = false;
    }

    protected override void Move()
    {
        rigidBody.AddForce(MovementDir * Time.fixedDeltaTime);
    }
}
