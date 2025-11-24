using UnityEngine;
using UnityEngine.InputSystem;
using FlappyBird.Core;
using FlappyBird.Settings;

namespace FlappyBird.Player
{
    /// <summary>
    /// 플레이어의 물리 기반 이동과 회전을 처리합니다
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rb;
        private GameSettings settings;
        private bool isDead = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            settings = GameManager.Instance.Settings;
            
            // 중력 스케일 적용
            rb.gravityScale = settings.gravityScale;
            
            // 초기에는 물리 비활성화
            rb.simulated = false;

            // 이벤트 구독
            GameEvents.OnGameStarted += HandleGameStarted;
            GameEvents.OnPlayerDied += HandlePlayerDied;
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제
            GameEvents.OnGameStarted -= HandleGameStarted;
            GameEvents.OnPlayerDied -= HandlePlayerDied;
        }

        private void Update()
        {
            if (isDead || !GameManager.Instance.IsPlaying()) return;

            // 플레이어 회전 처리 (속도에 따라)
            UpdateRotation();
            
            // 입력 처리 - Space 또는 마우스 왼쪽 버튼
            if (Keyboard.current.spaceKey.wasPressedThisFrame || 
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                Jump();
            }
        }

        /// <summary>
        /// 점프 동작을 수행합니다
        /// </summary>
        private void Jump()
        {
            // 기존 속도를 0으로 만들고 위로 힘을 가함
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * settings.jumpForce, ForceMode2D.Impulse);
        }

        /// <summary>
        /// 속도에 따라 플레이어를 회전시킵니다
        /// </summary>
        private void UpdateRotation()
        {
            // 속도에 따라 회전 각도 계산
            float targetRotation = 0f;

            if (rb.linearVelocity.y > 0)
            {
                // 위로 올라갈 때 위쪽으로 회전
                targetRotation = settings.maxRotationAngle;
            }
            else
            {
                // 아래로 떨어질 때 아래쪽으로 회전
                targetRotation = -settings.maxRotationAngle;
            }

            // 부드럽게 회전
            float currentRotation = transform.eulerAngles.z;
            if (currentRotation > 180f) currentRotation -= 360f;

            float newRotation = Mathf.Lerp(currentRotation, targetRotation, 
                settings.rotationSpeed * Time.deltaTime);
            
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }

        /// <summary>
        /// 게임 시작 시 호출
        /// </summary>
        private void HandleGameStarted()
        {
            rb.simulated = true;
            Jump(); // 시작과 동시에 점프
        }

        /// <summary>
        /// 플레이어 사망 시 호출
        /// </summary>
        private void HandlePlayerDied()
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;
        }

        /// <summary>
        /// 충돌 감지 (파이프나 바닥/천장에 닿으면 게임 오버)
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isDead) return;

            // "Obstacle" 태그를 가진 오브젝트와 충돌 시 사망
            if (other.CompareTag("Obstacle"))
            {
                Die();
            }
        }

        /// <summary>
        /// 플레이어 사망 처리
        /// </summary>
        private void Die()
        {
            if (isDead) return;
            
            isDead = true;
            GameEvents.RaisePlayerDied();
        }
    }
}