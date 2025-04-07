using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
[RequireComponent (typeof (AudioSource))]
public class Gun : MonoBehaviour {

    public LayerMask collisianMask;
    public float rpm; // ���-�� ��������� � ������
    public float damage = 5;

    [Header("FX")]
    //public ParticleSystem ShootFlashParticles;

    [Header("")]
    public Transform spawn;

    private float secondBetweenShots;
    private float nextShootTime;
    private bool _canShoot = true;

    private float shotDistance = 20; // Distance shooting

    private void Start() {
        secondBetweenShots = 60 / rpm; 
    }
    public void Shoot()
    {
        if (CanShoot()) {
            Ray ray = new Ray(spawn.position, spawn.forward);
            RaycastHit hit;
            //ShootFlashParticles.Play();
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 1);
            if (Physics.Raycast(ray, out hit, shotDistance)) // �������� 
            {
                shotDistance = hit.distance;

                if (hit.collider.GetComponent<Entity>()) {
                    hit.collider.GetComponent<Entity>().TakeDamage(damage);
                }
            }
            nextShootTime = secondBetweenShots;
            _canShoot = false;
        }
    }

    public void DebugShoot() // debug func
    {
        Ray ray = new Ray(spawn.position, spawn.forward);
        Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 1); // �������� ��������(������ Gizmo)
    }

    // ��������, �������� �� ��������� �������
    private bool CanShoot() {
        if (nextShootTime <= 0) {
            _canShoot = true;
        }
        nextShootTime -= Time.fixedTime;
        return _canShoot;
    }
    





}
