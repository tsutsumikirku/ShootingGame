using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _moveSpeed;
    [SerializeField] float _maxz;
    [SerializeField] float _maxx;
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float inputx = Input.GetAxis("Horizontal");
        float inputy = Input.GetAxis("Vertical");
        _rb.velocity = new Vector3(inputx * _moveSpeed, inputy * _moveSpeed * -1, 0);
        transform.rotation = Quaternion.Euler(_maxx * inputy,0,_maxz * inputx);
        
    }
}
