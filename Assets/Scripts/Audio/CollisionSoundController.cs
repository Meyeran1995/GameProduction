using UnityEngine;

public class CollisionSoundController : MonoBehaviour
{
    public static void UnmuteCollisions() => FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MuteCollisions", 0);

    public static void MuteCollisions() => FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MuteCollisions", 1);
}
