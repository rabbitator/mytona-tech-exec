using UnityEngine;

namespace MyTonaTechExec.UI
{
    public class FaceToCamera : MonoBehaviour
    {
        private Transform _cameraTransform;
        
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            transform.rotation = _cameraTransform.transform.rotation;
        }
    }
}
