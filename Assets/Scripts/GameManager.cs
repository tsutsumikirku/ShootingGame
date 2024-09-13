using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Threading;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState _gameState = GameState.Red;
    [SerializeField] GameObject _boss;
    [SerializeField] GameObject _red;
    [SerializeField] GameObject _blue;
    [SerializeField] GameObject _pl;
    [SerializeField] GameObject _gameOverObj;
    [SerializeField] GameObject _gameCleaObj;
    GameObject _redd;
    GameObject _blued;
    GameObject _bossd;
    float _timer;
    Animator _anim;
    Text _text;
    private void Start()
    {
        _text = GameObject.Find("Timer").GetComponent<Text>();
        Red();
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(_gameState != GameState.GameClear)
        {
            _timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(_timer / 60f);  // 分を計算
            int seconds = Mathf.FloorToInt(_timer % 60f);  // 秒を計算

            // テキストを更新 (分を三桁にして表示)
            _text.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        }
        
    }
    private void LateUpdate()
    {
        if(_gameState == GameState.Red) 
        {
            RedSignal();
        }
        else if (_gameState == GameState.Blue)
        {
            BlueSignal();
        }
        else if(_gameState == GameState.RedBlue)
        {
            RedBlueSignal();
        }
        else if(_gameState == GameState.Boss)
        {
            BossSignal();
        }
        else if(_gameState == GameState.GameOver)
        {
            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                _anim.Play("Load");
                GameObject.Find("scene").transform.SetAsLastSibling();
            }
        }
        else if(_gameState == GameState.GameClear)
        {
            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                _anim.Play("Load");
                GameObject.Find("scene").transform.SetAsLastSibling();
            }
        }
    }
    public void Sce()
    {
        SceneManager.LoadScene("SampleScene 1");
    }
    public void RedSignal()
    {
        if(_redd.gameObject.active == false)
        {
            Blue();
            _gameState = GameState.Blue;
         
        }
    }
    public void BlueSignal()
    {
        if (_blued.gameObject.active == false)
        {
            Red();
            Blue();
            _gameState = GameState.RedBlue;
        }
    }
    public void RedBlueSignal()
    {
        if (_blued.gameObject.active == false && _redd.gameObject.active == false)
        {
            Red();
            Blue();
            Boss();
            _gameState = GameState.Boss;
        }
    }
    public void BossSignal()
    {
        if (_blued.gameObject.active == false && _redd.gameObject.active == false && _bossd.gameObject.active == false)
        {
            _gameCleaObj.SetActive(true);
            _gameCleaObj.transform.SetAsLastSibling();
            int minutes = Mathf.FloorToInt(_timer / 60f);  
            int seconds = Mathf.FloorToInt(_timer % 60f);  
            GameObject.Find("Timer (1)").GetComponent<Text>().text = minutes.ToString("00") + ":" + seconds.ToString("00");
            Destroy(GameObject.Find("music"));
            _gameState = GameState.GameClear;
        }
    }
    public void SetGameOver(GameObject _player)
    {
        _pl.transform.position = _player.transform.position;
        Destroy( _player );
        Gamepad.current.SetMotorSpeeds(0, 0);
        _pl.gameObject.SetActive(true);
        _gameOverObj.gameObject.SetActive(true);
        _gameState = GameState.GameOver;
        _gameOverObj.transform.SetAsLastSibling();
        
    }
    public void Red()
    {
        _redd = Instantiate(_red);
    }
    public void Blue()
    {
       _blued = Instantiate (_blue);
    }
    public void Boss()
    {
       _bossd = Instantiate(_boss);
    }
}
public enum GameState
{
    Red,
    Blue,
    RedBlue,
    Boss,
    GameClear,
    GameOver
}
