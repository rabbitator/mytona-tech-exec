using UnityEngine;

[ExecuteInEditMode]
public class ScaleParticles : MonoBehaviour
{
    private ParticleSystem.MainModule _mainModule;

    private void Awake()
    {
        _mainModule = GetComponent<ParticleSystem>().main;
    }

    private void Update()
    {
        _mainModule.startSize = transform.lossyScale.magnitude;
    }
}