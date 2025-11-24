
using UnityEngine;

namespace FlappyBird.Settings
{
    /// <summary>
    /// 게임 설정값을 저장하는 ScriptableObject입니다
    /// 에디터에서 쉽게 밸런싱 조정이 가능합니다
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "FlappyBird/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("플레이어 설정")]
        [Tooltip("플레이어가 점프할 때 가해지는 힘")]
        public float jumpForce = 5f;
        
        [Tooltip("플레이어에게 적용되는 중력 배율")]
        public float gravityScale = 2f;
        
        [Tooltip("플레이어의 최대 회전 각도")]
        public float maxRotationAngle = 30f;
        
        [Tooltip("플레이어의 회전 속도")]
        public float rotationSpeed = 3f;

        [Header("파이프 설정")]
        [Tooltip("파이프가 이동하는 속도")]
        public float pipeSpeed = 3f;
        
        [Tooltip("파이프 생성 간격 (초)")]
        public float pipeSpawnInterval = 2f;
        
        [Tooltip("파이프 위아래 간격")]
        public float pipeGapSize = 2.5f;
        
        [Tooltip("파이프 Y축 랜덤 범위")]
        public float pipeRandomYRange = 2f;

        [Header("오브젝트 풀 설정")]
        [Tooltip("미리 생성할 파이프 개수")]
        public int initialPoolSize = 5;

        [Header("게임 설정")]
        [Tooltip("게임 시작 시 카운트다운 시간")]
        public float startCountdown = 1f;
    }
}