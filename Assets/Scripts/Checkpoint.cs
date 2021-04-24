using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IComparable<Checkpoint>
{
    public Vector3 CheckPointPosition => transform.position;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterMovement>().RegisterCheckpoint(this);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public int CompareTo(Checkpoint other)
    {
        return transform.position.x.CompareTo(other.transform.position.x);
    }
}
