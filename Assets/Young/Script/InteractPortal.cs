using UnityEngine;
using UnityEngine.SceneManagement;

// IInteractable 인터페이스를 상속받습니다.
public class EndPortal : MonoBehaviour, IInteractable
{
    public string EndingSceneName = "EndScene";

    public void Interact(PlayerInteraction interactor)
    {
        Debug.Log("InteractPortal");
        ToEnding();
    }

    private void ToEnding()
    {
        SceneManager.LoadScene(EndingSceneName);
    }
}