using UnityEngine;
using UnityEditor;
using SupanthaPaul;

public class EquipManager : MonoBehaviour
{
    private bool isActive;

    public GameObject EquipUI;
    
    void Start()
    {
        isActive = false;
        EquipUI.SetActive(false);
    }

    void Update()
    {
        if(InputSystem.Equip())
        {
            if (!isActive)
            {
                isActive = true;
                EquipUI.SetActive(isActive);
            }
            else
            {
                isActive = false;
                EquipUI.SetActive(isActive);
            }
        }
    }
}
