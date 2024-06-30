using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; 
using UnityEngine.UI; // buttons

public class ActionButtonUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;

    public void SetBaseAction(BaseAction baseAction) {

        textMeshPro.text = baseAction.GetActionName().ToUpper(); // // Update the button's text

        button.onClick.AddListener(() => {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        }); // anonymous function (lambda)
    }

    

}