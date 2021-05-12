//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reacts to Collisions for the main character and changes state accordingly
/// </summary>
public class MainCharacterCollisionEvaluator : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

    [Header("Collisions")]
    [SerializeField] [Tooltip("Minimum amount of time the character will stand still for after being hit")] private float staggerTime;
    private int numberOfCollidingObjects;

    [Header("Collision Query")] 
    [SerializeField] private Vector3 boxSize;

    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (++numberOfCollidingObjects == 1)
        {
            stateMachine.ChangeState(new WaitingState(stateMachine, this, staggerTime));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        --numberOfCollidingObjects;
    }
    
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
