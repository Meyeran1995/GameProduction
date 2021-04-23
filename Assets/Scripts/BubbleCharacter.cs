using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleCharacter : MonoBehaviour
{
    private Rigidbody2D bubbleBody;
    private Transform player;
    [SerializeField] private Vector3 positionOffset;

    private void Awake()
    {
        bubbleBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        bubbleBody.MovePosition(player.position + positionOffset);
    }

    private void OnValidate()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + positionOffset;
    }
}
