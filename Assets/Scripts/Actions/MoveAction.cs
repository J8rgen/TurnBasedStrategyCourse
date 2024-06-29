using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAction : BaseAction {

    [SerializeField] private Animator unitAnimator; // Animator component for unit animations (object with visual, animator)
    [SerializeField] private int maxMoveDistance = 4;

    //private const string IS_WALKING = "IsWalking";

    private Vector3 targetPosition; // Target position for movement

    protected override void Awake() {
        base.Awake();
        targetPosition = transform.position;  // Set initial target position to current position (so units dont move on start)
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        float stoppingDistance = .1f; // Distance at which the unit can stop moving, otherwise will keep trying to correct the distance

        Vector3 moveDirection = (targetPosition - transform.position).normalized; // target - current position (move direction vector)

        // Move the unit towards the target position
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) {

            
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime; // framerate independent


            unitAnimator.SetBool("IsWalking", true); // Set walking animation
        }
        else {
            unitAnimator.SetBool("IsWalking", false); // Stop walking animation
            isActive = false;
        }

        float rotateSpeed = 10f; // Unit Rotation speed
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        // unit rotation for movement, lerp for smoothing rotation
    }



    // Method to set a new target position for the unit
    public void Move(GridPosition gridPosition, Action onActionComplete) {
        this.onActionComplete = onActionComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition); // this object (the unit)
        isActive = true;
        onActionComplete();
    }


    public bool IsValidActionGridPosition(GridPosition gridPosition) { // returns true or false for valid grid position
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        // cycle all positions in maximum range
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++) {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++) {
                GridPosition offsetGridPostion = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPostion;

                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition)) {
                    // is in grid bounds
                    continue;
                }
                if (unitGridPosition == testGridPosition) {
                    // Same gridposition where the unit is already at
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) {
                    // Grid position already occupied with another unit
                    continue;
                }

                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }
     

}
