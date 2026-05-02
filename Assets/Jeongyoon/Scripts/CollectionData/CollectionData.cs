using UnityEngine;

[CreateAssetMenu(fileName = "CollectionData", menuName = "Collection/CollectionData")]
public class CollectionData : ScriptableObject
{
	public int collectionId;
	public string collectionName;
	public Sprite icon;
	[TextArea]
	public string description;

	[Header("Stat Bonus Per Level")]
	public float bonusMaxHP;
	public float bonusMeleeATK;
	public float bonusRangeATK;
	public float bonusArmor;
	public float bonusSpeed;
	public float bonusCritRate;
	public float bonusCritDamage;
}