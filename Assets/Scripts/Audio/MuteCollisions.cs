using UnityEngine;

public class MuteCollisions : MonoBehaviour
{
    private void Awake()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MuteCollisions", 1);
    }

    public void UnmuteCollisions()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MuteCollisions", 0);
    }
}
