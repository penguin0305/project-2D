using Unity.Netcode;
using UnityEngine;

public class MapInitializer : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        // 서버에서만 몬스터 스폰 처리를 담당합니다.
        if (!IsServer) return;

        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        // 맵 하위에 존재하는 모든 스폰 포인트 스크립트를 찾습니다.
        MonsterSpawnPoint[] spawnPoints = GetComponentsInChildren<MonsterSpawnPoint>();

        foreach (var point in spawnPoints)
        {
            if (point.monsterPrefab != null)
            {
                // 1. 지정된 위치와 회전값으로 몬스터 프리팹 Instantiate
                GameObject monster = Instantiate(point.monsterPrefab, point.transform.position, point.transform.rotation);

                // 2. 몬스터 네트워크 스폰
                NetworkObject netObj = monster.GetComponent<NetworkObject>();
                if (netObj != null)
                {
                    // 파라미터 true는 씬이 언로드되거나 부모 맵이 파괴될 때 몬스터도 같이 파괴됨을 의미합니다.
                    netObj.Spawn(true);

                    // [중요] 하이어라키 창 정리를 위해 생성된 몬스터를 맵의 자식으로 넣고 싶다면,
                    // 일반적인 transform.SetParent 대신 반드시 NGO 전용 메서드를 사용해야 합니다.
                    netObj.TrySetParent(this.transform);
                }
                else
                {
                    Debug.LogError($"[{point.gameObject.name}] 몬스터 프리팹에 NetworkObject 컴포넌트가 없습니다!");
                }
            }
        }
    }
}
