using UnityEngine;
using UnityEngine.UI;

public class CircularResourceBar : MonoBehaviour
{
    [SerializeField] private Image circleImage;

    [Header("Base Values")]
    [SerializeField] [Range(0f, 1f)] private float startingValue;
    [SerializeField] [Range(0f, 1f)] private float maximumValue;
    [SerializeField] private float currentValue;

    public float FillAmount => circleImage.fillAmount;

    private void Awake()
    {
        if(circleImage == null)
            circleImage = GetComponent<Image>();

        currentValue = circleImage.fillAmount;
    }

    public void SetCurrentValue(float newValue)
    {
        currentValue = maximumValue * newValue;
        circleImage.fillAmount = currentValue;
    }

    private void OnValidate()
    {
        if (circleImage == null)
            circleImage = GetComponent<Image>();

        if (maximumValue != 0 && startingValue > maximumValue)
        {
            circleImage.fillAmount = maximumValue;
        }
        else
        {
            circleImage.fillAmount = startingValue;
        }
    }
}
