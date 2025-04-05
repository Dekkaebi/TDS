using UnityEngine;
using System.Collections;
public class EntityEnemy : MonoBehaviour {
    public float healthEnemy;

    public void TakeDamage(float dmg) {
        healthEnemy -= dmg;
        if (healthEnemy <= 0) {
            Death();
        }
    }
    public void Death() {
        Debug.Log("Death");
        Destroy(gameObject);
    }

}
