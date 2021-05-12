using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class MainCharacterMovement : AListenerEnabler
{
    #region Fields

    [Header("Resources")]
    [SerializeField] private CircularResourceBar mainResourceBar;
    [SerializeField] [Tooltip("How much of the maximum speed is gained by collecting feathers?")] [Range(0f, 1f)] private float pickupSpeedGain;

    [Header("Speed")]
    [SerializeField] [Tooltip("Maximum speed to be reached")] [Range(1f, 10f)] private float maxSpeed;
    [SerializeField] [Tooltip("Minimum speed after being hit")] [Range(0.1f, 10f)] private float minSpeed;
    [SerializeField] [Tooltip("How fast is speed gained? \nOnly use values between 0 and 1!")] private AnimationCurve speedCurve;
    [SerializeField] [Tooltip("How long does it take to gain maximum speed?")] private float rampTime;

    [Header("Speed Debug")] 
    [SerializeField] private float speedTime;
    [SerializeField] private float speedProgress, currentSpeed;

    // Movement
    private static readonly List<Checkpoint> CheckPoints = new List<Checkpoint>();
    private int currentCheckpointIndex;
    private Vector2 currentDirection;
    private Rigidbody2D characterBody;

    [Header("Movement States Debug")]
    [SerializeField] [Tooltip("Was the end of the journey reached?")] private bool journeyCompleted;
    [SerializeField] [Tooltip("Are we able to move?")] private bool canMove;

    [Header("Events")] 
    [SerializeField] [Tooltip("Event when reaching maximum speed")] private GameEvent maxSpeedEvent;
    [SerializeField] [Tooltip("Event when the character gets hit and loses speed")] private GameEvent speedLostEvent;

    #endregion

    private void Start()
    {
        characterBody = GetComponent<Rigidbody2D>();

        if (CheckPoints.Count != 0)
        {
            CheckPoints.Sort();
        }

        currentSpeed = minSpeed;
    }

    #region Movement

    private void FixedUpdate()
    {
        if (journeyCompleted || !canMove) return;

        RampUpSpeed();
        CheckPointCompletionProgress();
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
            Debug.LogError("No ground tiles left for movement!");
        }
        else
        {
            currentDirection = hit.transform.right * currentSpeed;
        }
    }

    private void MoveForward() => characterBody.MovePosition(characterBody.position + currentDirection * Time.fixedDeltaTime);

    public void RegisterCheckpoint(Checkpoint checkpoint) => CheckPoints.Add(checkpoint);

    /// <summary>
    /// Checks whether the current checkpoint has been passed. Passing the last checkpoint completes the journey
    /// </summary>
    private void CheckPointCompletionProgress()
    {
        var directionToCheckpoint = CheckPoints[currentCheckpointIndex].CheckPointPosition - transform.position;

        if (directionToCheckpoint.x > 0f) return;

        currentCheckpointIndex++;

        if (currentCheckpointIndex != CheckPoints.Count) return;

        journeyCompleted = true;
    }

    #endregion

    #region Speed

    /// <summary>
    /// Increase speed while moving
    /// </summary>
    private void RampUpSpeed()
    {
        if (currentSpeed >= maxSpeed) return;

        // Which time is it inside of the speed cycle?
        speedTime = Mathf.Clamp(speedTime + Time.fixedDeltaTime, 0f, rampTime);
        // How much progress did we make, according to time?
        speedProgress = Mathf.InverseLerp(0f, rampTime,speedTime);

        // Compare progress made to actual speed value, in case we picked up a feather
        if(speedProgress < mainResourceBar.FillAmount) return;

        // Calculate current speed based on progress
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedCurve.Evaluate(speedProgress));
        // Update Ui
        mainResourceBar.SetCurrentValue(speedProgress);

        if (currentSpeed < maxSpeed) return;

        currentSpeed = maxSpeed;
        maxSpeedEvent.Raise();
    }

    /// <summary>
    /// Halt movement, lose speed
    /// </summary>
    public void StaggerCharacterMovement()
    {
        canMove = false;
        speedLostEvent.Raise();
    }

    /// <summary>
    /// Start gaining speed again
    /// </summary>
    public void RestartSpeedGain()
    {
        currentSpeed = minSpeed;
        speedTime = 0f;
        // Update Ui
        mainResourceBar.SetCurrentValue(0f);
        canMove = true;
    }

    /// <summary>
    /// Instantly gain a pre-defined amount of speed
    /// </summary>
    public void GainSpeed()
    {
        currentSpeed = Mathf.Clamp(currentSpeed + pickupSpeedGain * maxSpeed, 0f, maxSpeed);
        mainResourceBar.SetCurrentValue(Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed));
    }

    #endregion

    private void OnValidate()
    {
        if(speedCurve.keys[0].value < 0f || speedCurve.keys[speedCurve.keys.Length - 1].value > 1f)
            Debug.LogError("Speed curve values need to be between 0 and 1");
    }
}
