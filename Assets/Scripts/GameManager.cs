using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameState _gameState     = GameState.Start;
    private void Awake()
    {
        if(Instance = null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    } 
}
public enum GameState
{
    Start,
    Setting,
    Ingame,
}
