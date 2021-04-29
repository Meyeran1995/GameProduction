using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [Header("Resource Changes")]
    [SerializeField] [Range(1f, 10f)] private float depletionRate;
    [SerializeField] [Range(1f, 10f)] private float increaseRate;

    public bool IsDepleting { get; set; }
    public bool IsDepleted => resource.value <= minimumValue;
    public bool IsReplenishing { get; set; }

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
        if (resource.value > minimumValue)
        {
            resource.value -= depletionRate * Time.deltaTime;
        }
    }

    public void DepleteResource(float amount)
    {
        if (resource.value > minimumValue)
        {
            resource.value -= amount;
        }
    }

    public void ReplenishResource(float timeModifier)
    {
        if (resource.value < maximumValue)
        {
            resource.value += increaseRate * timeModifier;
        }
    }

    public void IncreaseMaxResource(float amount)
    {
        maximumValue += amount;
        resource.maxValue = maximumValue;
        resource.value = maximumValue;
        Debug.Log("Stamina Increased");
    }

    public void FullyDepleteResource() => resource.value = minimumValue;

    private void OnValidate()
    {
        Slider slider = GetComponent<Slider>();
        slider.maxValue = maximumValue;
        slider.minValue = minimumValue;
        slider.value = startingValue;
    }
}
