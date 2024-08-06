using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float Damage;
    public PlayerHitbox Hitbox;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject != Hitbox.gameObject) return;

        PlayerHealth playerHealth = Hitbox.gameObject.GetComponentInParent<PlayerHealth>();

        playerHealth.ActiveCollisions += 1;
        playerHealth.CurrentCollisions.Add(Damage);

        playerHealth.TakeDamage(Damage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Hitbox == null) return;
        if (collision.gameObject != Hitbox.gameObject) return;

        PlayerHealth playerHealth = Hitbox.gameObject.GetComponentInParent<PlayerHealth>();

        playerHealth.ActiveCollisions -= 1;
        playerHealth.CurrentCollisions.Remove(Damage);
    }
}
