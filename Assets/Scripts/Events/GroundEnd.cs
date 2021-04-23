using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class GroundEnd : MonoBehaviour
{
    [SerializeField] private GameEvent end = null;

    private void OnCollisionEnter(Collision collision)
    {
        end.Raise();
    }
}
