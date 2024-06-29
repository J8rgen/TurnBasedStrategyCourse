using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour {

    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private bool isBusy; // cant perform a action while another is in progress


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one UnitActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update() {

        if (isBusy) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {  // left click [0]; right [1]; and so on

            if (TryHandleUnitSelection()) return;
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)) {
                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
            }
            
        }

        if (Input.GetMouseButtonDown(1)){
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy); // delegate pass a funtion
        }

    }

    private void SetBusy() {
        isBusy = true;
    }

    private void ClearBusy() {
        isBusy = false;
    }







    private bool TryHandleUnitSelection() {

        // Ray object that starts at the camera's position and points through the specified screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) { // current instance (private mousePlaneLayerMask)
                                                                                              // "out" writes to "RaycastHit reaycastHit" variable instead
                                                                                              // Added a layer to Plane on GameScene called "M
            //Hit something

            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) { // tries to find the component 
                SetSelectedUnit(unit); //if found "out Unit unit", set it
                return true;
            }

        }
        return false;

    }


    private void SetSelectedUnit(Unit unit) {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);//if (OnSelectedUnitChanged != null){OnSelectedUnitChanged(this, EventArgs.Empty);}

    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

}
