using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int Health;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<Player>().Heal(Health);
        Destroy(gameObject);
    }
}