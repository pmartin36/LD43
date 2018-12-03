using UnityEngine;
using System.Collections;
using TMPro;

public class MentalHealthScores : MonoBehaviour {

	public TMP_Text Bored;
	public TMP_Text Anxiety;
	public TMP_Text Hunger;
	public TMP_Text Sleep;

	public void Init(Status s) {
		int val = (int)(s.Boredom * 100);
		Bored.text = val.ToString();

		val = (int)(s.Anxiety * 100);
		Anxiety.text = val.ToString();

		val = (int)(s.Hunger * 100);
		Hunger.text = val.ToString();

		val = (int)(s.Sleepiness * 100);
		Sleep.text = val.ToString();
	}
}
