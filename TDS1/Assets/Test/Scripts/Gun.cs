using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
[RequireComponent (typeof (AudioSource))]
public class Gun : MonoBehaviour {

    public LayerMask collisianMask;
    public float rpm; // кол-во выстрелов в минуту
    public float damage = 5;

    public Transform spawn;

    private float secondBetweenShots;
    private float nextShootTime;

    private void Start() {
        secondBetweenShots = 60 / rpm; 
    }
    public void Shoot()
    {
        if (CanShoot()) {
            Ray ray = new Ray(spawn.position, spawn.forward);
            RaycastHit hit;
            float shotDistance = 20;

            if (Physics.Raycast(ray, out hit, shotDistance)) // коллизия 
            {
                shotDistance = hit.distance;

                if (hit.collider.GetComponent<Entity>()) {
                    hit.collider.GetComponent<Entity>().TakeDamage(damage);
                }
            }
            nextShootTime = Time.time + secondBetweenShots;
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 1); // проверка стрельбы(включи Gizmo)
        }
    }
    // проверка, возможен ли следующий выстрел
    private bool CanShoot() {
        bool canShoot = true;
        
        if (Time.time < nextShootTime) {
            canShoot = false;
        }
        return canShoot;
    }
    





}
