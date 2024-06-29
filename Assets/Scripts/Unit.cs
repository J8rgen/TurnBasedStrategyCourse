using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    [SerializeField] private Animator unitAnimator; // Animator component for unit animations (object with visual, animator)

    //private const string IS_WALKING = "IsWalking";

    private Vector3 targetPosition; // Target position for movement

    private GridPosition gridPosition; // Current grid position of the unit

    private void Awake() {
        targetPosition = transform.position;  // Set initial target position to current position (so units dont move on start)
    }

    private void Start() {
        // Get initial grid position and add unit to grid
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); // this = unit
    }


    private void Update() {

        float stoppingDistance = .1f; // Distance at which the unit can stop moving, otherwise will keep trying to correct the distance

        // Move the unit towards the target position
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance ) {

            Vector3 moveDirection = (targetPosition - transform.position).normalized; // target - current position (move direction vector)
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime; // framerate independent

            float rotateSpeed = 10f; // Unit Rotation speed
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime); 
            // unit rotation for movement, lerp for smoothing rotation


            unitAnimator.SetBool("IsWalking", true); // Set walking animation
        }
        else {
            unitAnimator.SetBool("IsWalking", false); // Stop walking animation
        }

        // Update grid position if the unit has moved to a new grid cell
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition) {
            // Unit changed Grid position
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition); // move the unit
            gridPosition = newGridPosition; // set current position
        }


    }

    // Method to set a new target position for the unit
    public void Move(Vector3 targetPosition) {
        this.targetPosition = targetPosition; // this object (the unit)
    }



}
