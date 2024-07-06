using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction {

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs {
        public Unit targetUnit;
        public Unit shootingUnit;
    }


    private enum State {
        Aiming,
        Shooting,
        Cooloff,
    }

    private State state;
    private int maxShootDistance = 7;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;


    private void Update() {
        if (!isActive) {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state) {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f; // Unit Rotation speed
                transform.forward = Vector3.Lerp(transform.forward, aimDir, rotateSpeed * Time.deltaTime);
                // unit rotation for movement, lerp for smoothing rotation
                break;
            case State.Shooting:
                if (canShootBullet) {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f) {
            NextState();
        }
    }

    private void NextState() {
        switch (state) {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }

    }

    private void Shoot() {
        OnShoot?.Invoke(this, new OnShootEventArgs {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.Damage(40); // we shoot for 40 damage
    }


    public override string GetActionName() {
        return "Shoot";
    }


    public override List<GridPosition> GetValidActionGridPositionList() { 
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    // Can pass in any gridPosition and will get valid targets around that grid position
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition) {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // cycle all positions in maximum range
        for (int x = -maxShootDistance; x <= maxShootDistance; x++) {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++) {
                GridPosition offsetGridPostion = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPostion;

                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition)) {
                    // is in grid bounds
                    continue;
                }


                // change the area to not be a square
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance) {
                    continue;
                }
                

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) {
                    // Grid position is empty (no Units)
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy()) { 
                    // Both units are on the same team (IsEnemy boolean == for both)
                    continue;
                }

                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }


    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);


        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete); // important for this to be at the end for camera
    }




    public Unit GetTargetUnit() {
        return targetUnit;
    }

    public int GetMaxShootDistance() {
        return maxShootDistance;
    }



    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {

        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition); 


        return new EnemyAIAction() {
            gridPosition = gridPosition,

            // priority for taking action
            // shoot the enemy with the lowest health
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtGridPosition(GridPosition gridPosition) {
        return GetValidActionGridPositionList(gridPosition).Count;
    }


}
