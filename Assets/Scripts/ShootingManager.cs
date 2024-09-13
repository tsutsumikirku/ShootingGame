using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystem‚ðŽg‚¤‚½‚ß‚Ìusing•¶

public class ShootingManager : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _shootInterbal;
    [SerializeField] float _zOffSet = 10;
    [SerializeField] float _sensivity = 10;
    [SerializeField] ParticleSystem _particles;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;
    [SerializeField] AudioClip _audioClip2;
    [SerializeField]AudioClip _audioClip3;
    GameObject _closshair2;
    GameObject _closshair;
    GameObject _beam;
    Animator _closshairAnim;
    bool _target = false;
    float _shoottime;
    Vector3 _crosshairPosition;
    GameObject _currentTarget; 

    private void Start()
    {
        _beam = GameObject.Find("ban");
        _particles = _beam.GetComponent<ParticleSystem>();
        _closshair = GameObject.Find("Crosshair");
        _closshair2 = GameObject.Find("Crosshair2");
        _closshairAnim = _closshair.GetComponent<Animator>();

  
        _crosshairPosition = new Vector3(Screen.width / 2, Screen.height / 2, _zOffSet);
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _shoottime += Time.deltaTime;

    
        float horizontal = Input.GetAxis("RightStickHorizontal") * _sensivity;
        float vertical = Input.GetAxis("RightStickVertical") * -1 * _sensivity;


        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            _crosshairPosition += new Vector3(horizontal, vertical, 0) * Time.deltaTime * 1000;
        }

   
        _crosshairPosition.x = Mathf.Clamp(_crosshairPosition.x, 0, Screen.width);
        _crosshairPosition.y = Mathf.Clamp(_crosshairPosition.y, 0, Screen.height);

   
        _closshair2.transform.position = _crosshairPosition;

        _crosshairPosition.z = _zOffSet;
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(_crosshairPosition);
        Vector3 direction = (worldPosition - transform.position).normalized;
        if (_shootInterbal < _shoottime && Input.GetButton("Fire1"))
        {
            StartCoroutine(VibrateController(0.5f, 0.5f)); 
            _beam.transform.forward = direction;
            Debug.Log("shoot");
            _particles.Play();
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("enemyy"))
                {
                    Debug.Log("hit");
                    GameObject _enemy = hit.collider.gameObject;
                    _enemy.GetComponent<EnemyBase>().Damage(10);
                    _audioSource.PlayOneShot(_audioClip2);
                }
             
            }
            else
            {
                _audioSource.PlayOneShot(_audioClip);
            }

            _shoottime = 0;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            Stop();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            _target = true;
            SetNearestTarget();
            _audioSource.PlayOneShot(_audioClip3);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            _target = false;
            _currentTarget = null; 
        }

        if (_target && _currentTarget != null)
        {
            Vector3 targetScreenPosition = _mainCamera.WorldToScreenPoint(_currentTarget.transform.position);
            _crosshairPosition = new Vector3(targetScreenPosition.x, targetScreenPosition.y, _zOffSet);
            _closshair2.transform.position = _crosshairPosition;
        }
    }
    void Stop()
    {
        _particles.Stop();
    }

    private void LateUpdate()
    {
        _closshairAnim.SetBool("Target", _target);
    }

    private void SetNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        float nearestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyScreenPosition = _mainCamera.WorldToScreenPoint(enemy.transform.position);
            float distance = Vector3.Distance(enemyScreenPosition, _crosshairPosition);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        _currentTarget = nearestEnemy;
    }
    IEnumerator VibrateController(float duration, float intensity)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.2f, 0.2f);
            yield return new WaitForSeconds(0.3f);
            Gamepad.current.SetMotorSpeeds(0, 0);
            yield return new WaitForSeconds(1);
        }
    }
}