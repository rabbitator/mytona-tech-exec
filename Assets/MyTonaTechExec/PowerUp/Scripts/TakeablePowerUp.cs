using UnityEngine;

public class TakeablePowerUp : MonoBehaviour
{
    CustomizablePowerUp customPowerUp;

    void Start()
    {
        customPowerUp = transform.parent.gameObject.GetComponent<CustomizablePowerUp>();
        //this.audio.clip = customPowerUp.pickUpSound;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        PowerUpManager.Instance.Add(customPowerUp);
        if (customPowerUp.pickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(customPowerUp.pickUpSound, transform.position);
        }

        Destroy(transform.parent.gameObject);
    }
}