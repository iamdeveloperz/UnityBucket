
using TMPro;
using UnityEngine;

public sealed class UserInterfaceText : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameManagerTemp _gameManager;
    [SerializeField] private TMP_Text _scoreText;

    #endregion

    private void Start()
    {
        _scoreText.text = "Temporary";
    }

    private void OnEnable()
    {
        _gameManager.OnScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _gameManager.OnScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        _scoreText.text = score.ToString();
    }
}
