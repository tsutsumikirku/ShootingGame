using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float _shootInterbal;
    [SerializeField] float _zOffSet = 10;
    float _shoottime;
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _zOffSet;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Ray shoot = new Ray(transform.position,worldPosition);
        if(_shootInterbal < _shoottime && Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(shoot, out RaycastHit hit))
            {
                
            }
            _shoottime = 0;
        }

    }
}
