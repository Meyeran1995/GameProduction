using UnityEngine;

public abstract class AMovingObstacle : Obstacle
{
    [Header("Base Values Movement")]
    [SerializeField] [Tooltip("How fast is the obstacle going to move?")] protected float obstacleSpeed;

    protected Vector2 movementDir;

    protected abstract void Move();

    protected void FixedUpdate() => Move();
}
