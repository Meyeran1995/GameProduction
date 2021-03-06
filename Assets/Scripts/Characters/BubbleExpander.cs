using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BubbleExpander : AListenerEnabler, IRestartable
{
    [SerializeField] private CircleCollider2D bubbleCollider;
    [SerializeField] private SpriteRenderer bubbleEdgeRenderer;
    [SerializeField] private AutoFillResourceBar energyBar;
    [SerializeField] private Animator bubbleAnimator;
    private Rigidbody2D bubbleRigidbody;
    private BubbleResourceController bubbleResourceController;

    [Header("Bubble Properties")]
    [SerializeField] [Tooltip("Minimum radius for the bubble")] [Range(0.1f, 2f)] private float minRadius;
    [SerializeField] [Tooltip("Maximum radius for the bubble")] [Range(0.1f, 2f)] private float maxRadius;
    [SerializeField] [Tooltip("At which radius does the collider become active?")] [Range(0.1f, 2f)] private float activationRadius;

    [SerializeField] 
    [Tooltip("How much is the minimum bubble opacity?")]
    [Range(0f, 1f)]
    private float edgeOpacityOffset;
    [SerializeField]
    [Tooltip("How much percent of opacity resulting rom remaining energy is lost?")]
    [Range(0f, 1f)]
    private float edgeOpacityLossStrength;

    [SerializeField] 
    [Tooltip("How fast is the bubble growing?")] 
    [Range(0.1f, 2f)] 
    private float expansionRate;

    [SerializeField] 
    [Tooltip("Amount of force applied to objects that are pushed by the bubble")] 
    [Range(0.1f, 1f)] 
    private float bubblePushStrength;

    private bool isExpanding;
    private Color bubbleActiveColor;

    [Header("Sounds")]
    [EventRef] [SerializeField] [Tooltip("Sound to be played while using the bubble")] private string bubbleSound;
    private EventInstance bubbleSoundInstance;

    #region Restart

    public void Restart()
    {
        StopExpanding();
        bubbleCollider.radius = minRadius;
        transform.GetChild(0).localScale = new Vector3(minRadius, minRadius) * 2f;

        bubbleCollider.enabled = false;
        bubbleEdgeRenderer.enabled = false;
        bubbleActiveColor = new Color(1f, 0.5f, 0f);
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);

    #endregion

    private void Awake()
    {
        bubbleCollider = GetComponent<CircleCollider2D>();
        bubbleCollider.enabled = false;
        bubbleEdgeRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bubbleEdgeRenderer.enabled = false;

        bubbleRigidbody = GetComponent<Rigidbody2D>();

        bubbleActiveColor = new Color(1f, 0.5f, 0f);

        bubbleResourceController = GetComponent<BubbleResourceController>();
    }

    private IEnumerator Start()
    {
        RegisterWithHandler();

        yield return new WaitUntil(() => RuntimeManager.HasBankLoaded("Master"));

        bubbleSoundInstance = RuntimeManager.CreateInstance(bubbleSound);
        bubbleSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, bubbleRigidbody));
    }

    private void FixedUpdate()
    {
        if (isExpanding)
        {
            EvaluateEnergyAmount();

            bubbleSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, bubbleRigidbody));

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

    /// <summary>
    /// Activate collision and rendering based on thresholds
    /// </summary>
    private void EvaluateBubbleState()
    {
        if (bubbleCollider.radius >= activationRadius)
        {
            bubbleCollider.enabled = true;
        }
        else
        {
            bubbleCollider.enabled = false;
            bubbleEdgeRenderer.color = Color.grey;
        }
        
        bubbleEdgeRenderer.enabled = bubbleCollider.radius > minRadius;
    }

    /// <summary>
    /// Changes bubble opacity based on remaining energy
    /// </summary>
    private void EvaluateEnergyAmount()
    {
        if(!bubbleCollider.enabled) return;

        bubbleActiveColor.a = Mathf.Clamp01(energyBar.FillAmount * edgeOpacityLossStrength + edgeOpacityOffset);
        bubbleEdgeRenderer.color = bubbleActiveColor;
    }

    #region Bubble Expansion

    public void StartExpanding()
    {
        if (energyBar.IsDepleted) return;

        bubbleAnimator.SetBool("isExpanding",true);
        bubbleSoundInstance.start();
        isExpanding = true;
        bubbleResourceController.StartResourceDepletion();
    }

    public void StopExpanding()
    {
        bubbleAnimator.SetBool("isExpanding", false);
        bubbleSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        isExpanding = false;
        bubbleResourceController.EndResourceDepletion();
    }

    private void AdjustRadius(int sign)
    {
        bubbleCollider.radius = Mathf.Clamp(bubbleCollider.radius + expansionRate * Time.fixedDeltaTime * sign, minRadius, maxRadius);
        transform.GetChild(0).localScale = new Vector3(bubbleCollider.radius, bubbleCollider.radius) * 2f;
    }

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
