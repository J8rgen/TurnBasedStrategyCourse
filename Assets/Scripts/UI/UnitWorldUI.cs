using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem; // Unit has the healthSystem

    private void Start() {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        // this one is static so will update UI elsewhere aswell

        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
    }

    

    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e) {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText() {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount =  healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

}
