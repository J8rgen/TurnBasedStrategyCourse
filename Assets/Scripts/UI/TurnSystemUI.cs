using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour {

    [SerializeField] private Button endTurnBtn;
    [SerializeField] private TextMeshProUGUI turnNumberText;

    [SerializeField] private GameObject enemyTurnVisaualGameObject;

    private void Start() {
        endTurnBtn.onClick.AddListener(() => {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisability();
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e) {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisability();
    }

    private void UpdateTurnText() {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual() {
        enemyTurnVisaualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn()); // If it is the enemy turn
    }


    private void UpdateEndTurnButtonVisability() {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn()); // end turn button only active if players turn
    }

}
