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
		string text = string.Format("{0:0.0}", t.CodeTaskAverage * 5);
		GameplayAverage.text = $"({text} average from {t.CodeTasksCompleted} ratings)";

		text = string.Format("{0:0.0}", t.ArtTaskAverage * 5);
		GraphicsAverage.text = $"({text} average from {t.ArtTasksCompleted} ratings)";

		text = string.Format("{0:0.0}", t.MusicTaskAverage * 5);
		AudioAverage.text = $"({text} average from {t.MusicTasksCompleted} ratings)";

		GameplayRanking.text = $"{Mathf.Lerp(999, 1, t.CodeTaskAverage)}th";
		GraphicsRanking.text = $"{Mathf.Lerp(999, 1, t.ArtTaskAverage)}th";
		AudioRanking.text = $"{Mathf.Lerp(999, 1, t.MusicTaskAverage)}th";
	}
}
