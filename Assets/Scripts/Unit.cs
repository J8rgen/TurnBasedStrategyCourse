using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    private Vector3 targetPosition;

    private void Update() {

        float stoppingDistance = .1f; // distance it is fine to stop at, otherwise will keep trying to correct the distance

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance ) {

            Vector3 moveDirection = (targetPosition - transform.position).normalized; // target - current position (move direction vector)
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime; // framerate independent
        }


        if (Input.GetKeyDown(KeyCode.T)) { // test move
            Move(new Vector3(4, 0, 4));
        }


    }


    private void Move(Vector3 targetPosition) {
        this.targetPosition = targetPosition; // this object (the unit)
    }



}
