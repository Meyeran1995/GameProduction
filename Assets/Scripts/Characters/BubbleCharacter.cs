using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleCharacter : MonoBehaviour
{
    private Rigidbody2D bubbleBody;
    private PlayerInputController inputController;
    private Camera mainCam;

    [SerializeField] [Tooltip("With how much delay is the character following the mouse?")] [Range(0f, 0.5f)] private float movementDelay;
    private const int MAX_FPS = 60;

    private Vector2[] positionBuffer;
    private float[] timeBuffer;
    private int oldestIndex;
    private int newestIndex;

    private void Awake()
    {
        mainCam = Camera.main;
        bubbleBody = GetComponent<Rigidbody2D>();
        inputController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        int bufferLength = Mathf.CeilToInt(movementDelay * MAX_FPS);
        positionBuffer = new Vector2[bufferLength];
        timeBuffer = new float[bufferLength];

        positionBuffer[0] = positionBuffer[1] = transform.position;
        timeBuffer[0] = timeBuffer[1] = Time.time;

        oldestIndex = 0;
        newestIndex = 1;
    }

    private void FixedUpdate()
    {
        AddNewPosToCache(inputController.InputControls.Player.Aim.ReadValue<Vector2>());
        MoveToNextPos();
    }

    #region Delayed movement

    //Delay code taken from:
    //https://gamedev.stackexchange.com/questions/120189/unity-method-to-get-a-vector-which-is-x-seconds-behind-another-moving-vector-a/120225#120225

    private void AddNewPosToCache(Vector2 mousePos)
    {
        // Insert newest position into our cache.
        // If the cache is full, overwrite the latest sample.
        int newIndex = (newestIndex + 1) % positionBuffer.Length;
        if (newIndex != oldestIndex)
            newestIndex = newIndex;

        positionBuffer[newestIndex] = mousePos;
        timeBuffer[newestIndex] = Time.time;
    }

    private void MoveToNextPos()
    {
        // Skip ahead in the buffer to the segment containing our target time.
        float targetTime = Time.time - movementDelay;
        int nextIndex;
        while (timeBuffer[nextIndex = (oldestIndex + 1) % timeBuffer.Length] < targetTime)
            oldestIndex = nextIndex;

        // Interpolate between the two samples on either side of our target time.
        float span = timeBuffer[nextIndex] - timeBuffer[oldestIndex];
        float progress = 0f;
        if (span > 0f)
        {
            progress = (targetTime - timeBuffer[oldestIndex]) / span;
        }

        bubbleBody.MovePosition(mainCam.ScreenToWorldPoint(Vector3.Lerp(positionBuffer[oldestIndex], positionBuffer[nextIndex], progress)));
    }

    //private void OnDrawGizmos()
    //{
    //    if (_positionBuffer == null || _positionBuffer.Length == 0)
    //        return;

    //    Gizmos.color = Color.grey;

    //    Vector3 oldPosition = _positionBuffer[_oldestIndex];
    //    int next;
    //    for (int i = _oldestIndex; i != _newestIndex; i = next)
    //    {
    //        next = (i + 1) % _positionBuffer.Length;
    //        Vector3 newPosition = _positionBuffer[next];
    //        Gizmos.DrawLine(oldPosition, newPosition);
    //        oldPosition = newPosition;
    //    }
    //}

    #endregion
}
