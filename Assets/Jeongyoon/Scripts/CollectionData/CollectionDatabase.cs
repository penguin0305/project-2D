using System.Collections.Generic;
using UnityEngine;

public class CollectionDatabase : MonoBehaviour
{
    public static CollectionDatabase Instance;

    public List<CollectionData> collectionList;

    private Dictionary<int, CollectionData> collectionDict;

    private void Awake()
    {
        Instance = this;

        collectionDict = new Dictionary<int, CollectionData>();

        foreach (var data in collectionList)
        {
            collectionDict[data.collectionId] = data;
        }
    }

    public CollectionData Get(int collectionId)
    {
        if (collectionDict.TryGetValue(collectionId, out var data))
            return data;

        Debug.LogError($"CollectionData 없음: {collectionId}");
        return null;
    }
}