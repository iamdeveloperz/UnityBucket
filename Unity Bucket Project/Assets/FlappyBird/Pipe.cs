
using UnityEngine;
using FlappyBird.Core;
using FlappyBird.Score;

namespace FlappyBird.Obstacles
{
    /// <summary>
    /// 파이프(장애물)의 이동과 점수 체크를 담당합니다
    /// </summary>
    public class Pipe : MonoBehaviour
    {
        private float moveSpeed;
        private bool hasScored = false; // 점수를 이미 획득했는지 체크
        
        [SerializeField] private Transform scoreZone; // 점수 획득 영역

        /// <summary>
        /// 파이프를 초기화합니다
        /// </summary>
        public void Initialize(float speed)
        {
            moveSpeed = speed;
            hasScored = false;
        }

        private void Update()
        {
            // 게임이 진행 중일 때만 이동
            if (!GameManager.Instance.IsPlaying()) return;

            // 왼쪽으로 이동
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            // 화면 밖으로 나가면 비활성화 (풀로 반환될 예정)
            if (transform.position.x < -10f)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 플레이어가 파이프를 통과하면 점수 획득
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (hasScored) return;

            if (other.CompareTag("Player"))
            {
                hasScored = true;
                // ScoreManager에게 점수 추가 요청
                FindObjectOfType<ScoreManager>()?.AddScore();
            }
        }

        /// <summary>
        /// 파이프가 활성화될 때 초기화
        /// </summary>
        private void OnEnable()
        {
            hasScored = false;
        }
    }
}