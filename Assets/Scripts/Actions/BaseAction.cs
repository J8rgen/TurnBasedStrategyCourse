using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {
    // abstract - cant create an instance of this class, cant insentiate this class

    // classes that extend this extend can access - protected
    protected Unit unit;
    protected bool isActive;

    protected Action onActionComplete;// same as :
    // public delegate void SpinCompleteDelegate();
    // private SpinCompleteDelegate onSpinComplete;


    protected virtual void Awake() { // virtual - child classes can override
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName(); // abstract must have this function


    public abstract void TakeAction(GridPosition gridPostion, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition) {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

}