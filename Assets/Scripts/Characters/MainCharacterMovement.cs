using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class MainCharacterMovement : AListenerEnabler, IRestartable
{
    #region Fields

    [SerializeField] private CircularResourceBar mainResourceBar;

    [Header("Speed")]
    [SerializeField] [Tooltip("Maximum speed to be reached for each stage")] private float[] maxSpeeds;
    [SerializeField] [Tooltip("Minimum speed after being hit")] [Range(0.1f, 10f)] private float minSpeed;
    [SerializeField] [Tooltip("How fast is speed gained? \nOnly use values between 0 and 1!")] private AnimationCurve speedCurve;
    [SerializeField] [Tooltip("How long does it take to gain maximum speed?")] private float rampTime;

    [Header("Speed Debug")] 
    [SerializeField] private float speedTime;
    [SerializeField] private float speedProgress, currentSpeed, stageTransitionHaltTime;
    private int currentMaxSpeed;
    private Coroutine haltSpeedGainRoutine;

    // Movement
    private Vector2 currentDirection;
    private Rigidbody2D characterBody;

    [Header("Movement States")]
    [SerializeField] [Tooltip("Was the end of the journey reached?")] private bool journeyCompleted;
    [SerializeField] [Tooltip("Are we able to move?")] private bool canMove;
    [SerializeField] private MainCharacterAnimationStageController animStageController;

    public bool CanMove => canMove;
    public bool MaxSpeedReached => currentSpeed >= maxSpeeds[currentMaxSpeed];

    [Header("Events")] 
    [SerializeField] [Tooltip("Event when reaching maximum speed")] private GameEvent maxSpeedEvent;
    [SerializeField] [Tooltip("Event when the character gets hit and loses speed")] private GameEvent speedLostEvent;
    [SerializeField] [Tooltip("Event when the journey has been completed")] private GameEvent endOfGameEvent;

    #endregion

    #region Restart

    private Vector3 originalPosition;

    public void Restart()
    {
        transform.position = originalPosition;
        currentMaxSpeed = 0;
        speedTime = 0;
        speedProgress = 0;
        currentSpeed = minSpeed;
        journeyCompleted = false;
        RestartMovingOnUpdate();
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this, 0);

    #endregion

    private void Awake()
    {
        characterBody = GetComponent<Rigidbody2D>();
        currentSpeed = minSpeed;
        originalPosition = transform.position;
    }

    private void Start() => RegisterWithHandler();

    #region Movement

    private void FixedUpdate()
    {
        if (journeyCompleted || !canMove) return;

        RampUpSpeed();
        GroundCharacter();

        if (journeyCompleted) return;
        MoveForward();
    }

    /// <summary>
    /// Casts a ray downward in order to get the current ground tile
    /// </summary>
    private void GroundCharacter()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 5f, 1 << 8);

        if (hit.transform == null)
        {
            journeyCompleted = true;
           StartCoroutine(OnEndOfJourneyReached());
        }
        else
        {
            currentDirection = hit.transform.right * currentSpeed;
        }
    }

    private void MoveForward() => characterBody.MovePosition(characterBody.position + currentDirection * Time.fixedDeltaTime);

    private IEnumerator OnEndOfJourneyReached()
    {
        animStageController.OnEndOfGameReached();

        yield return new WaitForSeconds(3.5f);

        endOfGameEvent.Raise();
    }

    #endregion

    #region Speed

    /// <summary>
    /// Halt speed gain on stage transition
    /// </summary>
    /// <returns></returns>
    private IEnumerator HaltSpeedGain()
    {
        yield return new WaitForSeconds(stageTransitionHaltTime);

        haltSpeedGainRoutine = null;

        if (currentMaxSpeed < maxSpeeds.Length - 1)
            currentMaxSpeed++;
    }

    [UsedImplicitly]
    public void OnStageTransitionHaltSpeedGain() => haltSpeedGainRoutine = StartCoroutine(HaltSpeedGain());

    private void CalcSpeed()
    {
        // Which time is it inside of the speed cycle?
        speedTime = Mathf.Clamp(speedTime + Time.fixedDeltaTime, 0f, rampTime);

        // How much progress did we make, according to time?
        speedProgress = Mathf.InverseLerp(0f, rampTime, speedTime);

        // Calculate current speed based on progress
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeeds[currentMaxSpeed], speedCurve.Evaluate(speedProgress));

        // Update Ui
        mainResourceBar.SetCurrentValue(speedProgress);
    }

    /// <summary>
    /// Increase speed while moving
    /// </summary>
    private void RampUpSpeed()
    {
        if (haltSpeedGainRoutine != null || currentSpeed >= maxSpeeds[currentMaxSpeed]) return;

        CalcSpeed();

        if (currentSpeed < maxSpeeds[currentMaxSpeed]) return;

        currentSpeed = maxSpeeds[currentMaxSpeed];
        maxSpeedEvent.Raise();
    }

    /// <summary>
    /// Halt movement, lose speed
    /// </summary>
    public void StaggerCharacterMovement()
    {
        canMove = false;
        currentSpeed = minSpeed;
        speedTime = 0f;
        // Update Ui
        mainResourceBar.SetCurrentValue(0f);
        speedLostEvent.Raise();
    }

    /// <summary>
    /// Start gaining speed again
    /// </summary>
    public void RestartSpeedGain() => canMove = true;

    [UsedImplicitly]
    public void StopMovingOnUpdate() => Time.timeScale = 0f;
    
    public void RestartMovingOnUpdate() => Time.timeScale = 1f;

    #endregion

    private void OnValidate()
    {
        if(speedCurve.keys[0].value < 0f || speedCurve.keys[speedCurve.keys.Length - 1].value > 1f)
            Debug.LogError("Speed curve values need to be between 0 and 1");
    }
}
