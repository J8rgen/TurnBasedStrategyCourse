using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    public static UnitActionSystem Instance { get; private set; }

    [SerializeField] private Unit unit;


    private void Start() {

    }

    //private void Update() {
    //    if (Input.GetKeyDown(KeyCode.T)) {
    //        GridSystemVisual.Instance.HideAllGridPosition();
    //        GridSystemVisual.Instance.ShowAllGridPositionList(unit.GetMoveAction().GetValidActionGridPositionList());
    //    }
    //}
    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition startGridPosition = new GridPosition(0, 0);
            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            for (int i = 0; i< gridPositionList.Count - 1; i++) {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                    Color.white,
                    10f
                );
            }
        }

    }


}
