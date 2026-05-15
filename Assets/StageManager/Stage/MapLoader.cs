using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;

public class MapLoader : NetworkBehaviour
{
    // �� �����¿� ���� ��ųʸ�<���̵�, ������>
    private Dictionary<string, List<GameObject>> mapPool = new Dictionary<string, List<GameObject>>();
    private string key; // �� �������� ���̵��� �����ϱ� ���� Ű
    private Queue<GameObject> usedMap = new Queue<GameObject>(); // �� ������ �����ϱ� ���� ť
    

    //�� ������ ���� ������ ����Ʈ ����
    [Header("Pooling Candidate")]
    public List<GameObject> BeginningPreset;
    public List<GameObject> FirstHPreset;
    public List<GameObject> SecondHPreset;
    public List<GameObject> EndingPreset;

    //�� ���� ��ġ ������ ���� ���� ũ��� ���� �� ���� ��ġ����. Y�ุ ����. X�� �ʿ� �� �߰�(��������)
    private float mapHeight = 16f; //���� �� ũ�⿡ ���� ���� �ʿ�. 1ĭ = 1f
    private float nextMapY = 0f; // ������ �ҷ��� ���� ��ġ����
    private float Threshold = 14f; // �� ���� Ÿ�̹� ������ ���� �Ӱ谪 ����
    private int mapCount = 0; // ������� ���� ��. ���̵� ������ ���
    private int stageDepth = 5;

    private IObjectResolver _objectResolver;

    [Inject]
    public void Construct(IObjectResolver objectResolver)
    {
        _objectResolver = objectResolver;
    }

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return; // ���������� �� �ε� ����
        key = "beginning";
        mapCount += 1; // ���̵� ������ ���� �� ī��Ʈ ����
        nextMapY = -mapHeight; // ���� ���� �ε�� ��ġ ����
        Debug.Log("nextMapY: " + nextMapY);
    }


    // �Լ� �̸�: SetMapPool
    // ���: ���� ���̿� ���� ���� ���̵��� �����ϴ� �Լ�
    // �Ķ����: X
    // ��ȯ��: X
    public void SetMapPool()
    {
        if (mapCount < stageDepth)
        {
            mapPool[key] = BeginningPreset; // �ʹݺδ� BeginningPreset���� �� ����
        }
        /*
        else if (mapCount < stageDepth * 2)
        {
            key = "firstH";
            mapPool[key] = FirstHPreset; // ���ݺδ� FirstHPreset���� ����
        }
        else if (mapCount < stageDepth * 3)
        {
            key = "secondH";
            mapPool[key] = SecondHPreset; // �Ĺݺδ� SecondHPreset���� ����
        }
        */
        else
        {
            key = "ending";
            mapPool[key] = EndingPreset; // ������ ������ ���� ���������� EndingPreset���� ����
        }
    }

    // �Լ� �̸�: GetMap
    // ���: ������ ������ �ߺ����� �ʰ� ������ ���� �����ϴ� �Լ�
    // �Ķ����: X
    // ��ȯ��: �ҷ������ ������ �� ������
    public GameObject GetMap()
    {
        bool isUsed = false; // ���� ������ Ȯ���ϱ� ���� ����

        while(true)
        {

            //�����ϰ� ���� ����
            int mapIndex = Random.Range(0, mapPool[key].Count);
            GameObject SelectedMap = mapPool[key][mapIndex];

            
            foreach (GameObject used in usedMap)
            {
                if (SelectedMap == used)
                    isUsed = true;
            }

            //���� ���� �ƴ϶�� usedMap�� �ְ� ����
            if(!isUsed)
            {
                usedMap.Enqueue(SelectedMap);
                return SelectedMap;
            }

            isUsed = false;
        }
    }

    // �Լ� �̸�: SpawnMapPool
    // ���: ���������� ���� ���� �ҷ����� ����� �ϴ� �Լ�
    // �Ķ����: X
    // ��ȯ��: X
    public void SpawnMapPool()
    {
        mapCount += 1; // ���̵� ������ ���� �� ī��Ʈ ����
        SetMapPool(); // ���̵� ����
        GameObject selectedMap = GetMap(); // �ҷ��� ���� ����
        Vector3 spawnLoc = new Vector3(0, nextMapY, 0); // ������ ���� �ε��� ��ġ ����
        GameObject map = Instantiate(selectedMap, spawnLoc, Quaternion.identity); // ���� ���� �ε�
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
            Debug.LogError("�� �����տ� ��Ʈ��ũ ������Ʈ �ʿ�");
        }

        Debug.Log("nextMap Loaded");

        // ������ ���������� �ƴ϶�� ���� ���� ��ġ�� �缳��
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
        
        // 버그 안전장치
        if (NetworkManager.Singleton == null || NetworkManager.Singleton.ConnectedClientsList == null) return;

        float playerLoc = float.MaxValue; // �÷��̾� ��ġ�� ��� ���� �ʱ�ȭ

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            // �÷��̾� ������Ʈ�� �����ϴ��� Ȯ��
            if (client.PlayerObject != null)
            {
                float playerY = client.PlayerObject.transform.position.y;
                if (playerY < playerLoc)
                {
                    playerLoc = playerY;
                }
            }
        }

        // �÷��̾ ���� ���(��ġ�� ������) ����
        if (playerLoc == float.MaxValue) return;

        // ���� �Ʒ��� �ִ� �÷��̾ ���� Ư�� ���̿� �����ϸ� ���� ���� �ҷ���
        while (playerLoc < nextMapY + Threshold)
        {
            SpawnMapPool();
            Debug.Log("mapCount: " + mapCount);
        }
    }
}
