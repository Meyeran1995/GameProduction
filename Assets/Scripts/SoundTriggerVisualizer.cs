using UnityEngine;

public class SoundTriggerVisualizer : MonoBehaviour
{
    [SerializeField] private Color gizmoColor;
    [SerializeField] private BoxCollider2D triggerColl;

    private void Awake() => Destroy(this);

    private void OnDrawGizmos()
    {
        if(triggerColl == null) return;

        Gizmos.color = gizmoColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(transform.forward, triggerColl.size);
    }
}
