using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; 
using UnityEngine.UI; // buttons

public class ActionButtonUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction baseAction;


    public void SetBaseAction(BaseAction baseAction) {

        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper(); // // Update the button's text

        button.onClick.AddListener(() => {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        }); // anonymous function (lambda)
    }

    public void UpdateSelectedVisual() {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }

}