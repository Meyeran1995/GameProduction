using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IComparable<Checkpoint>
{
    public Vector3 CheckPointPosition => transform.position;

    private static MainCharacterMovement PlayerMovement;

    private void Awake()
    {
        if(PlayerMovement == null)
            PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterMovement>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Start()
    {
        PlayerMovement.RegisterCheckpoint(this);
    }

    public int CompareTo(Checkpoint other)
    {
        return transform.position.x.CompareTo(other.transform.position.x);
    }
}
