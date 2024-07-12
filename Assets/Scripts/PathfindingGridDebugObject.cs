// Ignore Spelling: Pathfinding

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class PathfindingGridDebugObject : GridDebugObject {

    [SerializeField] private TextMeshPro gCostText; // walking cost from start node
    [SerializeField] private TextMeshPro hCostText; // heuristic cost to reach end node
    [SerializeField] private TextMeshPro fCostText; // G + H (total cost) (start node to current node + estimate to end node)
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;


    private PathNode pathNode;



    public override void SetGridObject(object gridObject) {
        
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;  // cast to pathNode and store 
    }


    protected override void Update() {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();

        // if walkable -> green, otherwise red (inLine if)
        isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red; 
    }


}
