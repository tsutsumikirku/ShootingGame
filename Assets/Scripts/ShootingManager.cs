using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _shootInterbal;
    [SerializeField] float _zOffSet = 10;
    [SerializeField] GameObject _closshair;
    float _shoottime;

    private void Start()
    {
    }

    void Update()
    {
        Cursor.visible = false;
        _shoottime += Time.deltaTime;
        Vector3 mousePosition = Input.mousePosition;
        _closshair.transform.position = mousePosition;
        mousePosition.z = _zOffSet;
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        Vector3 direction = (worldPosition - transform.position).normalized;   
        if (_shootInterbal < _shoottime && Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
            {
            }
            _shoottime = 0;
        }
    }
}
