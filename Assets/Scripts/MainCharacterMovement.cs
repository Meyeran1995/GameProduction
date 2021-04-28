using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{
    [SerializeField] private ResourceBar mainResourceBar;
    [Header("Movement")]
    private static readonly List<Checkpoint> CheckPoints = new List<Checkpoint>();
    private int currentCheckpointIndex;
    private Vector2 currentDirection;
    
    [SerializeField] [Range(1f, 10f)] private float speed;
    [SerializeField] private bool journeyCompleted, canMove;
    private float currentSpeed;

    private Rigidbody2D characterBody;

    private void Start()
    {
        characterBody = GetComponent<Rigidbody2D>();

        if (CheckPoints.Count != 0)
        {
            CheckPoints.Sort();
        }

        currentDirection = CheckPoints[0].CheckPointPosition - transform.position;
        currentDirection.Normalize();

        currentSpeed = speed;
    }

    private void FixedUpdate()
    {
        if (!journeyCompleted)
        {
            if (!canMove)
            {
                mainResourceBar.DepleteResource(Time.fixedDeltaTime);
                return;
            }

            CheckPointCompletionProgress();
            if (!journeyCompleted)
            {
                MoveToCheckPoint();
            }
        }
    }

    private void MoveToCheckPoint() => characterBody.MovePosition(characterBody.position + currentDirection * currentSpeed * Time.fixedDeltaTime);

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
            currentCheckpointIndex++;
        }
    }


    public void RegisterCheckpoint(Checkpoint checkpoint) => CheckPoints.Add(checkpoint);

    public void SlowDownCharacter()
    {
        currentSpeed = speed / 2f;
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void RegainSpeed()
    {
        currentSpeed = speed;
        GetComponent<SpriteRenderer>().color = Color.white;
        canMove = true;
    }

    public void StopCharacterMovement()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
        canMove = false;
    }

    public void RestartCharacterMovement()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        canMove = true;
    }
}
