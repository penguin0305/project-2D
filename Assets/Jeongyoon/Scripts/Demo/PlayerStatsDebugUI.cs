using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsDebugUI : MonoBehaviour
{
	private List<Player> players = new List<Player>();

	private GUIStyle headerStyle;
	private GUIStyle statStyle;

	private void Awake()
	{
		headerStyle = new GUIStyle();
		headerStyle.fontSize = 18;
		headerStyle.fontStyle = FontStyle.Bold;
		headerStyle.normal.textColor = Color.yellow;

		statStyle = new GUIStyle();
		statStyle.fontSize = 16;
		statStyle.normal.textColor = Color.white;
	}

	private void Update()
	{
		players.Clear();
		players.AddRange(FindObjectsByType<Player>(FindObjectsSortMode.None));
	}

	private void OnGUI()
	{
		float x = 20f;
		float y = 20f;
		float lineHeight = 24f;
		float columnWidth = 220f;

		for (int i = 0; i < players.Count; i++)
		{
			var p = players[i];
			if (p == null || p.Status == null) continue;

			float col = x + i * columnWidth;
			float row = y;

			string label = p.IsOwner ? "[나]" : $"[플레이어 {i + 1}]";
			GUI.Label(new Rect(col, row, columnWidth, 26), label, headerStyle);
			row += lineHeight + 4f;

			var s = p.Status;
			GUI.Label(new Rect(col, row, columnWidth, 24), $"HP:       {s.CurrentHealth} / {s.MaxHealth}", statStyle);
			row += lineHeight;
			GUI.Label(new Rect(col, row, columnWidth, 24), $"MeleeATK: {s.MeleeATK}", statStyle);
			row += lineHeight;
			GUI.Label(new Rect(col, row, columnWidth, 24), $"RangeATK: {s.RangeATK}", statStyle);
			row += lineHeight;
			GUI.Label(new Rect(col, row, columnWidth, 24), $"Armor:    {s.Armor}", statStyle);
			row += lineHeight;
			GUI.Label(new Rect(col, row, columnWidth, 24), $"Speed:    {s.Speed}", statStyle);
		}
	}
}