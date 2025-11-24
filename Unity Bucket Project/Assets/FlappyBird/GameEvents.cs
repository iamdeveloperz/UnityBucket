
using System;
using UnityEngine;

namespace FlappyBird.Core
{
    /// <summary>
    /// 게임 내 이벤트를 중앙에서 관리합니다
    /// Event-Driven Programming 패턴을 사용합니다
    /// </summary>
    public static class GameEvents
    {
        // 게임 상태 변경 이벤트
        public static event Action<GameState> OnGameStateChanged;
        
        // 점수 획득 이벤트
        public static event Action<int> OnScoreChanged;
        
        // 플레이어 사망 이벤트
        public static event Action OnPlayerDied;
        
        // 게임 시작 이벤트
        public static event Action OnGameStarted;

        /// <summary>
        /// 게임 상태 변경을 알립니다
        /// </summary>
        public static void RaiseGameStateChanged(GameState newState)
        {
            OnGameStateChanged?.Invoke(newState);
            Debug.Log($"[GameEvents] 게임 상태 변경: {newState}");
        }

        /// <summary>
        /// 점수 변경을 알립니다
        /// </summary>
        public static void RaiseScoreChanged(int newScore)
        {
            OnScoreChanged?.Invoke(newScore);
        }

        /// <summary>
        /// 플레이어 사망을 알립니다
        /// </summary>
        public static void RaisePlayerDied()
        {
            OnPlayerDied?.Invoke();
            Debug.Log("[GameEvents] 플레이어 사망");
        }

        /// <summary>
        /// 게임 시작을 알립니다
        /// </summary>
        public static void RaiseGameStarted()
        {
            OnGameStarted?.Invoke();
            Debug.Log("[GameEvents] 게임 시작");
        }

        /// <summary>
        /// 모든 이벤트 리스너를 정리합니다
        /// 메모리 누수 방지를 위해 씬 전환 시 호출하세요
        /// </summary>
        public static void ClearAllEvents()
        {
            OnGameStateChanged = null;
            OnScoreChanged = null;
            OnPlayerDied = null;
            OnGameStarted = null;
        }
    }
}