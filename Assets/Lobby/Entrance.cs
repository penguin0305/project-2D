using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour, IInteractable
{
    [SerializeField] private string MainSceneName = "tScene";
    
    public void Interact(PlayerInteraction player)
    {
        Enter();
    }

    private void Enter()
    {
        SceneManager.LoadScene(MainSceneName);
    }
}


