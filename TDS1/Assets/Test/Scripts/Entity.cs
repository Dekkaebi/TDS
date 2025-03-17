using UnityEngine;
using System.Collections;
public class Entity : MonoBehaviour {
    public float health;
    
    public void TakeDamage(float dmg) {
        health -= dmg;
        Debug.Log(health);
        if (health <= 0) {
            Death();
        }
    }
    public void Death() {
        Debug.Log("Death");
        Destroy(gameObject);
    }

}
