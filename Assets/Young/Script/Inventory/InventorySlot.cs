using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI amountText;

    public void SetItem(Sprite sprite, int amount)
    {
        Image.sprite = sprite;
        Image.enabled = true;

        if (amount > 1)
        {
            amountText.text = amount.ToString();
        }

        else
        {
            amountText.text = "";
        }

    }
}