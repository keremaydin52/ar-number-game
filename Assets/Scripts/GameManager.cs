using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public AState[] states;
    
    private List<AState> _stateStack = new List<AState>();
    private Dictionary<string, AState> _stateDict = new Dictionary<string, AState>();
    
    protected void OnEnable()
    {
        // We build a dictionary from state for easy switching using their name
        _stateDict.Clear();

        if (states.Length == 0) return;

        for(int i = 0; i < states.Length; ++i)
        {
            states[i].manager = this;
            _stateDict.Add(states[i].GetName(), states[i]);
        }

        _stateStack.Clear();

        PushState(states[0].GetName());
    }
    
    protected void Update()
    {
        if(_stateStack.Count > 0)
        {
            _stateStack[_stateStack.Count - 1].Tick();
        }
    }
    
    private void PushState(string name)
    {
        AState state;
        if(!_stateDict.TryGetValue(name, out state))
        {
            Debug.LogError("Can't find the state named " + name);
            return;
        }

        if (_stateStack.Count > 0)
        {
            _stateStack[_stateStack.Count - 1].Exit(state);
            state.Enter(_stateStack[_stateStack.Count - 1]);
        }
        else
        {
            state.Enter(null);
        }
        _stateStack.Add(state);
    }
    
    public void SwitchState(string newState)
    {
        print("New state: " + newState);
        AState state = FindState(newState);
        if (state == null)
        {
            Debug.LogError("Can't find the state named " + newState);
            return;
        }

        _stateStack[_stateStack.Count - 1].Exit(state);
        state.Enter(_stateStack[_stateStack.Count - 1]);
        _stateStack.RemoveAt(_stateStack.Count - 1);
        _stateStack.Add(state);
    }
    
    public AState FindState(string stateName)
    {
        AState state;
        if (!_stateDict.TryGetValue(stateName, out state))
        {
            return null;
        }

        return state;
    }
}
