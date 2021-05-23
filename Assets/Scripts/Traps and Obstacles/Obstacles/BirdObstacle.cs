using System.Collections;
using UnityEngine;
using FMODUnity;

/// <summary>
/// An evil bird that charges at the target
/// </summary>
public class BirdObstacle : AMovingObstacle
{
    [Header("Additional Sounds")]
    [EventRef] [SerializeField] [Tooltip("Sound to be played on destruction")] protected string onDestroyedSound;

    public Transform Target { get; set; }

    protected override void Awake()
    {
        // Overwrite to do nothing in Awake (rigidbody is now set in onenable
    }

    public override void Restart()
    {
        transform.localPosition = originalPosition;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
    }

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
        OnDestroyedPlay();
    }

    private void OnDestroyedPlay()
    {
        if (string.IsNullOrWhiteSpace(onDestroyedSound)) return;

        RuntimeManager.PlayOneShotAttached(onDestroyedSound, gameObject);
    }
}
