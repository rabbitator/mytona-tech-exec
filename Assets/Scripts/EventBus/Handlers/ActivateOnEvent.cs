using UnityEngine;

public class ActivateOnEvent : MonoBehaviour
{
	public int EventId = 0;
	public GameObject Button;
	private void Awake()
	{
		EventBus.Sub(HandleMessage,EventId);
	}

	private void OnDestroy()
	{
		EventBus.Unsub(HandleMessage,EventId);
	}


	public void HandleMessage()
	{
		Button.SetActive(true);
	}
}