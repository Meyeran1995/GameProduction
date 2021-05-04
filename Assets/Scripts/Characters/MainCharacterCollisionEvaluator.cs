using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterCollisionEvaluator : MonoBehaviour
{
    [SerializeField] private float staggerTime;
    private PlayerStateMachine stateMachine;

    [Header("Collisions")]
    [SerializeField] private float collisionClearTime;
    [SerializeField] private int numberOfCollidingObjects;
    private readonly List<GameObject> collidingGameObjects = new List<GameObject>();

    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collidingGameObjects.Contains(collision.gameObject)) return;

        if (numberOfCollidingObjects == 0)
        {
            stateMachine.ChangeState(new StaggeredState(stateMachine, staggerTime));
        }

        StartCoroutine(ClearOnGoingCollisionFromList(collision.gameObject));

        numberOfCollidingObjects++;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (--numberOfCollidingObjects == 0 && stateMachine.Movement.HasStamina)
        {
            stateMachine.ChangeState(new MovingState(stateMachine));
        }
    }

    /// <summary>
    /// Attempt to restart movement after stamina has been restored
    /// </summary>
    public void AttemptToBeginMoving()
    {
        StartCoroutine(WaitForEndOfCollisions());
    }

    /// <summary>
    /// Waits for all remaining collisions to be cleared before restarting movement
    /// </summary>
    private IEnumerator WaitForEndOfCollisions()
    {
        yield return new WaitUntil(() => numberOfCollidingObjects == 0);

        stateMachine.ChangeState(new MovingState(stateMachine));
    }

    /// <summary>
    /// Clears colliding object from the list
    /// </summary>
    private IEnumerator ClearOnGoingCollisionFromList(GameObject collision)
    {
        collidingGameObjects.Add(collision.gameObject);

        yield return new WaitForSeconds(collisionClearTime);

        collidingGameObjects.Remove(collision);
    }
}
