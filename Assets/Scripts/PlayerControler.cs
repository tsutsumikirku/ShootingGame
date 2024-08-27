using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _moveSpeed;
    [SerializeField] float _maxz;
    [SerializeField] float _maxx;
    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _highPosision;
    [SerializeField] float _minPosision;
    [SerializeField] float _leftPosision;
    [SerializeField] float _rightPosision;
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        NormalMaxPosision();
    }
    private void FixedUpdate()
    {
        Normal();
    }
    void Normal()
    {
        float inputx = Input.GetAxis("Horizontal");
        float inputy = Input.GetAxis("Vertical");
        _rb.velocity = new Vector3(inputx * _moveSpeed, inputy * _moveSpeed * -1, 0);
        Quaternion targetRotation = Quaternion.Euler(_maxx * inputy, 0, _maxz * inputx);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }
    void NormalMaxPosision()
    {
        if(transform.position.y > _highPosision)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = _highPosision;
            transform.position = newPosition;
        }
        else if(transform.position.y < _minPosision)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = _minPosision;
            transform.position = newPosition;
        }
        if(transform.position.x > _rightPosision)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = _rightPosision;
            transform.position = newPosition;
        }
        else if(transform.position.x < _leftPosision)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = _leftPosision;
            transform.position = newPosition;
        }
    }
}