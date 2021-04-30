using UnityEngine;

public class WindTrap : ATrap
{
    private static MainCharacterMovement MainCharacter;
    [SerializeField] private Transform windOrigin;
    private Vector3 blowDirection;
    private bool isBlowing;
    [SerializeField] private LayerMask mask;

    private void Awake()
    {
        if(MainCharacter == null)
            MainCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterMovement>();

        blowDirection = transform.position - windOrigin.position;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    protected override void TriggerTrap(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isBlowing = true;
    }

    private void FixedUpdate()
    {
        if(isBlowing)
            CheckForBubbleCollision();
    }

    private void CheckForBubbleCollision()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(windOrigin.position, blowDirection, blowDirection.magnitude + 1, mask);
        if (rayHit)
        {
            if (rayHit.transform != MainCharacter.transform)
            {
                //Debug.Log("Bubble hit");
                MainCharacter.RegainSpeed();
            }
            else
            {
                //Debug.Log("Player hit");
                MainCharacter.SlowDownCharacter();
            }
        }
        else
        {
            isBlowing = false;
            MainCharacter.RegainSpeed();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, windOrigin.position);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
