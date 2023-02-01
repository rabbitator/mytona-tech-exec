using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
	public abstract WeaponType Type { get; }
	public GameObject Model;

	public enum WeaponType
	{
		Rifle,
		Shotgun,
		AutomaticRifle
	}

	protected virtual void Awake()
	{
		GetComponent<Player>().OnWeaponChange += Change;
	}

	protected virtual void OnDestroy()
	{
		EventBus<PlayerInputMessage>.Unsub(Fire);
	}

	protected void Change(WeaponType type)
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