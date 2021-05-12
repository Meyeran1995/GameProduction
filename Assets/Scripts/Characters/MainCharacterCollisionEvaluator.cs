//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reacts to Collisions for the main character and changes state accordingly
/// </summary>
public class MainCharacterCollisionEvaluator : MonoBehaviour
{
    [SerializeField] private float staggerTime;
    private PlayerStateMachine stateMachine;

    [Header("Collisions")]
    //[SerializeField] private float collisionClearTime;
    [SerializeField] private int numberOfCollidingObjects;
    //private readonly List<GameObject> collidingGameObjects = new List<GameObject>();

    [Header("Collision Query")] 
    [SerializeField] private Vector3 boxSize;

    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //numberOfCollidingObjects++;

        //if (collidingGameObjects.Contains(collision.gameObject)) return;

        if (++numberOfCollidingObjects == 1)
        {
            stateMachine.ChangeState(new WaitingState(stateMachine, this, staggerTime));
        }

        //StartCoroutine(ClearOnGoingCollisionFromList(collision.gameObject));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (--numberOfCollidingObjects == 0 && stateMachine.Movement.HasStamina)
        //{
        //    stateMachine.ChangeState(new MovingState(stateMachine));
        //}
        --numberOfCollidingObjects;
    }

    /// <summary>
    /// Attempt to restart movement after stamina has been restored
    /// </summary>
    //public void AttemptToBeginMoving()
    //{
    //    StartCoroutine(WaitForEndOfCollisions());
    //}

    ///// <summary>
    ///// Waits for all remaining collisions to be cleared before restarting movement
    ///// </summary>
    //private IEnumerator WaitForEndOfCollisions()
    //{
    //    yield return new WaitUntil(() => numberOfCollidingObjects == 0);

    //    stateMachine.ChangeState(new MovingState(stateMachine));
    //}

    ///// <summary>
    ///// Clears colliding object from the list
    ///// </summary>
    //private IEnumerator ClearOnGoingCollisionFromList(GameObject collidingGameObject)
    //{
    //    collidingGameObjects.Add(collidingGameObject);

    //    yield return new WaitForSeconds(collisionClearTime);

    //    collidingGameObjects.Remove(collidingGameObject);
    //}

    public bool QueryForFrontalCollisions()
    {
        return Physics2D.OverlapBox(transform.position, Vector3.Scale(transform.localScale, boxSize), transform.localEulerAngles.z, 1 << 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // transform gizmo using this scripts transform matrix
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(transform.forward, boxSize);
    }
}
