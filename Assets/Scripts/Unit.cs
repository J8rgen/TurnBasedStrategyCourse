using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    private GridPosition gridPosition; // Current grid position of the unit
    private MoveAction moveAction;
    private SpinAction spinAction;

    private void Awake() {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
    }
    private void Start() {
        // Get initial grid position and add unit to grid
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); // this = unit
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



}
