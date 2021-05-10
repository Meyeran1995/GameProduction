using System.Collections;
using UnityEngine;

/// <summary>
/// An evil bird that charges at the target
/// </summary>
public class BirdObstacle : AMovingObstacle
{
    public Transform Target { get; set; }

    protected override void Awake() => rigidBody = GetComponent<Rigidbody2D>();

    protected override void OnCollisionEnter2D(Collision2D collision) => StartCoroutine(WaitAndDisable());

    protected override void Move()
    {
        MovementDir = (Target.position - transform.position).normalized * obstacleSpeed;
        rigidBody.MovePosition((Vector2)transform.position + MovementDir * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Waits til all OnCollisions have been resolved
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAndDisable()
    {
        yield return new WaitForEndOfFrame();

        transform.parent.gameObject.SetActive(false);
    }
}
