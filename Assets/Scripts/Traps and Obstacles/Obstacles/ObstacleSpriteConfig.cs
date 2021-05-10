using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ObstacleSpriteConfig")]
public class ObstacleSpriteConfig : ScriptableObject
{
    [SerializeField] private Sprite[] obstacleSprites;
    private bool randomIsInitialized;

    public Sprite GetRandomSprite()
    {
        if (!randomIsInitialized)
        {
            Random.InitState(System.DateTime.Today.Millisecond);
            randomIsInitialized = true;
        }

        return obstacleSprites[Random.Range(0, obstacleSprites.Length - 1)];
    }

    public Sprite GetFirstSprite() => obstacleSprites[0];
}
