using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public float Speed = 8;
    public bool DamagePlayer = false;
    public bool DamageMob;
    public float TimeToLive = 5f;
    private float timer = 0f;
    private bool destroyed = false;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (destroyed)
        {
            return;
        }

        if (DamagePlayer && other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(Damage);
            destroyed = true;
        }

        if (DamageMob && other.CompareTag("Mob"))
        {
            var mob = other.GetComponent<Mob>();
            mob.TakeDamage(Damage);
            destroyed = true;
        }
    }

    protected virtual void Update()
    {
        if (!destroyed)
        {
            transform.position += transform.forward * (Speed * Time.deltaTime);
        }

        timer += Time.deltaTime;
        if (timer > TimeToLive)
        {
            Destroy(gameObject);
        }
    }
}