using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameState : AState
{
    public override void Enter(AState from)
    {
        gameObject.SetActive(true);
        StartGame();
    }

    public override void Exit(AState to)
    {
        gameObject.SetActive(false);
    }

    public override void Tick()
    {
    }

    public override string GetName()
    {
        return "Game";
    }

    public void StartGame()
    {
        TrackManager.Instance.Begin();
    }
    
    public void GameOver()
    {
        GameManager.Instance.SwitchState("GameOver");
    }
}
