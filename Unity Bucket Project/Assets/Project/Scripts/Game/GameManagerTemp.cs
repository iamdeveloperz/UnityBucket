
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GameManagerTemp : SingletonBehavior<GameManagerTemp>
{
    #region Fields

    private int _score;

    public int Score
    {
        get => _score;
        private set
        {
            if (_score == value) return;
            _score = value;
            OnScoreChanged.Invoke(value);
        }
    }

    public event Action<int> OnScoreChanged = delegate { }; 

    #endregion

    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            ++Score;
        }
    }
    
    // Syntax Error (Abstract Method)
    protected override void RequirementOverride() { }
}

public sealed class SoundManager : MonoBehaviour
{
    
}