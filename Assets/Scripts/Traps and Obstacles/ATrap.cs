using UnityEngine;


public abstract class ATrap : MonoBehaviour
{
    //[SerializeField] protected Obstacle obstacle;

    //protected virtual void Awake()
    //{
    //    obstacle = transform.GetChild(0).GetComponent<Obstacle>();
    //}

    protected void OnTriggerEnter2D(Collider2D collision) => TriggerTrap(collision);

    protected abstract void TriggerTrap(Collider2D collision);
}
