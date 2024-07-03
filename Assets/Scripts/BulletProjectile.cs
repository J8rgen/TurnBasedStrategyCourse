using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;
    private Vector3 targetPosition;


    public void Setup(Vector3 targetPosition) { // passed in animator
        this.targetPosition = targetPosition;
    }

    private void Update() {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        // bullets distance from target
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving) {
            transform.position = targetPosition; // make the bullet disappear exactly

            trailRenderer.transform.parent = null; // make the trail smoothly destroy itself
            Destroy(gameObject);

            Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity); // bullet hit target VFX
        }
    }


}
