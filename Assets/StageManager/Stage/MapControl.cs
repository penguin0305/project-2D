using Unity.Netcode;
using UnityEngine;

public class MapControl : NetworkBehaviour
{
    [Tooltip("이 맵에 존재하는 트리거의 총 개수")]
    public int requiredNodesCount;

    [Tooltip("지워질 투명 벽")]
    public GameObject targetWall;

    public override void OnNetworkSpawn()
    {
        // 서버(호스트)에서 단 한 번만 등록을 수행
        if (IsServer)
        {
            var stageManager = FindAnyObjectByType<StageManager>();
            if (stageManager != null)
            {
                stageManager.SetupNewMap(requiredNodesCount, NetworkObjectId);
            }
        }
    }
}
