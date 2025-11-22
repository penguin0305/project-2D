using UnityEngine;

public class DummySwitch : MonoBehaviour, IInteractable
{
    private SpriteRenderer sr;
    private bool isRed = true;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
    }

    public void Interact(PlayerInteraction player)
    {
        if (isRed)
        {
            sr.color = Color.blue;
            isRed = false;
        }
        else
        {
            sr.color = Color.red;
            isRed = true;
        }
    }
}