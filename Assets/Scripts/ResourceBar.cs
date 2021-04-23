using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [Header("Resource Changes")]
    [SerializeField] [Range(1f, 10f)] private float depletionRate;
    [SerializeField] [Range(1f, 10f)] private float increaseRate;
    [SerializeField] private bool autoReplenish;
    public bool IsDepleting { get; set; }

    [Header("Base Value")]
    [SerializeField] private float minimumValue, maximumValue, startingValue;
    [SerializeField] private Slider resource;

    private void Awake()
    {
        resource.maxValue = maximumValue;
        resource.minValue = minimumValue;
        resource.value = startingValue;
        resource = GetComponent<Slider>();
    }

    private void Update()
    {
        if (IsDepleting)
        {
           DepleteResource(Time.deltaTime);
           return;
        }
        
        if(autoReplenish)
        {
            ReplenishResource(Time.deltaTime);
        }
    }

    public void DepleteResource(float timeModifier)
    {
        if (resource.value > minimumValue)
        {
            resource.value -= depletionRate * timeModifier;
        }
    }

    public void ReplenishResource(float timeModifier)
    {
        if (resource.value < maximumValue)
        {
            resource.value += increaseRate * timeModifier;
        }
    }

    private void OnValidate()
    {
        Slider slider = GetComponent<Slider>();
        slider.maxValue = maximumValue;
        slider.minValue = minimumValue;
        slider.value = startingValue;
    }
}
