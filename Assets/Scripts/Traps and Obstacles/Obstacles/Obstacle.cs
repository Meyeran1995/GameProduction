using UnityEngine;
using FMODUnity;

public class Obstacle : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    [Header("Base Values")]
    [SerializeField] [Tooltip("Mass for the object \n\nOnly relevant for falling or rolling obstacles, does nothing for bird obstacles!")] [Range(0.1f, 10f)] protected float obstacleMass;
    [SerializeField] [Tooltip("Available sprites to be randomly selected at runtime \n\nOnly relevant for falling or rolling obstacles, does nothing for bird obstacles!")] private ObstacleSpriteConfig spriteConfig;

    [Header("Base Sounds")]
    [EventRef] [SerializeField] [Tooltip("Sound to be played when the corresponding trap gets triggered")] protected string onTriggeredSound;
    [EventRef] [SerializeField] [Tooltip("A continuous sound to be played while the obstacle is active")] protected string continuousSound;
    

    protected void OnEnable()
    {
        if (rigidBody)
        {
            OnTriggeredPlay();
        }
        else
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
    }

    protected virtual void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = spriteConfig.GetRandomSprite();
        gameObject.AddComponent<CircleCollider2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.mass = obstacleMass;
    }

    protected void OnBecameInvisible() => Destroy(gameObject);

    #region Sound

    private void OnTriggeredPlay()
    {
        if(string.IsNullOrWhiteSpace(onTriggeredSound)) return;

        RuntimeManager.PlayOneShotAttached(onTriggeredSound, gameObject);
    }

    

    #endregion

    protected void OnValidate()
    {
        if (spriteConfig == null) return;

        GetComponent<SpriteRenderer>().sprite = spriteConfig.GetFirstSprite();
    }
}
