using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private EndPortal endPortal;
    [SerializeField] private GameObject Target;

    public void CheckCondition(GameObject deadEnemy)
    {
        if (deadEnemy == Target)
        {
                endPortal.ActivatePortal();
        }
    }
}