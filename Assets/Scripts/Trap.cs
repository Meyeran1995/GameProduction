using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private Rigidbody2D obstacleBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        obstacleBody.bodyType = RigidbodyType2D.Dynamic;
    }
}
