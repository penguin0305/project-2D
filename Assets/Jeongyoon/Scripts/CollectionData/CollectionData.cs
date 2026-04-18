using UnityEngine;

[CreateAssetMenu(fileName = "CollectionData", menuName = "Collection/CollectionData")]
public class CollectionData : ScriptableObject
{
	public int collectionId;
	public string collectionName;
	public Sprite icon;
	[TextArea]
	public string description;
}