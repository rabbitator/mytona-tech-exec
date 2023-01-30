using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
	public const int Rifle = 0;
	public const int Shotgun = 1;
	public const int AutomaticRifle = 2;
	public abstract int Type { get; }
	public GameObject Model;

	protected virtual void Awake()
	{
		GetComponent<Player>().OnWeaponChange += Change;
	}

	protected virtual void OnDestroy()
	{
		EventBus<PlayerInputMessage>.Unsub(Fire);
	}

	protected void Change(int type)
	{
		EventBus<PlayerInputMessage>.Unsub(Fire);
		if (type == Type)
		{
			EventBus<PlayerInputMessage>.Sub(Fire);
		}
		Model.SetActive(type == Type);
	}

	protected abstract  void Fire(PlayerInputMessage message);
}