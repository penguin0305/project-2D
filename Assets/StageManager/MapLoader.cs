using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.Rendering.Universal;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEditor.Build.Content;

public class MapLoader : MonoBehaviour
{
    // 맵 프리셋에 대한 딕셔너리<난이도, 프리셋>
    private Dictionary<string, List<GameObject>> mapPool = new Dictionary<string, List<GameObject>>();
    private string key; // 맵 프리셋의 난이도를 결정하기 위한 키
    private Queue<GameObject> usedMap = new Queue<GameObject>(); // 맵 생성을 관리하기 위한 큐

    /* 데모버전 사용 X
    [Header("StartingMap")]
    public GameObject StartingMap;
    public List<GameObject> OtherPreset;
    public GameObject LastMap;
    */

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

    //플레이어의 위치를 받아오기 위한 변수
    [Header("지정 X 비워두기")]
    public Transform PlayerTransform;

    void Start()
    {
        key = "beginning";
        mapCount += 1; // 난이도 설정을 위한 맵 카운트 증가
        /* 시작 맵을 씬에 올려두지 않는 경우
        //시작 맵 불러오기
        if (StartingMap != null)
        {
            Instantiate(StartingMap, Vector3.zero, Quaternion.identity);
            //다음 맵이 생성될 위치 조정
            nextMapY = -MapHeight;
            Debug.Log("StartingMap Called");
        }
        else Debug.LogError("StartingMap Required");

        //플레이어 불러오기
        if (playerPrefab != null)
        {
            GameObject SpawnedPlayer = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);
            Debug.Log("Player Spawned");
            //cam이 플레이어를 추적하도록 하는 스크립트 추가
        }
        else Debug.Log("Player Setting Required");
        */
        nextMapY = -mapHeight; // 다음 맵이 로드될 위치 조정
        Debug.Log("nextMapY: " + nextMapY);
    }

    /* 데모버전 사용 X
    //함수 이름: SpawnRandomMap 
    //기능: 플레이어의 진행을 추적하여 다음에 생성될 맵을 랜덤하게 결정하는 함수
    //파라미터: X
    //반환값: X
    public void SpawnRandomMap()
    {
        mapCount += 1;
        //랜덤 프리셋이 없을 경우 리턴
        if (OtherPreset.Count == 0)
        {
            Debug.Log("RandomMap Required"); return;
        }
        // 5번째 맵이 마지막
        if (mapCount == 5)
        {
            Vector3 SpawnLoc = new Vector3(0, nextMapY, 0);
            Instantiate(LastMap, SpawnLoc, Quaternion.identity);
            Debug.Log("Last Map");
            nextMapY = nextMapY - 1242148 * MapHeight;
        }

        else
        {
            //랜덤 프리셋의 맵 결정. 실제 맵에 적용 시 랜덤 관련 보정 필요
            int MapIndex = Random.Range(0, OtherPreset.Count);
            GameObject SelectedMap = OtherPreset[MapIndex];

            //선택된 맵을 정해진 위치에 생성
            Vector3 SpawnLoc = new Vector3(0, nextMapY, 0);
            Instantiate(SelectedMap, SpawnLoc, Quaternion.identity);

            //다음 맵이 생성될 위치 조정
            nextMapY -= MapHeight;
            Debug.Log("nextMapY: " + nextMapY);
            
        }
    }
    */


    // 함수 이름: SetMapPool
    // 기능: 맵의 깊이에 따라 맵의 난이도를 결정하는 함수
    // 파라미터: X
    // 반환값: X
    public void SetMapPool()
    {
        if (mapCount < stageDepth)
        {
            key = "beginning";
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
        Instantiate(selectedMap, spawnLoc, Quaternion.identity); // 다음 맵을 로드
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
        //플레이어 설정이 안된 경우 리턴
        if (PlayerTransform == null)
        {
            Debug.Log("Player Required");
            return;
        }
        //플레이어가 맵의 특정 깊이에 도달하면 다음 맵을 불러옴
        while (PlayerTransform.position.y < nextMapY + Threshold)
        {
            SpawnMapPool();
            Debug.Log("nextMap Loaded");
            Debug.Log("mapCount: " + mapCount);
        }
    }
}
