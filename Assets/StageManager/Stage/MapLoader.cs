using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;

public class MapLoader : NetworkBehaviour
{
    // 맵 프리셋에 대한 딕셔너리<난이도, 프리셋>
    private Dictionary<string, List<GameObject>> mapPool = new Dictionary<string, List<GameObject>>();
    private string key; // 맵 프리셋의 난이도를 결정하기 위한 키
    private Queue<GameObject> usedMap = new Queue<GameObject>(); // 맵 생성을 관리하기 위한 큐
    

    //맵 생성을 위한 프리팹 리스트 지정
    [Header("Pooling Candidate")]
    public List<GameObject> BeginningPreset;
    public List<GameObject> FirstHPreset;
    public List<GameObject> SecondHPreset;
    public List<GameObject> EndingPreset;

    //맵 생성 위치 결정을 위한 맵의 크기와 다음 맵 생성 위치변수. Y축만 고려. X축 필요 시 추가(예정없음)
    private float mapHeight = 16f; //실제 맵 크기에 따라 조정 필요. 1칸 = 1f
    private float nextMapY = 0f; // 다음에 불러올 맵의 위치변수
    private float Threshold = 14f; // 맵 생성 타이밍 감지를 위한 임계값 설정
    private int mapCount = 0; // 만들어진 맵의 수. 난이도 조절에 사용
    private int stageDepth = 5;

    private IObjectResolver _objectResolver;

    [Inject]
    public void Construct(IObjectResolver objectResolver)
    {
        _objectResolver = objectResolver;
    }

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return; // 서버에서만 맵 로딩 관리
        key = "beginning";
        mapCount += 1; // 난이도 설정을 위한 맵 카운트 증가
        nextMapY = -mapHeight; // 다음 맵이 로드될 위치 조정
        Debug.Log("nextMapY: " + nextMapY);
    }


    // 함수 이름: SetMapPool
    // 기능: 맵의 깊이에 따라 맵의 난이도를 결정하는 함수
    // 파라미터: X
    // 반환값: X
    public void SetMapPool()
    {
        if (mapCount < stageDepth)
        {
            mapPool[key] = BeginningPreset; // 초반부는 BeginningPreset에서 맵 선택
        }
        /*
        else if (mapCount < stageDepth * 2)
        {
            key = "firstH";
            mapPool[key] = FirstHPreset; // 전반부는 FirstHPreset에서 선택
        }
        else if (mapCount < stageDepth * 3)
        {
            key = "secondH";
            mapPool[key] = SecondHPreset; // 후반부는 SecondHPreset에서 선택
        }
        */
        else
        {
            key = "ending";
            mapPool[key] = EndingPreset; // 게임의 엔딩을 위한 스테이지는 EndingPreset에서 선택
        }
    }

    // 함수 이름: GetMap
    // 기능: 프리셋 내에서 중복되지 않게 랜덤한 맵을 결정하는 함수
    // 파라미터: X
    // 반환값: 불러오기로 결정된 맵 프리팹
    public GameObject GetMap()
    {
        bool isUsed = false; // 사용된 맵임을 확인하기 위한 변수

        while(true)
        {

            //랜덤하게 맵을 결정
            int mapIndex = Random.Range(0, mapPool[key].Count);
            GameObject SelectedMap = mapPool[key][mapIndex];

            
            foreach (GameObject used in usedMap)
            {
                if (SelectedMap == used)
                    isUsed = true;
            }

            //사용된 맵이 아니라면 usedMap에 넣고 리턴
            if(!isUsed)
            {
                usedMap.Enqueue(SelectedMap);
                return SelectedMap;
            }

            isUsed = false;
        }
    }

    // 함수 이름: SpawnMapPool
    // 기능: 실질적으로 씬에 맵을 불러오는 기능을 하는 함수
    // 파라미터: X
    // 반환값: X
    public void SpawnMapPool()
    {
        mapCount += 1; // 난이도 설정을 위한 맵 카운트 변경
        SetMapPool(); // 난이도 설정
        GameObject selectedMap = GetMap(); // 불러올 맵을 선택
        Vector3 spawnLoc = new Vector3(0, nextMapY, 0); // 선택한 맵을 로드할 위치 설정
        GameObject map = Instantiate(selectedMap, spawnLoc, Quaternion.identity); // 다음 맵을 로드
        _objectResolver.InjectGameObject(map);
        var networkObject = map.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Spawn();

            var playData = map.GetComponent<MapControl>();
            var stageManager = FindAnyObjectByType<StageManager>();

            if (playData != null)
            {
                stageManager.SetupNewMap(playData.requiredNodesCount, networkObject.NetworkObjectId);
            }
        }
        else
        {
            Debug.LogError("맵 프리팹에 네트워크 오브젝트 필요");
        }

        Debug.Log("nextMap Loaded");

        // 마지막 스테이지가 아니라면 다음 스폰 위치를 재설정
        if (key != "ending")
        {
            nextMapY -= mapHeight;
            Debug.Log("nextMapY: " + nextMapY);
        }
        else nextMapY = nextMapY - 20251118 * mapHeight;
    }

    
    void Update()
    {
        if (!IsServer) return;
        float playerLoc = float.MaxValue; // 플레이어 위치를 담기 위해 초기화

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            // 플레이어 오브젝트가 존재하는지 확인
            if (client.PlayerObject != null)
            {
                float playerY = client.PlayerObject.transform.position.y;
                if (playerY < playerLoc)
                {
                    playerLoc = playerY;
                }
            }
        }

        // 플레이어가 없는 경우(위치를 못받음) 리턴
        if (playerLoc == float.MaxValue) return;

        // 가장 아래에 있는 플레이어가 맵의 특정 깊이에 도달하면 다음 맵을 불러옴
        while (playerLoc < nextMapY + Threshold)
        {
            SpawnMapPool();
            Debug.Log("mapCount: " + mapCount);
        }
    }
}
