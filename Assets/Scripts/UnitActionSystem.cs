using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour {

    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy; // cant perform a action while another is in progress


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one UnitActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        SetSelectedUnit(selectedUnit);
    }

    private void Update() {

        if (isBusy) { // when a action is in progress
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if (TryHandleUnitSelection()) { // unit selection, are we clicking unit
            return;
        }

        HandleSelectedAction();
    }

    private void HandleSelectedAction() {
        if (Input.GetMouseButtonDown(0)) {

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition()); // mouseGridposition

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition)){
                SetBusy();
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }
    }


    private void SetBusy() { // when a action is in progress set
        isBusy = true;
    }

    private void ClearBusy() { // when a action is in progress clear
        isBusy = false;
    }




    private bool TryHandleUnitSelection() {
        if (Input.GetMouseButton(0)) {

            // Ray object that starts at the camera's position and points through the specified screen point
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) { // current instance (private mousePlaneLayerMask)
                                                                                                  // "out" writes to "RaycastHit reaycastHit" variable instead
                                                                                                  // Added a layer to Plane on GameScene called "M
                                                                                                  //Hit something

                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) { // tries to find the component 
                    if (unit == selectedUnit) {
                        // Unit is alreay selected
                        return false;
                    }
                    SetSelectedUnit(unit); //if found "out Unit unit", set it
                    return true;
                }

            }
        }
        return false;
    }

    public void SetSelectedAction(BaseAction baseAction) {
        selectedAction = baseAction;
    }


    private void SetSelectedUnit(Unit unit) {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction()); // default to move actioon when change unit

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);//if (OnSelectedUnitChanged != null){OnSelectedUnitChanged(this, EventArgs.Empty);}

    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }


    public BaseAction GetSelectedAction() {
        return selectedAction;
    }

}