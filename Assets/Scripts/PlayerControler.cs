using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _maxz;
    [SerializeField] float _maxx;
    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _highPosision;
    [SerializeField] float _minPosision;
    [SerializeField] float _leftPosision;
    [SerializeField] float _rightPosision;
    [SerializeField] float _boostSpeed;
    float inputx;
    float inputy;
    bool boost;
    Rigidbody _rb;
    Animator _anim;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        Normal();
        NormalMaxPosision();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (inputy != 0 || inputx != 0)
            {
                boost = true;
            }
        }
    }
    private void LateUpdate()
    {
        _anim.SetFloat("Horizontal",inputx);
        _anim.SetFloat("Vertical", inputy);
        _anim.SetBool("Boost", boost);
    }
    void Normal()
    {
        inputx = Input.GetAxis("Horizontal");
        inputy = Input.GetAxis("Vertical");
        if (boost)
        {
            _rb.velocity = new Vector3(inputx * _moveSpeed, inputy * _moveSpeed * -1, 0) * _boostSpeed;
        }
        else
        {
            _rb.velocity = new Vector3(inputx * _moveSpeed, inputy * _moveSpeed * -1, 0);
        }
        Quaternion targetRotation = Quaternion.Euler(_maxx * inputy, 0, _maxz * inputx);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }
    void NormalMaxPosision()
    {
        if (transform.position.y > _highPosision)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = _highPosision;
            transform.position = newPosition;
        }
        else if (transform.position.y < _minPosision)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = _minPosision;
            transform.position = newPosition;
        }
        if (transform.position.x > _rightPosision)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = _rightPosision;
            transform.position = newPosition;
        }
        else if (transform.position.x < _leftPosision)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = _leftPosision;
            transform.position = newPosition;
        }
    }
}