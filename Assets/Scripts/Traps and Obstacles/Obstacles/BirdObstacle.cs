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

    [Header("Effect")]
    [SerializeField] private ParticleSystem deathEffect;

    public Transform Target { get; set; }

    protected override void Awake()
    {
        // empty method override
    }

    public override void Restart()
    {
        transform.localPosition = originalPosition;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
    }

    protected override void OnCollisionEnter2D(Collision2D collision) => StartCoroutine(WaitAndDisable(collision));

    protected override void Move()
    {
        MovementDir = (Target.position - transform.position).normalized * obstacleSpeed;
        rigidBody.MovePosition((Vector2)transform.position + MovementDir * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Waits til all OnCollisions have been resolved
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAndDisable(Collision2D collision)
    {
        deathEffect.transform.position = collision.GetContact(0).point;
        deathEffect.Play();

        yield return new WaitForEndOfFrame();

        gameObject.SetActive(false);
        OnDestroyedPlay();
    }

    private void OnDestroyedPlay()
    {
        if (string.IsNullOrWhiteSpace(onDestroyedSound)) return;

        RuntimeManager.PlayOneShotAttached(onDestroyedSound, gameObject);
    }
}
