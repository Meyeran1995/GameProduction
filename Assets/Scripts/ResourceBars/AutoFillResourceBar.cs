using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEngine.UI;

public class AutoFillResourceBar : MonoBehaviour, IRestartable
{
    [SerializeField] protected Slider resource;

    [Header("Base Values")]
    [SerializeField] private float startingValue;
    [SerializeField] private float minimumValue;
    [SerializeField] private float maximumValue;

    [Header("Resource Changes")]
    [SerializeField] [Range(1f, 10f)] private float depletionRate;
    [SerializeField] [Range(1f, 15f)] private float increaseRate;

    [Header("Low Resource Events")]
    [SerializeField] private GameEvent lowEvent;
    [SerializeField] private GameEvent lowAvertedEvent;
    private float lowResourceThreshold;
    private bool isBelowThreshold;

    public bool IsDepleted => resource.value <= minimumValue;
    public bool IsDepleting { get; set; }
    public bool IsReplenishing { get; set; }

    public float FillAmount => Mathf.InverseLerp(minimumValue, maximumValue, resource.value);

    private void Awake()
    {
        resource = GetComponent<Slider>();
        resource.value = startingValue;
        lowResourceThreshold = 0.25f * maximumValue;
    }

    private void Start() => RegisterWithHandler();

    private void Update()
    {
        if (IsDepleting)
        {
            DepleteResource();
            return;
        }

        if (IsReplenishing)
        {
            ReplenishResource(Time.deltaTime);
        }
    }

    private void DepleteResource()
    {
        if (resource.value <= minimumValue) return;

        resource.value -= depletionRate * Time.deltaTime;

        if(isBelowThreshold) return;

        isBelowThreshold = resource.value <= lowResourceThreshold;

        if(isBelowThreshold)
            lowEvent.Raise();
    }

    public void ReplenishResource(float timeModifier)
    {
        if (resource.value >= maximumValue) return;

        resource.value += increaseRate * timeModifier;

        if (!isBelowThreshold) return;

        isBelowThreshold = resource.value > lowResourceThreshold;

        if (!isBelowThreshold)
            lowAvertedEvent.Raise();
    }

    private void OnValidate()
    {
        resource = GetComponent<Slider>();
        resource.maxValue = maximumValue;
        resource.minValue = minimumValue;
    }

    public void Restart()
    {
        resource.value = startingValue;
        IsReplenishing = false;
        IsDepleting = false;
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this, 1);
}
