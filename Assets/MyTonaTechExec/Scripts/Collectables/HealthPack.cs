using MyTonaTechExec.PlayerUnit;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.Collectables
{
    public class HealthPack : MonoBehaviour
    {
        [FormerlySerializedAs("Health")]
        [SerializeField]
        private int _health;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            other.GetComponent<Player>().Heal(_health);
            Destroy(gameObject);
        }
    }
}