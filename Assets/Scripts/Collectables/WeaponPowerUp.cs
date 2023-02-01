using UnityEngine;

public class WeaponPowerUp : MonoBehaviour
{
    public PlayerWeapon.WeaponType Type;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<Player>().ChangeWeapon(Type);
        Destroy(gameObject);
    }
}