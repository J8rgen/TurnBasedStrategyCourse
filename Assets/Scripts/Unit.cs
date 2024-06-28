using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    [SerializeField] private Animator unitAnimator; // object with visual, animator

    //private const string IS_WALKING = "IsWalking";

    private Vector3 targetPosition;

    private void Awake() {
        targetPosition = transform.position;  // so units dont move from current position to 0 0 0 on start (set it to current position)
    }


    private void Update() {


        float stoppingDistance = .1f; // distance it is fine to stop at, otherwise will keep trying to correct the distance

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance ) {

            Vector3 moveDirection = (targetPosition - transform.position).normalized; // target - current position (move direction vector)
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime; // framerate independent

            float rotateSpeed = 10f; // unit rotate
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime); 
            // unit rotation for movement, lerp for smoothing rotation


            unitAnimator.SetBool("IsWalking", true); // set animation
        }
        else {
            unitAnimator.SetBool("IsWalking", false); // set animation
        }


        


    }


    public void Move(Vector3 targetPosition) {
        this.targetPosition = targetPosition; // this object (the unit)
    }



}
