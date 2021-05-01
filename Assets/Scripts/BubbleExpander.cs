using JetBrains.Annotations;
using UnityEngine;

public class BubbleExpander : AMultiListenerEnabler
{
    [SerializeField] private CircleCollider2D bubbleCollider;
    [SerializeField] private SpriteRenderer bubbleEdgeRenderer;
    [SerializeField] private ResourceBar energyBar;

    [Header("Bubble Properties")]
    [SerializeField] [Range(0.1f, 2f)] private float minRadius;
    [SerializeField] [Range(0.1f, 2f)] private float maxRadius;
    [SerializeField] [Range(0.1f, 2f)] private float expansionRate;
    [SerializeField] [Range(100f, 500f)] private float bubblePushStrength;

    private bool isExpanding;

    private void Awake()
    {
        bubbleCollider = GetComponent<CircleCollider2D>();
        bubbleCollider.enabled = false;
        bubbleEdgeRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bubbleEdgeRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isExpanding && !energyBar.IsDepleted)
        {
            if (bubbleCollider.radius >= maxRadius) return;

            AdjustRadius(1);
        }
        else if (bubbleCollider.radius > minRadius)
        {
            AdjustRadius(-1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 forceDirection = collision.rigidbody.transform.position - transform.position;
        collision.rigidbody.AddForce(forceDirection.normalized * bubblePushStrength);
    }

    public void StartExpanding()
    {
        if(energyBar.IsDepleted) return;

        bubbleCollider.enabled = true;
        isExpanding = true;
        bubbleEdgeRenderer.enabled = true;
        energyBar.IsDepleting = true;
    }

    public void StopExpanding()
    {
        bubbleCollider.enabled = false;
        isExpanding = false;
        bubbleEdgeRenderer.enabled = false;
        energyBar.IsDepleting = false;
    }

    private void AdjustRadius(int sign)
    {
        bubbleCollider.radius = Mathf.Clamp(bubbleCollider.radius + expansionRate * Time.fixedDeltaTime * sign, minRadius, maxRadius);
        transform.GetChild(0).localScale = new Vector3(bubbleCollider.radius, bubbleCollider.radius) * 2f;
    }

    #region Resource Controls

    [UsedImplicitly] public void StartResourceRegeneration() => energyBar.IsReplenishing = true;

    [UsedImplicitly] public void EndResourceGeneration() => energyBar.IsReplenishing = false;

    public void StartResourceDepletion() => energyBar.IsDepleting = true;

    public void EndResourceDepletion() => energyBar.IsDepleting = false;

    #endregion

    #region Editor quality of life

    private void OnValidate()
    {
        GetComponent<CircleCollider2D>().radius = minRadius;
        transform.GetChild(0).localScale = new Vector3(minRadius * 2f, minRadius * 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

    #endregion
}
