using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Unit : MonoBehaviour {

    [SerializeField] private bool isEnemy;

    private const int ACTION_POINTS_MAX = 4;
    private int actionPoints = ACTION_POINTS_MAX; // points the unit has

    private GridPosition gridPosition; // Current grid position of the unit

    //private MoveAction moveAction;
    //private SpinAction spinAction;
    //private ShootAction shootAction;
    private BaseAction[] baseActionArray;

    private HealthSystem healthSystem;


    // gets fired when any instance of this class does something
    public static event EventHandler OnAnyActionPointsChanged; // static - for whole class

    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;




    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
        //moveAction = GetComponent<MoveAction>();
        //spinAction = GetComponent<SpinAction>();
        //shootAction = GetComponent<ShootAction>();
        baseActionArray = GetComponents<BaseAction>(); // includes all actions
    }
    private void Start() {
        // Get initial grid position and add unit to grid
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this); // this = unit

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty); // when a unit spawns
    }

    

    private void Update() {
        // Update grid position if the unit has moved to a new grid cell
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition) {
            // Unit changed Grid position
            GridPosition oldGridPosition = gridPosition;  // 5.10 course lecture
            gridPosition = newGridPosition; // set current position

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition); // move the unit
        }
    }

    public T GetAction<T>() where T : BaseAction { // Generics
        foreach (BaseAction baseAction in baseActionArray) {
            if (baseAction is T) { // if is of type T 
                return (T)baseAction; // return the baseAction
            }
        }
        return null;
    } // receive Type T , return type T, only receive where T extends BaseAction   replaced all these:
    //public MoveAction GetMoveAction() {
    //public SpinAction GetSpinAction() {
    //public ShootAction GetShootAction() {

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


    public void Damage(int damageAmount) { // took damage
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e) {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this); // remove the unit from grid position
        Destroy(gameObject);
        // also had to unSubscribe to the UnitActionSystem_OnSelectedUnitChanged; event on UnitSelectedVisual.cs

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() {
        return healthSystem.GetHealthNormalized();
    }










}