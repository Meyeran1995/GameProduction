using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class MainCharacterMovement : AListenerEnabler
{
    #region Fields

    [Header("Resources")]
    [SerializeField] private ResourceBar mainResourceBar;
    [SerializeField] [Tooltip("How much stamina is lost on being staggered?")] private float staggerDepletion;
    [SerializeField] [Tooltip("How much stamina is gained by collecting feathers")] private float staminaGain;

    [Header("Movement")]
    private static readonly List<Checkpoint> CheckPoints = new List<Checkpoint>();
    private int currentCheckpointIndex;
    private Vector2 currentDirection;
    private Rigidbody2D characterBody;

    [Header("Speed")]
    [SerializeField] [Tooltip("Maximum speed to be reached")] [Range(1f, 10f)] private float maxSpeed;
    [SerializeField] [Tooltip("Minimum speed after being hit")] [Range(0.1f, 10f)] private float minSpeed;
    [SerializeField] [Tooltip("How fast is speed gained?")] private AnimationCurve speedCurve;
    [SerializeField] private float speedIncrease, currentSpeed;

    [Header("Movement States")]
    [SerializeField] [Tooltip("Was the end of the journey reached?")] private bool journeyCompleted;
    [SerializeField] [Tooltip("Are we able to move?")] private bool canMove;
    [SerializeField] [Tooltip("How fast is the character getting back up?")] [Range(0.1f, 1f)] private float timeToGetUp;

    public bool HasStamina => !mainResourceBar.IsDepleted;

    public bool HasMaximumStamina => mainResourceBar.IsFull;

    [Header("Events")] 
    [SerializeField] [Tooltip("Event when reaching maximum speed")] private GameEvent maxSpeedEvent;
    [SerializeField] [Tooltip("Event when the character gets hit and loses speed")] private GameEvent speedLostEvent;
    [SerializeField] [Tooltip("Event when the character gets knocked down after losing all their stamina")] private GameEvent knockdownEvent;

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

    /// <summary>
    /// Halt movement, lose speed and deplete stamina
    /// </summary>
    public void StaggerCharacterMovement()
    {
        mainResourceBar.DepleteResource(staggerDepletion);
        canMove = false;
        speedLostEvent.Raise();

        // Check whether the character should be downed because of stamina depletion
        if (mainResourceBar.IsDepleted)
        {
            StopCharacterMovement();
        }
    }

    /// <summary>
    /// Stop moving and go into downed state
    /// </summary>
    private void StopCharacterMovement()
    {
        knockdownEvent.Raise();
        PlayerStateMachine.Instance.ChangeState(new DownedState(PlayerStateMachine.Instance, timeToGetUp));
    }

    #endregion

    #region Speed

    /// <summary>
    /// Regain speed after previously being slowed
    /// </summary>
    public void RestartSpeedGain()
    {
        currentSpeed = minSpeed;
        speedIncrease = 0f;
        canMove = true;
    }

    /// <summary>
    /// Increase speed while moving
    /// </summary>
    private void RampUpSpeed()
    {

        if (currentSpeed >= maxSpeed) return;

        speedIncrease = Mathf.Clamp01(speedIncrease + Time.fixedDeltaTime);
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedIncrease);

        if (currentSpeed < maxSpeed) return;

        currentSpeed = maxSpeed;
        maxSpeedEvent.Raise();
    }

    #endregion

    #region Stamina

    /// <summary>
    /// Start regaining stamina
    /// </summary>
    public void RegainStamina() => mainResourceBar.IsReplenishing = true;

    /// <summary>
    /// Stops the character from regaining stamina and checks whether we can move again
    /// </summary>
    public void StopStaminaRegain()
    {
        mainResourceBar.IsReplenishing = false;
        GetComponent<MainCharacterCollisionEvaluator>().AttemptToBeginMoving();
    }

    /// <summary>
    /// Increase maximum amount of stamina (via event)
    /// </summary>
    [UsedImplicitly] public void IncreaseMaxStamina() => mainResourceBar.IncreaseMaxResource(staminaGain);

    #endregion
}
