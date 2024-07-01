using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour {

    [SerializeField] private Button endTurnBtn;
    [SerializeField] private TextMeshProUGUI turnNumberText;

    private void Start() {
        endTurnBtn.onClick.AddListener(() => {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e) {
        UpdateTurnText();
    }

    private void UpdateTurnText() {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

}
