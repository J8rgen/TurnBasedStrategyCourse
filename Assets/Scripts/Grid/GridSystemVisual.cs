using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class GridSystemVisual : MonoBehaviour {

    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;



    [Serializable]
    public struct GridVisualTypeMaterial {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType {
        White,
        Blue,
        Red,
        Yellow,
        RedSoft,
    }

    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;



    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than GridSystemVisual! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++) {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                // array with all the gridVisualSingle
                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        UpdateGridVisual();
    }

    

    public void HideAllGridPosition() {
        // cycle through the grid
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++) {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++) {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType) {

        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++) {
            for (int z = -range; z <= range; z++) {

                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    // is in grid bounds
                    continue;
                }


                // circle area
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range) {
                    continue;
                }

                gridPositionList.Add(testGridPosition);

            }
        }

        ShowAllGridPositionList(gridPositionList, gridVisualType);
    }



    public void ShowAllGridPositionList(List<GridPosition> gridPositionsList, GridVisualType gridVisualType) {
        foreach (GridPosition gridPosition in gridPositionsList) {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTyperMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual() {
        HideAllGridPosition();


        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;

        switch (selectedAction) {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
        }



        ShowAllGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);

    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, System.EventArgs e) {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) {
        UpdateGridVisual();
    }

    private Material GetGridVisualTyperMaterial(GridVisualType gridVisualType) {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList) {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType) {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null; // should never get down here
    }

}
