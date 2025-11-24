
using UnityEngine;

// Udemy 참고 자료
// https://www.udemy.com/course/best-3d-c-unity/?couponCode=CP251120G2

/*
 * Generic Singleton (제네릭 싱글톤)
 *
 * 
 */

public abstract class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
{
    #region Fields & Properties

    // lock object (다중 쓰레드, 멀티 쓰레드 환경에서 네트워크 게임같은 경우가 있음)
    // 한 프레임에 모든 요청을 받지 않도록 막아주는거야
    // https://frozenpond.tistory.com/24
    private static readonly object _gate = new();
    private static volatile T _sInstance;

    private static bool _isDestroyed;
    private static bool _isInitialized;
    private static bool _applicationIsQuitting;
    
    public static bool IsDontDestroyOnLoad { get; set; }

    #endregion

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting || _isDestroyed) return null;
            lock (_gate) if (_sInstance) return _sInstance;
            lock (_gate)
            {
                if (_sInstance || _applicationIsQuitting || _isDestroyed) return _sInstance;

                _sInstance = FindFirstObjectByType<T>();

                // 찾았는데도 인스턴스가 존재하지 않을 경우
                if (!_sInstance)
                {
                    // Lazy Pattern (찾아보기)
                    var gameObject = new GameObject($"[Singleton] {typeof(T).Name}");
                    _sInstance = gameObject.AddComponent<T>();
                    
                    if (IsDontDestroyOnLoad && Application.isPlaying) DontDestroyOnLoad(gameObject);
                }

                if (!_isInitialized && _sInstance)
                {
                    
                }

                return _sInstance;
            }
        }
    }

    #region Unity Lifecycle

    protected virtual void Awake()
    {
        lock (_gate)
        {
            if (!_sInstance)
            {
                _sInstance = this as T;
                
                if (IsDontDestroyOnLoad && Application.isPlaying) DontDestroyOnLoad(gameObject);
                if (!_isInitialized)
                {
                    OnInitialize();
                    _isInitialized = true;
                }
            }
            else if (!Equals(_sInstance, this))
            {
                Destroy(gameObject);
            }
        }
    }

    #endregion
    
    // Virtual (구현이 반드시 필요한 건 아님)
    // Abstract (상속받는 자식에서 반드시 구현을 해줘야함 아니면? 문법적 에러가 발생)
    protected virtual void OnInitialize() { }
    protected virtual void OnCleanup() { }

    protected abstract void RequirementOverride();
    
#if UNITY_EDITOR
    // 필수는 아님 (혹여나 발생할 널레퍼런스나 에디터상에서 에러 발생확률을 줄이기위함)
    // 유니티 에디터 안에서만 돌아가는 것이고
    // 한번씩 static 변수들을 초기화(Reset) 시켜주는 역할
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticsInEditor()
    {
        _sInstance = null;
        _isDestroyed = false;
        _isInitialized = false;
        _applicationIsQuitting = false;
    }
#endif
}

public abstract class Singleton<T> where T : class, new()
{
    
}