using UnityEngine;

/// <summary>
/// Reacts to Collisions for the main character and changes state accordingly
/// </summary>
public class MainCharacterCollisionEvaluator : MonoBehaviour, IRestartable
{
    private PlayerStateMachine stateMachine;

    [Header("Collisions")]
    [SerializeField] [Tooltip("Minimum amount of time the character will stand still for after being hit")] private float staggerTime;

    public float StaggerTime => staggerTime;
    private int numberOfCollidingObjects;

    [Header("Collision Query")] 
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private Vector3 boxOffset;

    private void Awake() => stateMachine = GetComponent<PlayerStateMachine>();

    private void Start() => RegisterWithHandler();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (++numberOfCollidingObjects == 1)
        {
            stateMachine.ChangeState(new WaitingState(gameObject, this, staggerTime));
        }
    }

    private void OnCollisionExit2D(Collision2D collision) => numberOfCollidingObjects = numberOfCollidingObjects != 0 ? numberOfCollidingObjects - 1 : 0;

    public bool QueryForFrontalCollisions() => Physics2D.OverlapBox(transform.TransformPoint(boxOffset),
                                            Vector3.Scale(transform.localScale, boxSize), transform.localEulerAngles.z, 1 << 0);


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // transform gizmo using this scripts transform matrix
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxOffset, boxSize);
    }

    public void Restart() => numberOfCollidingObjects = 0;
    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
