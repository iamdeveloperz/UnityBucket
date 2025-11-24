
using UnityEngine;
using UnityEngine.SceneManagement;
using FlappyBird.Settings;
using UnityEngine.InputSystem;

namespace FlappyBird.Core
{
    /// <summary>
    /// 게임의 전체 흐름을 관리하는 싱글톤 매니저입니다
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("설정")]
        [SerializeField] private GameSettings settings;

        // 현재 게임 상태
        private GameState currentState = GameState.Ready;
        
        // 게임 시작 카운트다운
        private float startTimer;

        public GameState CurrentState => currentState;
        public GameSettings Settings => settings;

        private void Awake()
        {
            // 싱글톤 패턴
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            // 이벤트 구독
            GameEvents.OnPlayerDied += HandlePlayerDied;
            
            // 초기 상태 설정
            ChangeState(GameState.Ready);
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제 (메모리 누수 방지)
            GameEvents.OnPlayerDied -= HandlePlayerDied;
        }

        private void Update()
        {
            // Ready 상태에서 입력이 들어오면 게임 시작
            if (currentState == GameState.Ready)
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    StartGame();
                }
            }
        }

        /// <summary>
        /// 게임을 시작합니다
        /// </summary>
        private void StartGame()
        {
            ChangeState(GameState.Playing);
            GameEvents.RaiseGameStarted();
        }

        /// <summary>
        /// 게임 상태를 변경합니다
        /// </summary>
        private void ChangeState(GameState newState)
        {
            if (currentState == newState) return;

            currentState = newState;
            GameEvents.RaiseGameStateChanged(newState);

            // 상태별 로직
            switch (newState)
            {
                case GameState.Ready:
                    Time.timeScale = 1f;
                    break;
                    
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                    
                case GameState.GameOver:
                    Time.timeScale = 0f; // 게임 일시정지
                    break;
            }
        }

        /// <summary>
        /// 플레이어 사망 처리
        /// </summary>
        private void HandlePlayerDied()
        {
            ChangeState(GameState.GameOver);
        }

        /// <summary>
        /// 게임을 재시작합니다
        /// </summary>
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// 게임이 진행 중인지 확인
        /// </summary>
        public bool IsPlaying() => currentState == GameState.Playing;
    }
}