using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* single cell within the grid. It stores the grid position and maintains a list of units present in that cell. 
 * provides methods to add and remove units and to get the list of units in the cell*/
public class GridObject {

    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;

    // Constructor to initialize the GridObject (single cell)
    public GridObject(GridSystem gridSystem, GridPosition gridPosition) {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    // Override ToString method to provide a string representation of the GridObject
    public override string ToString() {
        string unitString = "";
        foreach (Unit unit in unitList) {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }

    // Add a unit to this grid cell
    public void AddUnit(Unit unit) {
        unitList.Add(unit);
    }

    // Remove a unit from this grid cell
    public void RemoveUnit(Unit unit) {
        unitList.Remove(unit);
    }

    // Get the list of units in this grid cell
    public List<Unit> GetUnitList() {
        return unitList;
    }

}

