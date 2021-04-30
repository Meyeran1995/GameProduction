using UnityEngine;

public class RollingObstacle : Obstacle
{
    private Vector2 movementDir;

    public void SetMovementDir(Vector3 direction) => movementDir = direction;


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) return;

        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.mass = obstacleMass;

        this.enabled = false;
    }

    private void FixedUpdate()
    {
        rigidBody.AddForce(movementDir * Time.fixedDeltaTime);
    }
}
