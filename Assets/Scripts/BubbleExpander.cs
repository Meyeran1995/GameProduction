using UnityEngine;

public class BubbleExpander : MonoBehaviour
{
    [SerializeField] private CircleCollider2D bubbleCollider;
    [SerializeField] private SpriteRenderer bubbleEdgeRenderer;
    [SerializeField] private ResourceBar energyBar;

    [Header("Bubble Properties")]
    [SerializeField] [Range(0.1f, 2f)] private float minRadius;
    [SerializeField] [Range(0.1f, 2f)] private float maxRadius;
    [SerializeField] [Range(0.1f, 2f)] private float expansionRate;
    [SerializeField] [Range(100f, 500f)] private float bubblePushStrength;

    public bool IsExpanding
    {
        get => isExpanding;
        set { 
            bubbleCollider.enabled = value;
            isExpanding = value;
            bubbleEdgeRenderer.enabled = value;
            energyBar.IsDepleting = value;
        }
    }

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
        if (IsExpanding)
        {
            if (bubbleCollider.radius >= maxRadius)
            {
                return;
            }

            AdjustRadius(1);
        }
        else if (bubbleCollider.radius > minRadius)
        {
            AdjustRadius(-1);
        }
    }

    private void AdjustRadius(int sign)
    {
        bubbleCollider.radius = Mathf.Clamp(bubbleCollider.radius + expansionRate * Time.fixedDeltaTime * sign, minRadius, maxRadius);
        transform.GetChild(0).localScale = new Vector3(bubbleCollider.radius, bubbleCollider.radius) * 2f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 forceDirection =  collision.rigidbody.transform.position - transform.position;
        collision.rigidbody.AddForce(forceDirection.normalized * bubblePushStrength);
    }

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
}
