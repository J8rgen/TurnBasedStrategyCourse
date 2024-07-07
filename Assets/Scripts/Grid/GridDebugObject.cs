using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; // textmeshpro

/* debugging purposes. visualizes the grid by displaying a text label on each grid cell, 
 * showing the position and units present. */

public class GridDebugObject : MonoBehaviour {
    //PathfindingGridDebugObject extends this

    [SerializeField] private TextMeshPro textMeshPro; // Text component for displaying grid info


    private object gridObject;

    public virtual void SetGridObject(object gridObject) { // Set the GridObject for this debug object
        this.gridObject = gridObject;
    }

    protected virtual void Update() {
        textMeshPro.text = gridObject.ToString(); 
    }

    
}
