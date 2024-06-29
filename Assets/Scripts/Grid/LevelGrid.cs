using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* initializes a grid system, creates debug objects for visualization 
 * and provides methods to add, remove, and move units within the grid. */

public class LevelGrid : MonoBehaviour {

    public static LevelGrid Instance { get; private set; }



    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem gridSystem; // Reference to the GridSystem instance


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem(10, 10, 2f); // Initialize grid system with width, height, and cell size
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab); // Create debug objects for visualization (grid, texts)
    }

    // Add a unit to a specific grid position
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    // Get the list of units at a specific grid position
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    // Remove a unit from a specific grid position
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    // Update unit's grid position
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition) {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);
    }


    // Passthrough functions from GridSystem
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    // Short for closing brackets Lambda expression  Same as:
    // public GridPosition GetGridPosition(Vector3 worldPosition){ return gridSystem.GetGridPosition(worldPosition); }

    public bool isValidGridPosition(GridPosition gridPosition) => gridSystem.isValidGridPosition(gridPosition); // Lambda expression 

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) {

        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }




}
