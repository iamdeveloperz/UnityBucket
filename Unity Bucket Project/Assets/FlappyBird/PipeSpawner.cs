
using UnityEngine;
using FlappyBird.Core;
using FlappyBird.Settings;

namespace FlappyBird.Obstacles
{
    /// <summary>
    /// 파이프를 생성하고 오브젝트 풀로 관리합니다
    /// </summary>
    public class PipeSpawner : MonoBehaviour
    {
        [Header("프리팹")]
        [SerializeField] private Pipe pipePrefab;
        
        [Header("스폰 위치")]
        [SerializeField] private Transform spawnPoint;

        private ObjectPool<Pipe> pipePool;
        private GameSettings settings;
        private float spawnTimer;

        private void Start()
        {
            settings = GameManager.Instance.Settings;
            
            // 오브젝트 풀 초기화
            pipePool = new ObjectPool<Pipe>(
                pipePrefab, 
                settings.initialPoolSize, 
                transform
            );

            // 이벤트 구독
            GameEvents.OnGameStarted += HandleGameStarted;
        }

        private void OnDestroy()
        {
            GameEvents.OnGameStarted -= HandleGameStarted;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            // 일정 간격으로 파이프 생성
            spawnTimer += Time.deltaTime;
            
            if (spawnTimer >= settings.pipeSpawnInterval)
            {
                SpawnPipe();
                spawnTimer = 0f;
            }
        }

        /// <summary>
        /// 파이프를 생성합니다
        /// </summary>
        private void SpawnPipe()
        {
            // 풀에서 파이프 가져오기
            Pipe pipe = pipePool.Get();
            
            // Y축 랜덤 위치 설정
            float randomY = Random.Range(
                -settings.pipeRandomYRange, 
                settings.pipeRandomYRange
            );
            
            Vector3 spawnPosition = spawnPoint.position + new Vector3(0, randomY, 0);
            pipe.transform.position = spawnPosition;
            
            // 파이프 초기화
            pipe.Initialize(settings.pipeSpeed);
        }

        /// <summary>
        /// 게임 시작 시 첫 파이프 생성
        /// </summary>
        private void HandleGameStarted()
        {
            spawnTimer = settings.pipeSpawnInterval * 0.5f; // 조금 일찍 시작
        }
    }
}