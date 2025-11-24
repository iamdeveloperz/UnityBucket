
using UnityEngine;
using FlappyBird.Core;

namespace FlappyBird.Score
{
    /// <summary>
    /// 점수를 관리하는 싱글톤 매니저입니다
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        private int currentScore = 0;
        private int bestScore = 0;

        public int CurrentScore => currentScore;
        public int BestScore => bestScore;

        private void Start()
        {
            // PlayerPrefs에서 최고 점수 불러오기
            bestScore = PlayerPrefs.GetInt("BestScore", 0);
            
            // 이벤트 구독
            GameEvents.OnGameStarted += HandleGameStarted;
        }

        private void OnDestroy()
        {
            GameEvents.OnGameStarted -= HandleGameStarted;
        }

        /// <summary>
        /// 점수를 1점 추가합니다
        /// </summary>
        public void AddScore()
        {
            currentScore++;
            GameEvents.RaiseScoreChanged(currentScore);

            // 최고 점수 갱신
            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                PlayerPrefs.SetInt("BestScore", bestScore);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// 게임 시작 시 점수 초기화
        /// </summary>
        private void HandleGameStarted()
        {
            currentScore = 0;
            GameEvents.RaiseScoreChanged(currentScore);
        }
    }
}