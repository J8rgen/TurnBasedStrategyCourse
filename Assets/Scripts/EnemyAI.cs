using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour {

    private enum State {
        waitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake() {
        state = State.waitingForEnemyTurn;
    }



    private void Start() {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    

    private void Update() {
        if (TurnSystem.Instance.IsPlayerTurn()) { // enemy turn
            return;
        }

        switch (state) {
            case State.waitingForEnemyTurn:
                break;
            case State.TakingTurn:

                timer -= Time.deltaTime; // timer between enemy actions


                if (timer <= 0f) {
                    state = State.Busy; // taking enemy turn
                    if (TryTakeEnemyAIAction(SetStateTakingTurn)) {
                        state = State.Busy;
                    }
                    else {
                        // No more enemies have actions they can take, end turn
                        TurnSystem.Instance.NextTurn();
                    }

                }

                break;
            case State.Busy: 
                break;
        }


        
    }

    private void SetStateTakingTurn() {
        // when enemyAI action completes this callback will be called
        timer = 0.5f;
        state = State.TakingTurn;
    }


    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e) {
        if (!TurnSystem.Instance.IsPlayerTurn()) { // is enemy turn

            state = State.TakingTurn;
            timer = 3f;

        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete) {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList()) {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)) {
                return true; // try taking action
            }
        }
        return false; // if none of the units can take an action

    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete) {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(actionGridPosition)) { // right grid for action
            return false; // cant take action
        }
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) { // has enough actionPonts for action
            return false; // cant take action
        }
        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true; // can take action
    }


}