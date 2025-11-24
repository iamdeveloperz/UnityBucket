
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird.Obstacles
{
    /// <summary>
    /// 오브젝트 풀링 패턴을 구현한 제네릭 클래스입니다
    /// 오브젝트를 재사용하여 성능을 최적화합니다
    /// </summary>
    /// <typeparam name="T">풀링할 컴포넌트 타입</typeparam>
    public class ObjectPool<T> where T : Component
    {
        private readonly T prefab;
        private readonly Transform parent;
        private readonly Queue<T> pool = new Queue<T>();

        /// <summary>
        /// 오브젝트 풀을 초기화합니다
        /// </summary>
        /// <param name="prefab">풀링할 프리팹</param>
        /// <param name="initialSize">초기 풀 크기</param>
        /// <param name="parent">부모 Transform (정리용)</param>
        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;

            // 초기 오브젝트 생성
            for (int i = 0; i < initialSize; i++)
            {
                T obj = CreateNewObject();
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        /// <summary>
        /// 풀에서 오브젝트를 가져옵니다
        /// 풀이 비어있으면 새로 생성합니다
        /// </summary>
        public T Get()
        {
            T obj;

            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
            }
            else
            {
                obj = CreateNewObject();
                Debug.Log($"[ObjectPool] 풀이 부족하여 새 오브젝트 생성: {typeof(T).Name}");
            }

            obj.gameObject.SetActive(true);
            return obj;
        }

        /// <summary>
        /// 오브젝트를 풀로 반환합니다
        /// </summary>
        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        /// <summary>
        /// 새 오브젝트를 생성합니다
        /// </summary>
        private T CreateNewObject()
        {
            T obj = Object.Instantiate(prefab, parent);
            return obj;
        }

        /// <summary>
        /// 현재 풀에 있는 오브젝트 개수
        /// </summary>
        public int PoolCount => pool.Count;
    }
}