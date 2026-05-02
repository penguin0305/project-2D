using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EndingDirector : NetworkBehaviour
{
    [Header("Positions")]
    public Transform[] SpawnPoints;
    public Transform[] TargetPoints;

    [Header("UI")]
    public GameObject endingUI;

    [Header("Speed")]
    public float Speed = 0.5f;

    [Header("LoadScene")]
    public string LoadSceneName;

    private List<GameObject> players = new List<GameObject>();
    
    public override void OnNetworkSpawn()
    {
        if (endingUI != null) endingUI.SetActive(false);
        StartCoroutine(EndingSequence());
    }

    IEnumerator EndingSequence()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        bool allReached = false;

        for (int i = 0; i < playerObjects.Length; i++)
        {
            var p = playerObjects[i];
            players.Add(p);

            if (i < SpawnPoints.Length)
                p.transform.position = SpawnPoints[i].position;

            var inputHandler = p.GetComponent<PlayerInputState>();
            if (inputHandler != null) inputHandler.enabled = false;
        }
                while (!allReached)
        {
            allReached = true;
            for (int i = 0; i < playerObjects.Length; i++)
            {
                if (i >= TargetPoints.Length) continue;

                var p = playerObjects[i];
                var movement = p.GetComponent<PlayerMovement>();
                
                float dist = Mathf.Abs(p.transform.position.x - TargetPoints[i].position.x);
                if (dist > 0.1f)
                {
                    Vector2 dir = new Vector2(Speed, 0);
                    movement.RequestMove(dir);
                    allReached = false;
                }
                else
                {
                    movement.RequestMove(Vector2.zero);
                }
            }
            yield return null;
        }

        endingUI.SetActive(true);
    }


    public void ExitButton()
    {
        if (InventoryManager.Instance != null)
        {
            NetworkInventoryManager.Instance.SendInventoryToSession();
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown(); 
        }
        
        Time.timeScale = 1f;

        SceneManager.LoadScene(LoadSceneName);
    }
}