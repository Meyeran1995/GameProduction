using System.Collections;
using UnityEngine;
using FMODUnity;

/// <summary>
/// An evil bird that charges at the target
/// </summary>
public class BirdObstacle : AMovingObstacle
{
    [Header("Bird Values")]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private SpriteRenderer birdRenderer;
    [SerializeField] private Vector2 startingMovement = Vector2.left;
    private Vector2 lastHorizontalMovement;
    private bool isCharging;
    private bool hasHitPlayer;
    private Transform playerTransform;
    private float previousPLayerHeight;

    [Header("Additional Sounds")]
    [EventRef] [SerializeField] [Tooltip("Sound to be played on destruction")] protected string onDestroyedSound;

    [Header("Effect")]
    [SerializeField] private ParticleSystem deathEffect;

    public Transform Target { get; set; }

    protected override void Awake()
    {
        movementDir = startingMovement * obstacleSpeed;
        lastHorizontalMovement = movementDir;
    }

    public override void Restart()
    {
        StopAllCoroutines();
        transform.localPosition = originalPosition;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;

        movementDir = startingMovement * obstacleSpeed;
        lastHorizontalMovement = movementDir;

        previousPLayerHeight = 0;
        playerTransform = null;

        birdRenderer.flipX = false;
        hasHitPlayer = false;
        isCharging = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerTransform == null)
                playerTransform = collision.transform;
            previousPLayerHeight = playerTransform.position.y;
            isCharging = false;
            hasHitPlayer = true;
            movementDir = GetBounceVector(lastHorizontalMovement.x > 0f ? -1 : 1) * chargeSpeed;
        }
        else
        {
            StartCoroutine(WaitAndDisable(collision));
        }
    }

    protected override void Move()
    {
        if (isCharging)
        {
            movementDir = (Target.position - transform.position).normalized * chargeSpeed;
        }
        else if(!birdRenderer.isVisible && playerTransform)
        {
            AdjustHeightBasedOnPlayer();
        }

        rigidBody.MovePosition((Vector2)transform.position + movementDir * Time.fixedDeltaTime);
    }

    private Vector3 GetBounceVector(int sign) => Quaternion.AngleAxis(45f * sign, transform.forward) * playerTransform.up;

    private void AdjustHeightBasedOnPlayer()
    {
        movementDir.y = previousPLayerHeight - playerTransform.position.y;
        previousPLayerHeight = playerTransform.position.y;
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

    protected override void OnBecameInvisible()
    {
        // don't disable the bird when exiting screen
    }

    private void OnBecameVisible()
    {
        if (hasHitPlayer)
            OnTriggeredPlay();
    }

    protected void OnTriggerEnter2D() => isCharging = true;

    protected void OnTriggerExit2D()
    {
        if(gameObject.activeSelf)
            StartCoroutine(OnScreenLeft());
    }

    private IEnumerator OnScreenLeft()
    {
        yield return new WaitForSeconds(0.125f);

        SnapToScreenBounds();

        lastHorizontalMovement = lastHorizontalMovement.x > 0f
            ? new Vector2(-obstacleSpeed , 0f)
            : new Vector2(obstacleSpeed * 2f, 0f);

        movementDir = lastHorizontalMovement;
        birdRenderer.flipX = movementDir.x > 0f;
    }

    private void SnapToScreenBounds()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            lastHorizontalMovement.x > 0f ? Vector2.right : Vector2.left, Mathf.Infinity, 1 << 11);
        transform.position = new Vector2(hit.point.x, playerTransform.position.y + 3f);
    }

    private void OnDrawGizmos()
    {
        Vector3 lineEnd = startingMovement;
        lineEnd += transform.position;
        Gizmos.DrawLine(transform.position, lineEnd);
    }
}
