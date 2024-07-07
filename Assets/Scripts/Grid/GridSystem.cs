using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

using System;

/* handles the creation and management of the grid. It initializes the grid dimensions and cell size, 
 * creates GridObject instances for each cell, and provides methods to convert between world positions 
 * and grid positions. */

/*GridSystem constructs GridPosition instances and uses them to manage GridObject instances within the grid*/

public class GridSystem<TGridObject> {
    // No MonoBehaviour, otherwise you cant use constructor

    // Grid dimensions and cell size
    // grid x and y coordinates as width and height
    private int width;
    private int height;
    private float cellSize;

    private TGridObject[,] gridObjectArray;  // 2D array of GridObjects

    // Constructor to initialize the GridSystem
    // Call it to create a grid, e.g., new GridSystem(10, 10, 2)
    // Func - delegate instead of: new TGridObject(this, gridPosition); // Object, position, return type (pass this delegate to create the object)
    public GridSystem(int width, int height, float cellSize, 
        Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject) { 

        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new TGridObject[width, height]; // Initialize the grid object array

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);

                // Create new grid objects (single cells)
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }



    }

    // Method to get the world position of a grid cell
    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize; // new instance of a object (here vector3)
    }


    // Method to get the grid position from a world position (where something exists in the 3D space)
    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize));
    }


    // Create debug objects for each grid cell
    public void CreateDebugObjects(Transform debugPrefab) { // can use gameObject instead of transform -> more work
        for (int x = 0; x < width; x++) {

            for (int z = 0; z < height; z++) {

                GridPosition gridPosition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                // new instance (clone) of debugPrefab, coordinates, rotation (Quaternion.identity = no rotation)
                // if you Instantiate a gameObject you get a gameObject, transform gives a transform

                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));

            }
        }
    }

    // Get the GridObject at a specific grid position
    public TGridObject GetGridObject(GridPosition gridPosition) {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition) {
        return gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.x < width && gridPosition.z < height;
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }
}