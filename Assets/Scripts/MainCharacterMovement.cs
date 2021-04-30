using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class MainCharacterMovement : AListenerEnabler
{
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
    private float currentSpeed, previousSpeed;

    [Header("Movement States")]
    [SerializeField] [Tooltip("Was the end of the journey reached?")] private bool journeyCompleted;
    [SerializeField] [Tooltip("Are we able to move?")] private bool canMove;
    [SerializeField] [Tooltip("Is the character currently slowed?")] private bool isSlowed;

    public bool HasStamina => !mainResourceBar.IsDepleted;

    [Header("Events")] 
    [SerializeField] [Tooltip("Event when reaching maximum speed")] private GameEvent maxSpeedEvent;
    [SerializeField] [Tooltip("Event when the character gets hit and loses speed")] private GameEvent speedLostEvent;

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

    public void RegisterCheckpoint(Checkpoint checkpoint) => CheckPoints.Add(checkpoint);
    private void MoveToCheckPoint() => characterBody.MovePosition(characterBody.position + currentDirection /** currentSpeed*/ * Time.fixedDeltaTime);

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

    public void SlowDownCharacter()
    {
        isSlowed = true;
        previousSpeed = currentSpeed;
        currentSpeed = minSpeed;
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void RegainSpeed()
    {
        currentSpeed = previousSpeed;
        GetComponent<SpriteRenderer>().color = Color.white;
        canMove = true;
        isSlowed = false;
    }

    public void RegainStamina() => mainResourceBar.IsReplenishing = true;

    public void IncreaseMaxStamina() => mainResourceBar.IncreaseMaxResource(staminaGain);

    public void StopStaminaRegain()
    {
        mainResourceBar.IsReplenishing = false;

        if (PlayerStateMachine.Instance.NumberOfCollidingObjects == 0)
        {
            PlayerStateMachine.Instance.ChangeState(new MovingState(gameObject));
        }
    }

    public void StaggerCharacterMovement()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
        mainResourceBar.DepleteResource(staggerDepletion);
        canMove = false;
        speedLostEvent.Raise();

        if (mainResourceBar.IsDepleted)
        {
            StopCharacterMovement();
        }
    }

    private void StopCharacterMovement()
    {
        currentSpeed = minSpeed;
        PlayerStateMachine.Instance.ChangeState(new DownedState(gameObject));
    }

    public void RestartCharacterMovement()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        canMove = true;
    }
}
