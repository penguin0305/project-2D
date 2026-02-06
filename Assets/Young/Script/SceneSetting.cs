using UnityEditor;
using UnityEngine;
using VContainer; // DI 프레임워크
using VContainer.Unity;
// using SupanthaPaul; // CameraFollow 스크립트가 있는 네임스페이스 (필요시 주석 해제)

public class SceneSetup : MonoBehaviour
{
    private GameObject playerInstance;
    public Transform spawnPoint;

    //플레이어 위치 정보를 VContainer로 관리하도록 수정
    private PlayerProvider _playerProvider;
    private IObjectResolver _objectResolver;
    private GameSceneManager _playerSet;
    [Inject]
    public void Construct(PlayerProvider playerProvider, IObjectResolver objectResolver, GameSceneManager playerSet)
    {
        _playerProvider = playerProvider;
        _objectResolver = objectResolver;
        _playerSet = playerSet;
    }

    public void SpawnPlayer()
    {
        playerInstance = _objectResolver.Instantiate(_playerSet.playerPrefab, spawnPoint.position, Quaternion.identity);
    }

    void Start()
    {
        SpawnPlayer();
        _playerProvider.playerTransform = playerInstance.transform;
    }
}