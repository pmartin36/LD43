using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PostGameScreen : MonoBehaviour
{
	public TMP_Text FinalScoreField;
	public MentalHealthScores MentalHealth;
	public GameScores GameScore;

	public void Init(Status status, CompletedTaskTracker tasks) {
		float subtraction = status.Anxiety + status.Boredom + status.Hunger + status.Sleepiness;
		float addition = (tasks.ArtTaskAverage + tasks.CodeTaskAverage + tasks.MusicTaskAverage) * 5 / 3f;

		float finalScore = addition - subtraction;
		FinalScoreField.text = string.Format("{0:0.0}", finalScore);

		MentalHealth.Init(status);
		GameScore.Init(tasks);
	}
}
