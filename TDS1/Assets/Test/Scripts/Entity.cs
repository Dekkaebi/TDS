using UnityEngine;
using System.Collections;
public class Entity : MonoBehaviour {
    public float health;
    public HPBar hpbar;


    private void Start()
    {
        hpbar.SetHealth((int)health);
    }
    public void TakeDamage(float dmg) {
        health -= dmg;
        //hpbar.SetHealth((int)health);
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
