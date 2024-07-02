using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged; // update turn ui, unit action points
    // this makes the script run order important, Unit script must be updated first. could change script execution order in project settings

    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one TurnSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void NextTurn() {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber() {
        return turnNumber;
    }

    public bool IsPlayerTurn() {
        return isPlayerTurn;
    }


}
