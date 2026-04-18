using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class TestScript : MonoBehaviour
{
    void Start()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host success");

            CreateDummySession();
        }
        else
        {
            Debug.LogError("Host failed");
        }
    }

    void CreateDummySession()
    {
        if (PlayerSession.Instance == null)
        {
            Debug.LogError("PlayerSession not found in scene");
            return;
        }

        PlayerData data = new PlayerData
        {
            id = "test_user",
            username = "Tester",
            level = 1,
            exp = 0,
            currency = 1000,

            playeritems = new List<PlayerItem>
            {
                new PlayerItem
                {
                    id = "",
                    iid = 0,
                    eid = 0,
                    type = "",
                    dup_count = 50,
                    enhance_level = 1,
                    enhance_fail_count = 0,
                    base_atk = 0,
                    base_hp = 0,
                    base_armor = 0
                },

                new PlayerItem
                {
                    id = "",
                    iid = 0,
                    eid = 1,
                    type = "",
                    dup_count = 30,
                    enhance_level = 2,
                    enhance_fail_count = 1,
                    base_atk = 0,
                    base_hp = 0,
                    base_armor = 0
                },

                new PlayerItem
                {
                    id = "",
                    iid = 0,
                    eid = 7,
                    type = "",
                    dup_count = 10,
                    enhance_level = 1,
                    enhance_fail_count = 0,
                    base_atk = 0,
                    base_hp = 0,
                    base_armor = 0
                }
            }
        };

        PlayerSession.Instance.UpdateSessionData(data);

        Debug.Log("Dummy PlayerSession initialized");
    }
}