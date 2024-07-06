using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction {

    private float totalSpinAmount;



    private void Update() {
        if (!isActive) {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f) {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) { // gridposition not used, match BaseAction
        
        totalSpinAmount = 0;
        ActionStart(onActionComplete); // must be at the end of the function for camera
    }


    public override string GetActionName() {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition> {
            unitGridPosition
        };
    }

    public override int GetActionPointsCost() {
        return 2;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {

        return new EnemyAIAction() {
            gridPosition = gridPosition,
            actionValue = 0, // priority for taking action
        };
    }


}