using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CollectionSlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;

    private int collectionId;
    private Action<int> onClick;

    public void Init(int id)
    {
        collectionId = id;

        var data = CollectionDatabase.Instance.Get(id);
        var state = CollectionUIManager.Instance.GetState(id);

        nameText.text = data.collectionName;
        icon.sprite = data.icon;

        bool isOwned = (state.level > 0 || state.exp > 0);

        icon.color = isOwned ? Color.white : Color.gray;
        nameText.color = Color.black;
    }

    public void SetClickAction(Action<int> action)
    {
        onClick = action;
    }

    public void OnClick()
    {
        onClick?.Invoke(collectionId);
    }
}