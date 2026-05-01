using System.Collections.Generic;

public class BonusStatResult
{
	public int maxHP;
	public float meleeATK;
	public float rangeATK;
	public float armor;
	public float speed;
	public float critRate;
	public float critDamage;
}

public static class BonusStatCalculator
{
	public static BonusStatResult Calculate(PlayerItemNetwork[] items)
	{
		var result = new BonusStatResult();

		if (items == null)
			return result;

		foreach (var item in items)
		{
			var data = CollectionDatabase.Instance.Get(item.eid);
			if (data == null) continue;

			int level = item.enhance_level;

			result.maxHP      += (int)(data.bonusMaxHP * level);
			result.meleeATK   += data.bonusMeleeATK   * level;
			result.rangeATK   += data.bonusRangeATK   * level;
			result.armor      += data.bonusArmor      * level;
			result.speed      += data.bonusSpeed      * level;
			result.critRate   += data.bonusCritRate   * level;
			result.critDamage += data.bonusCritDamage * level;
		}

		return result;
	}
}