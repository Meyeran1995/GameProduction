using JetBrains.Annotations;
using UnityEngine;

public class BubbleExpander : AMultiListenerEnabler
{
    [SerializeField] private CircleCollider2D bubbleCollider;
    [SerializeField] private SpriteRenderer bubbleEdgeRenderer;
    [SerializeField] private ResourceBar energyBar;

    [Header("Bubble Properties")]
    [SerializeField] [Tooltip("Minimum radius for the bubble")] [Range(0.1f, 2f)] private float minRadius;
    [SerializeField] [Tooltip("Maximum radius for the bubble")] [Range(0.1f, 2f)] private float maxRadius;
    [SerializeField] [Tooltip("At which radius does the collider become active?")] [Range(0.1f, 2f)] private float activationRadius;
    [SerializeField] [Tooltip("How fast is the bubble growing?")] [Range(0.1f, 2f)] private float expansionRate;
    [SerializeField] [Tooltip("Amount of force applied to objects that are pushed by the bubble")] [Range(100f, 500f)] private float bubblePushStrength;

    private bool isExpanding;

    public bool HasEnergyLeft => !energyBar.IsDepleted;

    private void Awake()
    {
        bubbleCollider = GetComponent<CircleCollider2D>();
        bubbleCollider.enabled = false;
        bubbleEdgeRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bubbleEdgeRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isExpanding)
        {
            if (energyBar.IsDepleted)
            {
                StopExpanding();
                return;
            }

            if (bubbleCollider.radius >= maxRadius) return;

            AdjustRadius(1);
        }
        else if (bubbleCollider.radius > minRadius)
        {
            AdjustRadius(-1);
        }

        EvaluateBubbleState();
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
        isExpanding = false;
        energyBar.IsDepleting = false;
    }

    private void AdjustRadius(int sign)
    {
        bubbleCollider.radius = Mathf.Clamp(bubbleCollider.radius + expansionRate * Time.fixedDeltaTime * sign, minRadius, maxRadius);
        transform.GetChild(0).localScale = new Vector3(bubbleCollider.radius, bubbleCollider.radius) * 2f;
    }

    /// <summary>
    /// Activate collision and rendering based on thresholds
    /// </summary>
    private void EvaluateBubbleState()
    {

        if (bubbleCollider.radius >= activationRadius)
        {
            bubbleCollider.enabled = true;
            bubbleEdgeRenderer.color = new Color(1f, 0.5f, 0f);
        }
        else
        {
            bubbleCollider.enabled = false;
            bubbleEdgeRenderer.color = Color.grey;
        }

        bubbleEdgeRenderer.enabled = bubbleCollider.radius > minRadius;
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

        if (activationRadius > maxRadius || activationRadius < minRadius)
            Debug.LogError("Activation radius is not within radius bounds");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

    #endregion
}
