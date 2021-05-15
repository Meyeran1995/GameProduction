using UnityEngine;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Obstacle : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    [Header("Base Values")]
    [SerializeField] [Tooltip("Mass for the object \n\nOnly relevant for falling or rolling obstacles, does nothing for bird obstacles!")] [Range(0.1f, 10f)] protected float obstacleMass;
    [SerializeField] [Tooltip("Available sprites to be randomly selected at runtime \n\nOnly relevant for falling or rolling obstacles, does nothing for bird obstacles!")] private ObstacleSpriteConfig spriteConfig;

    [Header("Base Sounds")]
    [EventRef] [SerializeField] [Tooltip("Sound to be played when the corresponding trap gets triggered")] protected string onTriggeredSound;
    [EventRef] [SerializeField] [Tooltip("A continuous sound to be played while the obstacle is active")] protected string continuousSound;
    private FMOD.Studio.EventInstance continuousInstance;
    

    protected void OnEnable()
    {
        if (rigidBody)
        {
            OnTriggeredPlay();
            OnContinuousPlay();
        }
        else
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
    }

    protected virtual void Awake()
    {
        Sprite sprite = spriteConfig.GetRandomSprite();
        GetComponent<SpriteRenderer>().sprite = sprite;
        ResizeCollider(sprite);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        continuousInstance.stop(STOP_MODE.ALLOWFADEOUT);
        if (!collision.gameObject.CompareTag("Player")) return;
        
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.mass = obstacleMass;
    }

    protected void OnBecameInvisible() => Destroy(gameObject);

    #region Sound

    private void OnContinuousPlay()
    {
        if(string.IsNullOrWhiteSpace(continuousSound)) return;
        continuousInstance = RuntimeManager.CreateInstance(continuousSound);
        continuousInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, rigidBody));
        continuousInstance.start();
    }
    private void OnTriggeredPlay()
    {
        if(string.IsNullOrWhiteSpace(onTriggeredSound)) return;

        RuntimeManager.PlayOneShotAttached(onTriggeredSound, gameObject);
    }

    protected void ResizeCollider(Sprite spriteToFit)
    {
        var capsuleCollider = GetComponent<CapsuleCollider2D>();

        capsuleCollider.offset = new Vector2(0, 0);
        capsuleCollider.size = new Vector3(spriteToFit.bounds.size.x,
            spriteToFit.bounds.size.y,
            spriteToFit.bounds.size.z);
        capsuleCollider.direction = CapsuleDirection2D.Horizontal;
    }

    #endregion

    protected void OnValidate()
    {
        if (spriteConfig == null) return;

        GetComponent<SpriteRenderer>().sprite = spriteConfig.GetFirstSprite();
        ResizeCollider(spriteConfig.GetFirstSprite());
    }
}
