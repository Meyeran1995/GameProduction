using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleCharacter : MonoBehaviour
{
    private Rigidbody2D bubbleBody;
    private Transform player;
    private PlayerInputController inputController;
    private Camera mainCam;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private bool followMouse;

    private void Awake()
    {
        mainCam = Camera.main;
        bubbleBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inputController = player.GetComponent<PlayerInputController>();
    }

    private void FixedUpdate()
    {
        if (followMouse)
        {
            Vector3 mousePos = inputController.InputControls.Player.Aim.ReadValue<Vector2>();
            mousePos = mainCam.ScreenToWorldPoint(mousePos);
            bubbleBody.MovePosition(mousePos);
        }
        else
        {
            bubbleBody.MovePosition(player.position + positionOffset);
        }
    }

    private void OnValidate()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + positionOffset;
    }
}
