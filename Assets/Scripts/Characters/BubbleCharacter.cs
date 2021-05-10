using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleCharacter : MonoBehaviour
{
    private Rigidbody2D bubbleBody;
    private PlayerInputController inputController;
    private Camera mainCam;

    [SerializeField] [Tooltip("With how much delay is the character following the mouse?")] [Range(0f, 0.5f)] private float movementDelay;
    private const int MAX_FPS = 60;

    private Vector2[] _positionBuffer;
    private float[] _timeBuffer;
    private int _oldestIndex;
    private int _newestIndex;

    private void Awake()
    {
        mainCam = Camera.main;
        bubbleBody = GetComponent<Rigidbody2D>();
        inputController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        int bufferLength = Mathf.CeilToInt(movementDelay * MAX_FPS);
        _positionBuffer = new Vector2[bufferLength];
        _timeBuffer = new float[bufferLength];

        _positionBuffer[0] = _positionBuffer[1] = transform.position;
        _timeBuffer[0] = _timeBuffer[1] = Time.time;

        _oldestIndex = 0;
        _newestIndex = 1;
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
        int newIndex = (_newestIndex + 1) % _positionBuffer.Length;
        if (newIndex != _oldestIndex)
            _newestIndex = newIndex;

        _positionBuffer[_newestIndex] = mousePos;
        _timeBuffer[_newestIndex] = Time.time;
    }

    private void MoveToNextPos()
    {
        // Skip ahead in the buffer to the segment containing our target time.
        float targetTime = Time.time - movementDelay;
        int nextIndex;
        while (_timeBuffer[nextIndex = (_oldestIndex + 1) % _timeBuffer.Length] < targetTime)
            _oldestIndex = nextIndex;

        // Interpolate between the two samples on either side of our target time.
        float span = _timeBuffer[nextIndex] - _timeBuffer[_oldestIndex];
        float progress = 0f;
        if (span > 0f)
        {
            progress = (targetTime - _timeBuffer[_oldestIndex]) / span;
        }

        bubbleBody.MovePosition(mainCam.ScreenToWorldPoint(Vector3.Lerp(_positionBuffer[_oldestIndex], _positionBuffer[nextIndex], progress)));
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
