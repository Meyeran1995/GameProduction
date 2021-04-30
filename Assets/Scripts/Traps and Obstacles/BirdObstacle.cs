using UnityEngine;

public class BirdObstacle : Obstacle
{
    private Vector3 movementDir;

    public void SetMovementDir(Vector3 direction) => movementDir = direction;


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) return;

        Destroy(transform.parent.gameObject);
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(transform.position + movementDir * Time.fixedDeltaTime);
    }
}
