using UnityEngine;
using UnityEngine.SceneManagement;

// IInteractable 인터페이스를 상속받습니다.
public class EndPortal : MonoBehaviour, IInteractable
{
    [SerializeField] public Sprite OnPortal;
    [SerializeField] public Sprite OffPortal;
    [SerializeField] public string EndingSceneName = "EndScene";

    private SpriteRenderer spriteRenderer;
    private bool isActivated;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void ActivatePortal()
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
}


