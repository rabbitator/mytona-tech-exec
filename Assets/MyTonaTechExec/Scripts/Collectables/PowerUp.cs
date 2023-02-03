using MyTonaTechExec.PlayerUnit;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Collectables
{
    public class PowerUp : MonoBehaviour
    {
        [FormerlySerializedAs("Health")]
        [SerializeField]
        private int _health;
        [FormerlySerializedAs("Damage")]
        [SerializeField]
        private int _damage;
        [FormerlySerializedAs("MoveSpeed")]
        [SerializeField]
        private float _moveSpeed;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<Player>().Upgrade(_health, _damage, _moveSpeed);
            Destroy(gameObject);
        }
    }
}