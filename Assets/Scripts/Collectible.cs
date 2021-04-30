using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class Collectible : MonoBehaviour
{
    [SerializeField] private GameEvent collectionEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collectionEvent.Raise();
        Destroy(gameObject);
    }
}
