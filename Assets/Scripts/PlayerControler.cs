using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _hp = 100;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _maxz;
    [SerializeField] float _maxx;
    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _highPosition;
    [SerializeField] float _minPosition;
    [SerializeField] float _leftPosition;
    [SerializeField] float _rightPosition;
    [SerializeField] float _boostSpeed;
    [SerializeField] GameManager _gamemanager;
    [SerializeField] AudioSource _audiosourse;
    [SerializeField] AudioClip _audioClip;
    Slider _slider;
    float inputx;
    float inputy;
    float boosttime;
    bool boost;
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _slider = GameObject.Find("Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            boost = true;
            boosttime = 0;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            boost = false;
        }
        _slider.value = _hp;
    }

    private void FixedUpdate()
    {
        if(_hp <= 0)
        {
            _gamemanager.SetGameOver(gameObject);
        }
        Normal();
        NormalMaxPosition();
        boosttime += Time.deltaTime;
    }
    void Normal()
    {
        inputx = Input.GetAxis("Horizontal");
        inputy = Input.GetAxis("Vertical");

        // プレイヤーの移動
        Vector3 movement = new Vector3(inputx * _moveSpeed, inputy * _moveSpeed * -1, 0);
        _rb.velocity = movement;

        // プレイヤーの回転
        Quaternion targetRotation = Quaternion.Euler(_maxx * inputy, 0, _maxz * inputx);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }
    public void Damage(float damage)
    {
        DOTween.To(() => _hp,x => _hp = x,_hp - damage, 0.5f);
        _audiosourse.PlayOneShot(_audioClip);
    }

    void NormalMaxPosition()
    {
        // 高さの制限
        if (transform.position.y > _highPosition)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = _highPosition;
            transform.position = newPosition;
        }
        else if (transform.position.y < _minPosition)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = _minPosition;
            transform.position = newPosition;
        }

        // 左右の制限
        if (transform.position.x > _rightPosition)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = _rightPosition;
            transform.position = newPosition;
        }
        else if (transform.position.x < _leftPosition)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = _leftPosition;
            transform.position = newPosition;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Damage(20);
        }
    }
}