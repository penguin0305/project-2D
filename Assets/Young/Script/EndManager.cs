using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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

    public override void OnNetworkSpawn()
    {
        if (endingUI != null) endingUI.SetActive(false);
        StartCoroutine(EndingSequence());
    }

    IEnumerator EndingSequence()
    {
        GameObject[] playerObjects;
        while (true)
        {
            playerObjects = GameObject.FindGameObjectsWithTag("Player");
            if (playerObjects.Length > 0) break;
            yield return new WaitForSeconds(0.2f);
        }

        for (int i = 0; i < playerObjects.Length; i++)
        {
            var p = playerObjects[i];

            var inputState = p.GetComponent<PlayerInputState>();
            if (inputState != null) inputState.enabled = false;

            if (i < SpawnPoints.Length && SpawnPoints[i] != null)
            {
                p.transform.position = SpawnPoints[i].position;
            }
        }

        bool allReached = false;
        while (!allReached)
        {
            allReached = true;
            for (int i = 0; i < playerObjects.Length; i++)
            {
                if (i >= TargetPoints.Length || TargetPoints[i] == null) continue;

                var p = playerObjects[i];
                var motor = p.GetComponent<PlayerMotor>();
                if (motor == null) continue;

                var rb = p.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.simulated = true;
                    rb.isKinematic = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                float distanceX = TargetPoints[i].position.x - p.transform.position.x;

                if (Mathf.Abs(distanceX) > 0.1f)
                {
                    float moveDir = distanceX > 0 ? 1f : -1f;
                    motor.SetVelocityX(moveDir * Speed);
                    motor.UpdateFacingDirection(moveDir);
                    allReached = false;
                }
                else
                {
                    motor.StopHorizontal();
                }
            }
            yield return null;
        }

        if (endingUI != null) endingUI.SetActive(true);
    }

    public void ExitButton()
    {


        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        SceneManager.LoadScene(LoadSceneName);
    }
}