using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Collectables
{
    public class WeaponPowerUp : MonoBehaviour
    {
        [FormerlySerializedAs("Type")]
        [SerializeField]
        private PlayerWeapon.WeaponType _weaponType;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<Player>().ChangeWeapon(_weaponType);
            Destroy(gameObject);
        }
    }
}