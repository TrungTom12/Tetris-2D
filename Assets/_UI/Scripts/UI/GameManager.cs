using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MaiMenu, Gameplay, Pause ,GameOver}
public class GameManager : Singleton<GameManager>
{
    private GameState m_State;
    private void Start()
    {
        ChangeState(GameState.MaiMenu);
    }

    public void ChangeState(GameState gameState)
    {
        this.m_State = gameState;
    }
    //neu dag dung la state hien tai thi moi hoat dong cac su kien
    public bool IsState(GameState gameState)
    {
        return this.m_State == gameState;
    }
}
