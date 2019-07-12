using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManagerScript : MonoBehaviour
{
    private const int WorldMouseMaxDistance = 100;
    RaycastHit hit;
    List<Transform> selectedPlayerUnits = new List<Transform>();
    Camera mainCamera;

    // script variables
    public NavMeshAgent playerAgent;
    public LayerMask groundLayer;


    void Awake() {
        mainCamera = Camera.main;
    }
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1)) {
            moveSelectedUnits(target: getMouseWorldPosition());
        }

        if (!Input.GetMouseButtonDown(0)) { return; }

        var camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(camRay, out hit)) { return; }

        if (hit.transform.CompareTag("PlayerUnit")) {
            selectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift));
            return;
        }

        deselectUnits();

    }

    private void selectUnit(Transform playerUnit, bool isMultiselect = false) {

        if (!isMultiselect) { deselectUnits(); }
        selectedPlayerUnits.Add(playerUnit);

        highlightPlayerUnit(playerUnit);

    }

    private void deselectUnits() {

        selectedPlayerUnits.ForEach(playerUnit => unhighlightPlayerUnit(playerUnit));

        selectedPlayerUnits.Clear();

    }

    private void highlightPlayerUnit(Transform playerUnit)
        => playerUnit.Find("PlayerUnitHighlight").gameObject.SetActive(true);

    private void unhighlightPlayerUnit(Transform playerUnit)
        => playerUnit.Find("PlayerUnitHighlight").gameObject.SetActive(false);


    private Vector3 getMouseWorldPosition() {

        Vector2 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        RaycastHit hit;

        Physics.Raycast(
            origin: mouseWorldPosition,
            direction: mainCamera.transform.forward,
            out hit,
            maxDistance: WorldMouseMaxDistance,
            layerMask: groundLayer
        );

        return hit.point;

    }


    private void movePlayerUnit(Transform playerUnit, Vector3 target) {

        NavMeshAgent playerUnitNavMesh = playerUnit.gameObject.GetComponent<NavMeshAgent>();
        playerUnitNavMesh.SetDestination(target);

    }

    private void moveSelectedUnits(Vector3 target) {

        selectedPlayerUnits.ForEach(playerUnit => movePlayerUnit(playerUnit, target));
    }

}
