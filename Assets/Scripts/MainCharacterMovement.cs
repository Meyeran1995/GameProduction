using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{
    [SerializeField] private ResourceBar mainResourceBar;
    [Header("Movement")]
    private static readonly List<Vector3> checkPoints = new List<Vector3>();
    private int currentCheckpointIndex;
    private float currentLerpValue;
    private Vector3 currentStartPos;
    
    [SerializeField] [Range(0f, 1f)] private float speed;
    [SerializeField] private bool journeyCompleted, canMove;

    private Rigidbody2D characterBody;

    private void Start()
    {
        characterBody = GetComponent<Rigidbody2D>();

        if (checkPoints.Count != 0)
        {
            checkPoints.Sort((Vector3 a, Vector3 b) => a.x.CompareTo(b.x));
        }

        currentLerpValue = speed;
        currentStartPos = transform.position;
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

    private void MoveToCheckPoint()
    {
        characterBody.MovePosition(Vector3.Lerp(currentStartPos, checkPoints[currentCheckpointIndex], currentLerpValue));
        currentLerpValue = Mathf.Clamp01(currentLerpValue + speed);
    }

    private void CheckPointCompletionProgress()
    {
        if (currentLerpValue == 1f)
        {
            currentStartPos = checkPoints[currentCheckpointIndex++];
            currentLerpValue = speed;
        }

        journeyCompleted = currentCheckpointIndex == checkPoints.Count;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canMove = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canMove = true;
    }

    public void RegisterCheckpoint(Vector3 checkpointPos)
    {
        checkPoints.Add(checkpointPos);
    }

    public void UnStuckCharacter()
    {
        canMove = true;
    }
}
