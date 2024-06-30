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
            isActive = false;
            onActionComplete(); // will callback the original function we passed
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) { // gridposition not used, match BaseAction
        this.onActionComplete = onActionComplete;
        isActive = true;
        totalSpinAmount = 0;

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
}