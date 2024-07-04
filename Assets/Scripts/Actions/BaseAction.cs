using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {
    // abstract - cant create an instance of this class, cant insentiate this class


    public static event EventHandler OnAnyActionStarted; // for action camera
    public static event EventHandler OnAnyActionCompleted; // for action camera


    // classes that extend this extend can access - protected
    protected Unit unit;
    protected bool isActive;

    protected Action onActionComplete;// same as :
    // public delegate void SpinCompleteDelegate();
    // private SpinCompleteDelegate onSpinComplete;


    protected virtual void Awake() { // virtual - child classes can override, (virtual default)
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName(); // abstract must have this function


    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition) {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();


    public virtual int GetActionPointsCost() { // default can be override
        return 1;
    }


    protected void ActionStart(Action onActionComplete) {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty); // for action camera
    }

    protected void ActionComplete() {
        isActive = false;
        onActionComplete();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty); // for action camera
    }



    public Unit GetUnit() {
        return unit;
    }
}