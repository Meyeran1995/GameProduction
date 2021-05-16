using JetBrains.Annotations;
using UnityEngine;

public class MuteCollisions : MonoBehaviour
{
    [UsedImplicitly]
    public void UnmuteCollisions() => FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MuteCollisions", 0);
}
