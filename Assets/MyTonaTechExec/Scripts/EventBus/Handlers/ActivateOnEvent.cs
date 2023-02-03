using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.EventBus.Handlers
{
    public class ActivateOnEvent : MonoBehaviour
    {
        [FormerlySerializedAs("EventId")]
        [SerializeField]
        private int _eventId = 0;
        [FormerlySerializedAs("Button")]
        [SerializeField]
        private GameObject _button;

        private void Awake()
        {
            EventBus.Sub(HandleMessage, _eventId);
        }

        private void OnDestroy()
        {
            EventBus.Unsub(HandleMessage, _eventId);
        }


        public void HandleMessage()
        {
            _button.SetActive(true);
        }
    }
}