using UnityEngine;

public class ParallaxEffect : MonoBehaviour 
{
    private float length, startPos;
    private static Transform cinemachineVirtualFollowCam;
	[SerializeField] [Tooltip("How fast is the object going to scroll?")] [Range(0f, 1f)]private float effectStrength;
    [SerializeField] [Tooltip("Is the object moving to the left side of the screen?")] private bool moveLeft;

	private void Awake() 
    {
		startPos = transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x;

        if (cinemachineVirtualFollowCam == null)
        {
            cinemachineVirtualFollowCam = GameObject.FindGameObjectWithTag("ParallaxCam").transform;
        }

        if (!moveLeft) return;

        effectStrength *= -1f;
    }
	
	private void FixedUpdate () 
    {
		float temp = cinemachineVirtualFollowCam.position.x * (1-effectStrength);
		float dist = cinemachineVirtualFollowCam.position.x * effectStrength;

		transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
