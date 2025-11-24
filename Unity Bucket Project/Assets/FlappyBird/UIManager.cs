
using UnityEngine;
using TMPro;
using FlappyBird.Core;
using FlappyBird.Score;

namespace FlappyBird.UI
{
    /// <summary>
    /// 게임 UI를 관리합니다
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI 패널")]
        [SerializeField] private GameObject readyPanel;
        [SerializeField] private GameObject gameOverPanel;
        
        [Header("텍스트")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI bestScoreText;

        private ScoreManager scoreManager;

        private void Start()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            
            // 이벤트 구독
            GameEvents.OnGameStateChanged += HandleGameStateChanged;
            GameEvents.OnScoreChanged += HandleScoreChanged;
            
            // 초기 UI 설정
            UpdateUI(GameState.Ready);
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제
            GameEvents.OnGameStateChanged -= HandleGameStateChanged;
            GameEvents.OnScoreChanged -= HandleScoreChanged;
        }

        /// <summary>
        /// 게임 상태에 따라 UI를 업데이트합니다
        /// </summary>
        private void HandleGameStateChanged(GameState newState)
        {
            UpdateUI(newState);
        }

        /// <summary>
        /// 점수 변경 시 UI 업데이트
        /// </summary>
        private void HandleScoreChanged(int newScore)
        {
            scoreText.text = newScore.ToString();
        }

        /// <summary>
        /// UI 패널을 업데이트합니다
        /// </summary>
        private void UpdateUI(GameState state)
        {
            switch (state)
            {
                case GameState.Ready:
                    readyPanel.SetActive(true);
                    gameOverPanel.SetActive(false);
                    scoreText.text = "0";
                    break;
                    
                case GameState.Playing:
                    readyPanel.SetActive(false);
                    gameOverPanel.SetActive(false);
                    break;
                    
                case GameState.GameOver:
                    readyPanel.SetActive(false);
                    gameOverPanel.SetActive(true);
                    finalScoreText.text = $"점수: {scoreManager.CurrentScore}";
                    bestScoreText.text = $"최고 점수: {scoreManager.BestScore}";
                    break;
            }
        }

        /// <summary>
        /// 재시작 버튼 클릭 시 호출 (UI Button에 연결)
        /// </summary>
        public void OnRestartButtonClicked()
        {
            GameManager.Instance.RestartGame();
        }
    }
}