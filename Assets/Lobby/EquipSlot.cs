using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class EquipSlot : MonoBehaviour
{
    private EquipStats _equipStats;
    [Inject]
    public void Construct(EquipStats equipStats)
    {
        _equipStats = equipStats;
    }
    public EquipCategory eCategory;
}
