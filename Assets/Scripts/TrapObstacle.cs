using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObstacle : MonoBehaviour
{
    [SerializeField] private Rigidbody2D obstacleBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        obstacleBody.bodyType = RigidbodyType2D.Dynamic;
    }
}
