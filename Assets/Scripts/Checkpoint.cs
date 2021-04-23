using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterMovement>().RegisterCheckpoint(transform.position);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
