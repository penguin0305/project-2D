using NUnit.Framework;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

// IInteractable 인터페이스를 상속받습니다.
public class EndPortal : MonoBehaviour, IInteractable
{
    [SerializeField] public Sprite OnPortal;
    [SerializeField] public Sprite OffPortal;
    [SerializeField] public string EndingSceneName= "EndScene1217";
    private StageManager _stageManager;
    [Inject]
    public void Construct(StageManager stageManager)
    {
        _stageManager = stageManager;
        _stageManager.OnStageClear += ActivatePortal;
    }

    private SpriteRenderer spriteRenderer;
    private bool isActivated;
    /*
    public void Initialize()
    {
        if (_stageManager != null)
        {
            _stageManager.OnStageClear += ActivatePortal;
        }
    }*/

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //_stageManager.OnStageClear += ActivatePortal;
    }

    private void Start()
    {
        DeactivatePortal();
    }

    private void DeactivatePortal()
    {
        isActivated = false;

        spriteRenderer.sprite = OffPortal;

    }

    public void ActivatePortal(List<itemData> tmp)
    {
        isActivated = true;

        spriteRenderer.sprite = OnPortal;
    }
    public void Interact(PlayerInteraction player)
    {
        if (isActivated)
        {
            Debug.Log("InteractPortal");
            ToEnding();
        }
        else
        {
            Debug.Log("DeactivatePortal");
        }
    }
    private void ToEnding()
    {
        SceneManager.LoadScene(EndingSceneName);
    }

    private void OnDisable()
    {
        _stageManager.OnStageClear -= ActivatePortal;
    }
}


