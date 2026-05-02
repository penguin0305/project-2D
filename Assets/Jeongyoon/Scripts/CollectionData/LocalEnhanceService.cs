using UnityEngine;

public class LocalEnhanceService : IEnhanceService
{
    private const int MAX_LEVEL = 10;

    public void Enhance(int collectionId, CollectionState state, System.Action<CollectionState> onComplete)
    {
        if (state.level >= MAX_LEVEL)
            return;

        int need = GetNeedExp(state.level);

        if (state.exp < need)
            return;

        state.exp -= need;

        int pity = GetPity(state.level);

        bool success = (state.failCount >= pity) ||
                       (Random.value < GetSuccessRate(state.level));

        if (success)
        {
            state.level++;
            state.failCount = 0;
        }
        else
        {
            state.failCount++;
        }

        onComplete?.Invoke(state);
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

    float GetSuccessRate(int level)
    {
        switch (level)
        {
            case 0: return 1f;
            case 1: return 1f;
            case 2: return 0.8f;
            case 3: return 0.6f;
            case 4: return 0.5f;
            case 5: return 0.4f;
            case 6: return 0.3f;
            case 7: return 0.2f;
            case 8: return 0.15f;
            case 9: return 0.1f;
            default: return 0f;
        }
    }
}