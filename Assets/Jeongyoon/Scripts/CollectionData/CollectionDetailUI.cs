using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CollectionDetailUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text levelText;
    public TMP_Text expText;
    public TMP_Text needText;
    public TMP_Text failText;

    public Button enhanceButton;

    private int currentId;
    public Action<int> onEnhance;

    public void Show(CollectionData data, CollectionState state)
    {
        currentId = data.collectionId;

        icon.sprite = data.icon;
        nameText.text = data.collectionName;
        descriptionText.text = data.description;

        // 기존: (state.level > 0 || state.exp > 0)
        // dup_count(exp)가 0 초과면 보유
        bool isOwned = (state.level > 0 || state.exp > 0);

        if (!isOwned)
        {
            levelText.text = "Not Owned";
            expText.text = "Owned: 0";
            needText.text = "-";
            failText.text = "-";
            enhanceButton.interactable = false;
        }
        else
        {
            levelText.text = $"Lv. {state.level}";
            expText.text = $"Owned: {state.exp}";

            int need = GetNeedExp(state.level);
            needText.text = $"Need: {need}";

            int pity = GetPity(state.level);
            failText.text = $"Fail: {state.failCount} / {pity}";

            // 기존: state.exp >= need && state.level < 10
            // 0레벨도 강화 가능
            enhanceButton.interactable = state.exp >= need && state.level < 10;
        }

        enhanceButton.onClick.RemoveAllListeners();
        enhanceButton.onClick.AddListener(() =>
        {
            onEnhance?.Invoke(currentId);
        });
    }

    int GetNeedExp(int level)
    {
        switch (level)
        {
            case 0: return 1;
            case 1: return 1;
            case 2: return 1;
            case 3: return 1;
            case 4: return 2;
            case 5: return 2;
            case 6: return 3;
            case 7: return 3;
            case 8: return 4;
            case 9: return 5;
            default: return 0;
        }
    }

    int GetPity(int level)
    {
        switch (level)
        {
            case 0: return 0;
            case 2: return 3;
            case 3: return 4;
            case 4: return 5;
            case 5: return 6;
            case 6: return 8;
            case 7: return 10;
            case 8: return 12;
            case 9: return 15;
            default: return 0;
        }
    }
}