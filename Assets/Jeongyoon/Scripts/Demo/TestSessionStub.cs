using System.Collections.Generic;
using UnityEngine;

public class TestSessionStub : MonoBehaviour
{
	[System.Serializable]
	public class TestItem
	{
		[Range(0, 16)]
		public int eid;
		[Range(0, 10)]
		public int enhance_level;
		[Range(0, 200)]
		public int dup_count;
		[Range(0, 15)]
		public int enhance_fail_count;
	}

	[Header("Test Items")]
	public List<TestItem> testItems = new List<TestItem>();

	private void Start()
	{
		InjectData();
	}

	private void InjectData()
	{
		if (PlayerSession.Instance == null)
		{
			Debug.LogWarning("PlayerSession.Instance is null. Cannot inject test data.");
			return;
		}

		foreach (var item in testItems)
		{
			PlayerSession.Instance.UpdateItem(
				item.eid,
				item.enhance_level,
				item.dup_count,
				item.enhance_fail_count
			);
		}

		Debug.Log($"[TestSessionStub] {testItems.Count}개 아이템 주입 완료");
	}
}