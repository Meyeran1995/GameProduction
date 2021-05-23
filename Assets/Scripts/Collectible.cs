using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class Collectible : MonoBehaviour, IRestartable
{
    [SerializeField] private GameEvent collectionEvent;

    private void Start() => RegisterWithHandler();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collectionEvent.Raise();
        gameObject.SetActive(false);
    }

    public void Restart() => gameObject.SetActive(true);

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
