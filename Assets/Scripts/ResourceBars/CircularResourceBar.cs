using JetBrains.Annotations;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEngine.UI;

public class CircularResourceBar : AListenerEnabler, IRestartable
{
    [SerializeField] private Image circleImage;
    [SerializeField] private Animator speedCircleAnimator;
    [SerializeField] private GameEvent transitionEvent;

    [Header("Base Values")]
    [SerializeField] [Range(0f, 1f)] private float startingValue;
    [SerializeField] [Range(0f, 1f)] private float maximumValue;
    [SerializeField] private float currentValue;

    [Header("Speed Stages")] 
    [SerializeField] private float[] maxSpeeds;
    private int featherCount, currentStage;

    public float CurrentValue => currentValue;

    public float FillAmount => circleImage.fillAmount;

    private void Awake()
    {
        if(circleImage == null)
            circleImage = GetComponent<Image>();

        currentValue = circleImage.fillAmount;
    }

    private void Start() => RegisterWithHandler();

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

    [UsedImplicitly]
    public void OnFeatherCollected()
    {
        if (++featherCount % MainCharacterAnimationStageController.STAGE_THRESHOLD == 0)
        {
            transitionEvent.Raise();

            if (++currentStage >= maxSpeeds.Length) return;

            maximumValue = maxSpeeds[currentStage];
        }
        
        speedCircleAnimator.SetInteger("FeatherCount", featherCount);
    }

    public void Restart()
    {
        featherCount = 0;
        currentStage = 0;
        maximumValue = maxSpeeds[0];
        circleImage.fillAmount = startingValue;
        speedCircleAnimator.SetInteger("FeatherCount", 0);
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
