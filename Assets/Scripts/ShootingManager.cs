using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystemを使うためのusing文

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
    GameObject _currentTarget; // 現在のターゲットオブジェクト

    private void Start()
    {
        _beam = GameObject.Find("ban");
        _particles = _beam.GetComponent<ParticleSystem>();
        _closshair = GameObject.Find("Crosshair");
        _closshair2 = GameObject.Find("Crosshair2");
        _closshairAnim = _closshair.GetComponent<Animator>();

        // 初期クロスヘア位置を画面の中央に設定
        _crosshairPosition = new Vector3(Screen.width / 2, Screen.height / 2, _zOffSet);
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _shoottime += Time.deltaTime;

        // コントローラーの右スティックでエイム
        float horizontal = Input.GetAxis("RightStickHorizontal") * _sensivity;
        float vertical = Input.GetAxis("RightStickVertical") * -1 * _sensivity;

        // コントローラー入力でクロスヘアを移動
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            _crosshairPosition += new Vector3(horizontal, vertical, 0) * Time.deltaTime * 1000;
        }

        // クロスヘアを画面内に制限
        _crosshairPosition.x = Mathf.Clamp(_crosshairPosition.x, 0, Screen.width);
        _crosshairPosition.y = Mathf.Clamp(_crosshairPosition.y, 0, Screen.height);

        // クロスヘアの位置を更新
        _closshair2.transform.position = _crosshairPosition;

        // 3D空間でのクロスヘア位置を計算
        _crosshairPosition.z = _zOffSet;
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(_crosshairPosition);
        Vector3 direction = (worldPosition - transform.position).normalized;
        // トリガーボタンで射撃（コントローラーの右トリガー）
        if (_shootInterbal < _shoottime && Input.GetButton("Fire1"))
        {
            StartCoroutine(VibrateController(0.5f, 0.5f)); // 小刻みに振動
            _beam.transform.forward = direction;
            Debug.Log("shoot");
            _particles.Play();
           

            // 振動の設定 (小刻みに振動させる)
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
            {
                // ヒット処理
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

        // 左トリガーでターゲットモード切り替え
        if (Input.GetButtonDown("Fire2"))
        {
            _target = true;
            SetNearestTarget();
            _audioSource.PlayOneShot(_audioClip3);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            _target = false;
            _currentTarget = null; // ターゲットを解除
        }

        if (_target && _currentTarget != null)
        {
            // クロスヘアをターゲットに合わせる
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
        // "enemy"タグのついたオブジェクトをすべて取得
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        float nearestDistance = Mathf.Infinity; // 初期値は無限大に設定
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            // 敵のワールド座標をスクリーン座標に変換
            Vector3 enemyScreenPosition = _mainCamera.WorldToScreenPoint(enemy.transform.position);

            // クロスヘアと敵のスクリーン座標上の距離を計算
            float distance = Vector3.Distance(enemyScreenPosition, _crosshairPosition);

            // 最も近い敵を記録
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // 最も近い敵をターゲットに設定
        _currentTarget = nearestEnemy;
    }

    // コントローラーを小刻みに振動させるコルーチン
    IEnumerator VibrateController(float duration, float intensity)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.2f, 0.2f);
            yield return new WaitForSeconds(0.3f);
            Gamepad.current.SetMotorSpeeds(0, 0); // 振動を止める
            yield return new WaitForSeconds(1);
        }
    }
}