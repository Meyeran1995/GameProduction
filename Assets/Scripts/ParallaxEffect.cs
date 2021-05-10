using UnityEngine;

public class ParallaxEffect : MonoBehaviour 
{
    private float length, startPos;
    private static Transform CinemachineVirtualCam;
	[SerializeField] private float effectStrength;

	private void Awake() 
    {
		startPos = transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x;

        if (CinemachineVirtualCam != null) return;

        CinemachineVirtualCam = GameObject.FindGameObjectWithTag("ParallaxCam").transform;
    }
	
	private void FixedUpdate () 
    {
		float temp = (CinemachineVirtualCam.position.x * (1-effectStrength));
		float dist = (CinemachineVirtualCam.position.x * effectStrength);

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
