using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour {

    private static MouseWorld instance;

    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake() {
        instance = this;
    }


    public static Vector3 GetPosition() {

        // Ray object that starts at the camera's position and points through the specified screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask); // current instance (private mousePlaneLayerMask)
                                                                                                       // "out" writes to "RaycastHit reaycastHit" variable instead
                                                                                                       // Added a layer to Plane on GameScene called "M


        return raycastHit.point;

    }




}
