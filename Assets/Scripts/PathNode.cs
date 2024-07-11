using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// grid object represents a single node
public class PathNode {  // no mono

    private GridPosition gridPosition;

    private int gCost; // walking cost from start node
    private int hCost; // heuristic cost to reach end node
    private int fCost; // G + H (total cost) (start node to current node + estimate to end node)
    private PathNode cameFromPathNode; // last node we were on

    private bool isWalkable = true;

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

    public void SetGCost(int gCost) {
        this.gCost = gCost;
    }

    public void SetHCost(int hCost) {
        this.hCost = hCost;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }


    public void ResetCameFromPathNode() {
        cameFromPathNode = null; // last node we were on
    }

    public void SetCameFromPathNode(PathNode pathNode) {
        cameFromPathNode = pathNode;
    }

    public PathNode GetCameFromPathNode() {
        return cameFromPathNode;
    }



    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public bool IsWalkable() {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
    }

}
