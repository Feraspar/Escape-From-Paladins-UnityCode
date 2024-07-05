using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : MonoBehaviour
{
    public PlayerStats stats;
    public int damage = 100;

    private void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            stats.TakeDamage(damage);
        }
    }
}
