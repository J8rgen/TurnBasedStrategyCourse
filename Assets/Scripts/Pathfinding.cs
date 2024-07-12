// Ignore Spelling: Pathfinding

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public static Pathfinding Instance { get; private set; }
    

    private const int MOVE_STARIGHT_COST = 10; // 1 move value
    private const int MOVE_DIAGONAL_COST = 14; // 1 move value diagonal ( sqrt(2) )


    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake() {

        if (Instance != null) {
            Debug.LogError("There is more than one Pathfinding!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        //need to be the same values as on LevelGrid
        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        // CAN COMMENT THIS OUT SO WE DONT SEE DEBG OBJECTS
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab); // Create debug objects


        // Search for obstacles on the map
        for(int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

                // Raycast wont work if its fired from inside a obstacle, we fire it from under
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask)) {
                    // there is an obstacle
                    GetNode(x,z).SetIsWalkable(false); // set to unwalkable
                }
            }
        }
    }


    // calculate and return the entire path
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition) {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);


        // cycle all nodes and reset their state
        // first set to max value
        for (int x = 0; x < gridSystem.GetWidth(); x++) {
            for (int z = 0; z < gridSystem.GetHeight(); z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode(); // last node we were on
            }
        }

        startNode.SetGCost(0); // first node
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0) { // all nodes on open list
            PathNode currentNode = GetLowestFCostPathNode(openList); // current node

            if (currentNode == endNode) { // check if we have final path
                // reached final node
                return CalculatePath(endNode); // start from end node
            }

            // we have searched the current node, add it to the closed list
            openList.Remove(currentNode); 
            closedList.Add(currentNode);
            // search neighbors

            foreach (PathNode neighbourNode in GetNeighborList(currentNode)) {
                if (closedList.Contains(neighbourNode)) { // if we have already searched them (closedList)
                    continue;
                }


                if (!neighbourNode.IsWalkable()) { // skip nodes that are unWalkable
                    closedList.Add(neighbourNode);
                    continue;
                }




                // GCost (start to current node cost)
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());


                // if its smaller, this movement to neighbour is faster than current
                //update neighbour with new stats
                if (tentativeGCost < neighbourNode.GetGCost()) {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();


                    //if not yet on open list then add it
                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        // no more open nodes  -  no path found
        return null;
    }

    public int CalculateDistance(GridPosition girdGridPositionA, GridPosition gridPositionB) {
        GridPosition gridPositionDistance = girdGridPositionA - gridPositionB;
        int distance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);

        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);

        int remaining = Mathf.Abs(xDistance - zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STARIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost()) {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z) {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }


    private List<PathNode> GetNeighborList(PathNode currentNode) {

        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();


        if ( gridPosition.x - 1 >= 0 ) {
            // Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0 ) {
                // Left Down
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight()) {
                // Left Up
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth()) {
            // Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            if (gridPosition.z - 1 >= 0) {
                // Right Down
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight()) {
                // Right Up
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.z - 1 >= 0) {
            // Down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }
        if (gridPosition.z + 1 < gridSystem.GetHeight()) {
            // Up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        return neighbourList;
    }




    private List<GridPosition> CalculatePath(PathNode endNode) {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);

        //work backwards, start from endNode
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null) {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode(); // switch to next node
        }

        //reverse list order for final list
        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList) {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;

    }

}
