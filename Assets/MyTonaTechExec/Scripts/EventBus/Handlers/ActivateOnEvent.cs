using UnityEngine;

namespace MyTonaTechExec.EventBus.Handlers
{
	public class ActivateOnEvent : MonoBehaviour
	{
		public int EventId = 0;
		public GameObject Button;
		private void Awake()
		{
			global::MyTonaTechExec.EventBus.EventBus.Sub(HandleMessage,EventId);
		}

		private void OnDestroy()
		{
			global::MyTonaTechExec.EventBus.EventBus.Unsub(HandleMessage,EventId);
		}


		public void HandleMessage()
		{
			Button.SetActive(true);
		}
	}
}