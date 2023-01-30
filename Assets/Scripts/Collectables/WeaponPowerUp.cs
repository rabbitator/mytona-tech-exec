using UnityEngine;

public class WeaponPowerUp : MonoBehaviour
{
	public int Type;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<Player>().ChangeWeapon(Type);
			Destroy(gameObject);
		}
	}
}