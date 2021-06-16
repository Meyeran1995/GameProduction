using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SetGlobalParameterOnTrigger2D : MonoBehaviour
{
    [ParamRef] [SerializeField] private string paramName;
    [SerializeField] private float overrideValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            RuntimeManager.StudioSystem.setParameterByName(paramName, overrideValue);
    }
}
