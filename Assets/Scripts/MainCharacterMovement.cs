using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class MainCharacterMovement : AListenerEnabler
{
    #region Fields

    private Rigidbody2D characterBody;

    [Header("Resources")]
    [SerializeField] private ResourceBar mainResourceBar;
    [SerializeField] [Tooltip("How much stamina is lost on being staggered?")] private float staggerDepletion;
    [SerializeField] [Tooltip("How much stamina is gained by collecting feathers")] private float staminaGain;

    private static readonly List<Checkpoint> CheckPoints = new List<Checkpoint>();
    private int currentCheckpointIndex;
    private Vector2 currentDirection;
    
    [Header("Speed")]
    [SerializeField] [Tooltip("Maximum speed to be reached")] [Range(1f, 10f)] private float maxSpeed;
    [SerializeField] [Tooltip("Minimum speed after being hit")] [Range(0.1f, 10f)] private float minSpeed;
    [SerializeField] [Tooltip("How fast is speed gained?")] [Range(0.1f, 1f)] private float speedIncrease;
    [SerializeField] private float currentSpeed, previousSpeed;

    [Header("Movement States")]
    [SerializeField] [Tooltip("Was the end of the journey reached?")] private bool journeyCompleted;
    [SerializeField] [Tooltip("Are we able to move?")] private bool canMove;
    [SerializeField] [Tooltip("Is the character currently slowed?")] private bool isSlowed;

    public bool HasStamina => !mainResourceBar.IsDepleted;

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

        currentDirection = CheckPoints[0].CheckPointPosition - transform.position;
        currentDirection.Normalize();

        currentSpeed = minSpeed;
        currentDirection *= currentSpeed;
    }

    #region Movement

    private void FixedUpdate()
    {
        if (!journeyCompleted && canMove)
        {
            RampUpSpeed();
            CheckPointCompletionProgress();
            if (!journeyCompleted)
            {
                MoveToCheckPoint();
            }
        }
    }

    public void RegisterCheckpoint(Checkpoint checkpoint) => CheckPoints.Add(checkpoint);
    private void MoveToCheckPoint() => characterBody.MovePosition(characterBody.position + currentDirection * Time.fixedDeltaTime);

    private void CheckPointCompletionProgress()
    {
        var directionToCheckpoint = (CheckPoints[currentCheckpointIndex].CheckPointPosition - transform.position).normalized;

        if (Vector3.Dot(directionToCheckpoint, transform.right) < 0f)
        {
            if (currentCheckpointIndex + 1 == CheckPoints.Count)
            {
                journeyCompleted = true;
                return;
            }
            currentDirection = CheckPoints[currentCheckpointIndex + 1].CheckPointPosition - CheckPoints[currentCheckpointIndex].CheckPointPosition;
            currentDirection.Normalize();
            currentDirection *= currentSpeed;
            currentCheckpointIndex++;
        }
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
        currentSpeed = minSpeed;
        PlayerStateMachine.Instance.ChangeState(new DownedState(this));
    }

    /// <summary>
    /// Debug movement to be called from Editor script
    /// </summary>
    public void RestartCharacterMovement()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        canMove = true;
    }

    #endregion

    #region Speed

    /// <summary>
    /// Slow down movement to minimum speed
    /// </summary>
    public void SlowDownCharacter()
    {
        isSlowed = true;
        previousSpeed = currentSpeed;
        currentSpeed = minSpeed;
    }

    /// <summary>
    /// Regain speed after previously being slowed
    /// </summary>
    public void RegainSpeed()
    {
        currentSpeed = previousSpeed;
        canMove = true;
        isSlowed = false;
    }

    /// <summary>
    /// Increase speed while moving
    /// </summary>
    private void RampUpSpeed()
    {
        if (isSlowed) return;
        if (currentSpeed >= maxSpeed) return;

        currentSpeed += speedIncrease * Time.fixedDeltaTime;

        if (currentSpeed >= maxSpeed)
        {
            currentSpeed = maxSpeed;
            Debug.Log("Max speed reached");
            maxSpeedEvent.Raise();
        }
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

        if (PlayerStateMachine.Instance.NumberOfCollidingObjects == 0)
        {
            PlayerStateMachine.Instance.ChangeState(new MovingState(this));
        }
    }

    /// <summary>
    /// Increase maximum amount of stamina (via event)
    /// </summary>
    public void IncreaseMaxStamina() => mainResourceBar.IncreaseMaxResource(staminaGain);

    #endregion
}
