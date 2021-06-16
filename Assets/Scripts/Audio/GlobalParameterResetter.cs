using System.Collections;
using FMODUnity;
using UnityEngine;

public class GlobalParameterResetter : MonoBehaviour, IRestartable
{
    [ParamRef] [SerializeField] private string paramName;
    private float initialValue;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => RuntimeManager.HasBankLoaded("Master"));
        
        RuntimeManager.StudioSystem.getParameterByName(paramName, out initialValue);
    }

    public void Restart() => RuntimeManager.StudioSystem.setParameterByName(paramName, initialValue);

    public void RegisterWithHandler() => GameRestartHandler.RegisterRestartable(this);
}
