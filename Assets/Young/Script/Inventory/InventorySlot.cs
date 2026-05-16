using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public itemData item;
    public Image Image;
    public TextMeshProUGUI amountText;

    public void SetItem(itemData newitemData, int amount)
    {
        item = newitemData;

        Image.sprite = newitemData.collectionInfo.icon;
        Image.enabled = true;

        if (amount >= 1)
        {
            amountText.text = amount.ToString();
        }

        else
        {
            amountText.text = "";
        }

    }
    public void DeleteSlot()
    {
        Image.sprite = null;
        Image.enabled = false;
        amountText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            // ���� ���̱�
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ���� �����
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // ��� ������ ����
            }
        }
    }
}

