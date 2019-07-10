using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerScript : MonoBehaviour
{

    RaycastHit hit;
    List<Transform> selectedPlayerUnits = new List<Transform>();

    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update()
    {

        if (!Input.GetMouseButtonDown(0)) { return; }

        var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
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

}
