using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingDirector : MonoBehaviour
{
    [Header("Positions")]
    public Transform spawnPoint;
    public Transform TargetPoint;

    [Header("UI")]
    public GameObject endingUI;

    [Header("Speed")]
    public float Speed = 0.5f;

    private GameObject playerInstance;
    private PlayerMovement movement;

    void Start()
    {
        if (endingUI != null) endingUI.SetActive(false);
        StartCoroutine(EndingSequence());
    }

    IEnumerator EndingSequence()
    {
        if (GameSceneManager.Instance != null && GameSceneManager.Instance.playerPrefab != null)
        {
            playerInstance = Instantiate(GameSceneManager.Instance.playerPrefab, spawnPoint.position, Quaternion.identity);
        }

        var inputHandler = playerInstance.GetComponent<PlayerInputHandler>();
        movement = playerInstance.GetComponent<PlayerMovement>();

        if (inputHandler != null)
        {
            inputHandler.enabled = false;
        }

        Vector2 Moved = new Vector2(Speed, 0);

        while (Mathf.Abs(playerInstance.transform.position.x - TargetPoint.position.x) > 0.1f)
        {
            if (movement != null)
            {
                movement.RequestMove(Moved);
            }
            yield return null;
        }

        if (movement != null)
        {
            movement.RequestMove(Vector2.zero);
        }

        if (endingUI != null) endingUI.SetActive(true);
    }

    public void GoToStartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}