using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadoutState : AState
{
    public override void Enter(AState from)
    {
        gameObject.SetActive(true);
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
        return "LoadOut";
    }

    public void StartGame()
    {
        manager.SwitchState("Game");
    }
}
