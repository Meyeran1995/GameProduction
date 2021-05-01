using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleCharacter : MonoBehaviour
{
    private Rigidbody2D bubbleBody;
    private PlayerInputController inputController;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        bubbleBody = GetComponent<Rigidbody2D>();
        inputController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
    }

    private void FixedUpdate()
    {
        Vector3 mousePos = inputController.InputControls.Player.Aim.ReadValue<Vector2>();
        mousePos = mainCam.ScreenToWorldPoint(mousePos);
        bubbleBody.MovePosition(mousePos);
    }
}
