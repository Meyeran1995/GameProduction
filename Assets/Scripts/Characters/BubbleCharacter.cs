using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BubbleCharacter : AMultiListenerEnabler, IRestartable
{
    private Rigidbody2D bubbleBody;
    private PlayerInputController inputController;
    private Camera mainCam;

    [SerializeField]
    [Tooltip("How fast is the character going to sink after losing all its energy?")]
    [Range(1f, 10f)]
    private float sinkSpeed;

    private bool isMoving = true;
    private Coroutine sinkRoutine;

    [SerializeField] 
    [Tooltip("With how much delay is the character following the mouse?")] 
    [Range(0f, 0.5f)] 
    private float movementDelay;

    // Delayed movement fields
    private const int MAX_FPS = 60;
    private Vector2[] positionBuffer;
    private float[] timeBuffer;
    private int oldestIndex;
    private int newestIndex;

    #region Restart

    private Vector3 originalPosition;

    public void Restart()
    {
        isMoving = true;
        transform.position = originalPosition;
        SetUpBuffers();
        if(sinkRoutine != null)
            StopCoroutine(sinkRoutine);
        sinkRoutine = null;
    }

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);

    #endregion

    private void Awake()
    {
        mainCam = Camera.main;
        bubbleBody = GetComponent<Rigidbody2D>();
        inputController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
        originalPosition = transform.position;
        sinkSpeed = -sinkSpeed;
    }

    private void Start()
    {
       SetUpBuffers();
       RegisterWithHandler();
       gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        AddNewPosToCache(inputController.InputControls.Player.Aim.ReadValue<Vector2>());
        MoveToNextPos();
    }

    #region No Energy behaviour

    [UsedImplicitly]
    public void OnResourceDepleted()
    {
        isMoving = false;
        StartCoroutine(SinkToGround());
    }

    private IEnumerator SinkToGround()
    {
        // move downwards with a drift towards the x of last recorded position
        float drift = mainCam.ScreenToWorldPoint(positionBuffer[newestIndex]).x - transform.position.x;
        var characterRenderer = GetComponent<SpriteRenderer>();
        
        var downSpeed = new Vector2(drift, sinkSpeed);

        while (characterRenderer.isVisible)
        {
            bubbleBody.MovePosition(bubbleBody.position + downSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    [UsedImplicitly]
    public void OnEnergyRegained()
    {
        if (sinkRoutine != null)
            StopCoroutine(sinkRoutine);
        sinkRoutine = null;
        isMoving = true;
    }

    #endregion

    #region Delayed movement

    //Delay code adapted from:
    //https://gamedev.stackexchange.com/questions/120189/unity-method-to-get-a-vector-which-is-x-seconds-behind-another-moving-vector-a/120225#120225

    private void SetUpBuffers()
    {
        int bufferLength = Mathf.CeilToInt(movementDelay * MAX_FPS);
        positionBuffer = new Vector2[bufferLength];
        timeBuffer = new float[bufferLength];

        positionBuffer[0] = positionBuffer[1] = mainCam.WorldToScreenPoint(transform.position);
        timeBuffer[0] = timeBuffer[1] = Time.time;

        oldestIndex = 0;
        newestIndex = 1;
    }
    
    /// <summary>
    /// Tries to add a new position to the cache
    /// </summary>
    /// <param name="mousePos"></param>
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

    /// <summary>
    /// Moves to the next position, according to delayed targeted time
    /// </summary>
    private void MoveToNextPos()
    {
        // Skip ahead in the buffer to the segment containing our target time.
        float targetTime = Time.time - movementDelay;
        int nextIndex = (oldestIndex + 1) % timeBuffer.Length;

#if UNITY_EDITOR

        int startingIndex = nextIndex;

        while (timeBuffer[nextIndex] < targetTime)
        {
            oldestIndex = nextIndex;
            nextIndex = (oldestIndex + 1) % timeBuffer.Length;
            if (nextIndex != startingIndex) continue;
            Debug.LogError("No suitable target time found, breaking out of while loop");
            return;
        }
#else
        while (timeBuffer[nextIndex] < targetTime)
        {
            oldestIndex = nextIndex;
            nextIndex = (oldestIndex + 1) % timeBuffer.Length;
        }
#endif

        // Interpolate between the two samples on either side of our target time.
        float span = timeBuffer[nextIndex] - timeBuffer[oldestIndex];
        float progress = 0f;
        if (span > 0f)
        {
            progress = (targetTime - timeBuffer[oldestIndex]) / span;
        }
        
        bubbleBody.MovePosition(mainCam.ScreenToWorldPoint(Vector3.Lerp(positionBuffer[oldestIndex], positionBuffer[nextIndex], progress)));
    }

#endregion

}
