using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour {

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private void Start() {

        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged; // subscribe
        CreateUnitActionButtons();
    }

    // Event handler for when the selected unit changes
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e) {
        CreateUnitActionButtons(); 
    }

    private void CreateUnitActionButtons() {
        // Destroy all existing buttons to refresh the UI
        foreach (Transform buttonTransform in actionButtonContainerTransform) {
            Destroy(buttonTransform.gameObject); 
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit(); // Get the currently selected unit

        // Create new buttons for each action of the selected unit
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray()) { // foreach (action in actionArray)
            // instantiate button prefabs in container
            Transform actionButtonTransform =  Instantiate(actionButtonPrefab, actionButtonContainerTransform); // Instantiate button prefab
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>(); // Get the ActionButtonUI component

            actionButtonUI.SetBaseAction(baseAction); // Assign the action to the button
        }
    }

}
