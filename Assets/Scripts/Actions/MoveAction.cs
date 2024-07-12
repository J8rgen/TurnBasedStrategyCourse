using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAction : BaseAction {

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxMoveDistance = 4;

    //private const string IS_WALKING = "IsWalking";

    private List<Vector3> positionList; // Target position for movement
    private int currentPositionIndex;


    private void Update() {
        if (!isActive) {
            return;
        }


        Vector3 targetPosition = positionList[currentPositionIndex]; // current target position
        Vector3 moveDirection = (targetPosition - transform.position).normalized; // target - current position (move direction vector)

        // Rotate before moving
        float rotateSpeed = 10f; // Unit Rotation speed
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        // unit rotation for movement, lerp for smoothing rotation

        // Move
        float stoppingDistance = .1f; // Distance at which the unit can stop moving, otherwise will keep trying to correct the distance
        // Move the unit towards the target position
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) {

            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime; // framerate independent

        }
        else { 
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count) { // Reached the end
                // no more positions to follow
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }

        
    }



    // Method to set a new target position for the unit
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {

        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);



        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList) {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }


        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete); // must be at the end of the function for camera
    }


    

    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        // cycle all positions in maximum range
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++) {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++) {
                GridPosition offsetGridPostion = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPostion;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    // is in grid bounds
                    continue;
                }


                // change the area to not be a square
                //int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                //if (testDistance > maxMoveDistance) {
                //    continue;
                //}


                if (unitGridPosition == testGridPosition) {
                    // Same grid position where the unit is already at
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) {
                    // Grid position already occupied with another unit
                    continue;
                }

                // a wall or a blocked grid
                if (!Pathfinding.Instance.isWalkableGridPosition(testGridPosition)) {
                    continue;
                }

                //is there a path to a position (unreachable/ out of bounds)
                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition)) {
                    continue;
                }

                int pathfindingDistanceMultiplier = 10; // because we multiplied the costs by 10 fcost and so on
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier) {
                    // Path length is too long
                    continue;
                }

                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }


    public override string GetActionName() {
        return "Move";
    }


    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtGridPosition(gridPosition);

        return new EnemyAIAction() {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10, // the more targets at position, the bigger actionValue
        };
    }


}
