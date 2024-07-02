using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Unit : MonoBehaviour {

    private const int ACTION_POINTS_MAX = 4;
    private int actionPoints = ACTION_POINTS_MAX; // points the unit has

    private GridPosition gridPosition; // Current grid position of the unit

    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;


    public static event EventHandler OnAnyActionPointsChanged; // gets fired when any instance of this class does something

    [SerializeField] private bool isEnemy;



    private void Awake() {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>(); // includes all actions
    }
    private void Start() {
        // Get initial grid position and add unit to grid
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); // this = unit

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }


    private void Update() {
        // Update grid position if the unit has moved to a new grid cell
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition) {
            // Unit changed Grid position
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition); // move the unit
            gridPosition = newGridPosition; // set current position
        }
    }

    public MoveAction GetMoveAction() {
        return moveAction;
    }

    public SpinAction GetSpinAction() {
        return spinAction;
    }


    public GridPosition GetGridPosition() { 
        return gridPosition; 
    }


    public Vector3 GetWorldPosition() {
        return transform.position;
    }



    public BaseAction[] GetBaseActionArray() { // can get the array for selected unit actions
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) {

        if (CanSpendActionPointsToTakeAction(baseAction)) {

            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else { 
            return false; 
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) {
        if (actionPoints >= baseAction.GetActionPointsCost()) {
            return true;
        }
        else {
            return false;
        }
    }

    private void SpendActionPoints(int amount) {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints() {
        return actionPoints;
    }


    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e) {
        if ( (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()) ) {
        // if enemy and enemy turn   or   if player and player turn    ->    then refresh
        
        actionPoints = ACTION_POINTS_MAX;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }

    }

    public bool IsEnemy() {
        return isEnemy;
    }


    public void Damage() {
        Debug.Log(transform + " damaged!");
    }

}
