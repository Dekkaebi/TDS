using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    private Vector3 cameraTarget;

    private Transform target;

    private void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        cameraTarget = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.deltaTime * 8);
    }

}
