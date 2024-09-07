using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _shootInterbal;
    [SerializeField] float _zOffSet = 10;
    GameObject _closshair2;
    GameObject _closshair;
    Animator _closshairAnim;
    bool _target = false;
    float _shoottime;

    private void Start()
    {
        _closshair = GameObject.Find("Crosshair");
        _closshair2 = GameObject.Find("Crosshair2");
        _closshairAnim = _closshair.GetComponent<Animator>();

    }

    void Update()
    {
        Cursor.visible = false;
        _shoottime += Time.deltaTime;
        Vector3 mousePosition = Input.mousePosition;
        _closshair2.transform.position = mousePosition;
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
        if (Input.GetButtonDown("Fire2"))
        {
            _target = true;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            _target = false;
        }
    }
    private void LateUpdate()
    {
        _closshairAnim.SetBool("Target",_target);
    }
}
