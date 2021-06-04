using System.Collections;
using FMODUnity;
using UnityEngine;

public class BankLoadWaiter : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] behavioursToEnable;
    [SerializeField] private string bankName;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => RuntimeManager.HasBankLoaded(bankName));

        foreach (var script in behavioursToEnable)
        {
            script.enabled = true;
        }
    }
}
