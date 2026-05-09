using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;


public class StageManager : NetworkBehaviour
{
    private Dictionary<ulong, int> targetCountDict = new Dictionary<ulong, int>();
    private Dictionary<ulong, int> currentCountDict = new Dictionary<ulong, int>();

    public void SetupNewMap(int target, ulong mapId)
    {
        targetCountDict[mapId] = target;
        currentCountDict[mapId] = 0;
    }

    public void AddProgress(ulong mapId)
    {
        if (!IsServer) return;

        // 없는 맵이라면 무시
        if (!currentCountDict.ContainsKey(mapId)) return;

        currentCountDict[mapId]++;
        Debug.Log($"맵 {mapId} 진행도: {currentCountDict[mapId]} / {targetCountDict[mapId]}");

        if (currentCountDict[mapId] >= targetCountDict[mapId])
        {
            OpenWallRpc(mapId);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void OpenWallRpc(ulong mapId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(mapId, out NetworkObject mapObj))
        {
            var puzzleData = mapObj.GetComponent<MapControl>();
            if (puzzleData != null && puzzleData.targetWall != null)
            {
                Destroy(puzzleData.targetWall);
                Debug.Log($"맵 {mapId}의 벽이 파괴되었습니다!");
            }
        }
    }
}
