using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour {

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI ActionPointsText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake() {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start() {

        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged; // subscribe
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged; // subscribe
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted; // for action points
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        CreateUnitActionButtons();
        UpdateSelectedVisual(); // for selected action
        UpdateActionPoints(); // Amount of points the unit has (update UI)
    }

    

    private void CreateUnitActionButtons() {
        // Destroy all existing buttons to refresh the UI
        foreach (Transform buttonTransform in actionButtonContainerTransform) {
            Destroy(buttonTransform.gameObject); 
        }
        actionButtonUIList.Clear(); 

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit(); // Get the currently selected unit

        // Create new buttons for each action of the selected unit
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray()) { // foreach (action in actionArray)
            // instantiate button prefabs in container
            Transform actionButtonTransform =  Instantiate(actionButtonPrefab, actionButtonContainerTransform); // Instantiate button prefab
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>(); // Get the ActionButtonUI component

            actionButtonUI.SetBaseAction(baseAction); // Assign the action to the button

            actionButtonUIList.Add(actionButtonUI);
        }
    }


    // Event handler for when the selected unit changes
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e) {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e) { // for action points
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual() { // selected action visual

        foreach (ActionButtonUI actionButtonUI in actionButtonUIList) {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints() { // Update UI
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();


        ActionPointsText.text = "ACTION POINTS: " + selectedUnit.GetActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        UpdateActionPoints();
    }


    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e) {
        UpdateActionPoints();
    }


}
