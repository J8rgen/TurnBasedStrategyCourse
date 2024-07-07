using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// grid object represents a single node
public class PathNode {  // no mono

    private GridPosition gridPosition;

    private int gCost; // walking cost from start node
    private int hCost; // heuristic cost to reach end node
    private int fCost; // G + H (total cost) (start node to current node + estimate to end node)
    private PathNode cameFromPathNode; // last node
    
    public PathNode(GridPosition gridPosition) {
        this.gridPosition = gridPosition;
    }



    // Override ToString method 
    public override string ToString() {
        return gridPosition.ToString();
    }

    public int GetGCost() {
        return gCost;
    }

    public int GetHCost() {
        return hCost;
    }

    public int GetFCost() {
        return fCost;
    }

}
