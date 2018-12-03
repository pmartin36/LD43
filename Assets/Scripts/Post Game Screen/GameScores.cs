using UnityEngine;
using System.Collections;
using TMPro;

public class GameScores : MonoBehaviour {

	public TMP_Text GameplayRanking;
	public TMP_Text GraphicsRanking;
	public TMP_Text AudioRanking;

	public TMP_Text GameplayAverage;
	public TMP_Text GraphicsAverage;
	public TMP_Text AudioAverage;

	public void Init(CompletedTaskTracker t) {
		float val = t.CodeTaskAverage * (t.CodeTasksCompleted / 10f);
		string text = string.Format("{0:0.0}", val * 5);
		GameplayAverage.text = $"({text} average from {t.CodeTasksCompleted} ratings)";
		int place = (int)Mathf.Lerp(999, 1, val);
		GameplayRanking.text = $"{place}{GetSuffix(place)}";

		val = t.ArtTaskAverage * (t.ArtTasksCompleted / 10f);
		text = string.Format("{0:0.0}", val * 5);
		GraphicsAverage.text = $"({text} average from {t.ArtTasksCompleted} ratings)";
		place = (int)Mathf.Lerp(999, 1, val);
		GraphicsRanking.text = $"{place}{GetSuffix(place)}";

		val = t.ArtTaskAverage * (t.MusicTasksCompleted / 10f);
		text = string.Format("{0:0.0}", val * 5);
		AudioAverage.text = $"({text} average from {t.MusicTasksCompleted} ratings)";
		place = (int)Mathf.Lerp(999, 1, val);
		AudioRanking.text = $"{place}{GetSuffix(place)}";
	}

	public string GetSuffix(int val) {
		var s = val.ToString();
		char c = s[s.Length-1];

		if(c == '1') {
			return "st";
		}
		else if(c == '2') {
			return "nd";
		}
		else if(c == '3') {
			return "rd";
		}
		else {
			return "th";
		}
	}
}
