using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public float spinSpeed = 1f;
    public float damage = 1;
    public PlayerType targetType;
    public static Action<Vector3> RockImpact;

    private void Update()
    {
        transform.Rotate(Vector3.forward * 360 * spinSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DealDamage(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        DealDamage(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        DealDamage(collision);
    }

    private void DealDamage(Collider2D collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (damageable.PlayerType == targetType)
            {
                damageable.TakeDamage(damage * transform.localScale.x);
                RockImpact?.Invoke(transform.position);
                if (gameObject != null)
                    Destroy(gameObject);
            }
        }
    }
}
