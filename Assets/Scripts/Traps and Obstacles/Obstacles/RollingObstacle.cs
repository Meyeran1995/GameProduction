using UnityEngine;

public class RollingObstacle : AMovingObstacle
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) return;

        base.OnCollisionEnter2D(collision);
        this.enabled = false;
    }

    public override void Restart()
    {
        base.Restart();
        this.enabled = true;
    }

    protected override void Move()
    {
        GroundObstacle();
        rigidBody.MovePosition(rigidBody.position + MovementDir * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Casts a ray downward in order to get the current ground tile
    /// </summary>
    private void GroundObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 8);

        if (hit.transform == null)
        {
            this.enabled = false;
        }
        else
        {
            MovementDir = -hit.transform.right * obstacleSpeed;
        }
    }
}
