using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Weapon;
using UnityEngine;

namespace MyTonaTechExec.Collectables
{
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
}