using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("여기에 네트워크 오브젝트가 붙은 몬스터 프리팹을 할당하세요.")]
    public GameObject monsterPrefab;

    private void OnDrawGizmos()
    {
        // 씬 뷰에서 스폰 위치를 직관적으로 볼 수 있게 빨간색 구체를 그립니다.
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}