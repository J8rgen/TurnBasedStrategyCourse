using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; // textmeshpro

/* debugging purposes. visualizes the grid by displaying a text label on each grid cell, 
 * showing the position and units present. */

public class GridDebugObject : MonoBehaviour {

    [SerializeField] private TextMeshPro textMeshPro; // Text component for displaying grid info


    private GridObject gridObject; // Reference to the GridObject this debug object represents

    public void SetGridObject(GridObject gridObject) { // Set the GridObject for this debug object
        this.gridObject = gridObject;
    }

    private void Update() {
        textMeshPro.text = gridObject.ToString(); // we overrite toString in gridObject
    }
    
}
