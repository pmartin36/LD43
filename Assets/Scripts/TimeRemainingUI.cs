using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeRemainingUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text HoursTextField;
	[SerializeField]
	private TMP_Text MinutesTextField;
	[SerializeField]
	private TMP_Text SecondsTextField;

	private Timer RemainingTimer;

	public void Start() {
		RemainingTimer = GameManager.Instance.RemainingTimer;
	}

	public void LateUpdate() {
		float timeRemaining = RemainingTimer.Value;

		int hours = Mathf.FloorToInt(timeRemaining / 3600f);
		timeRemaining -= hours * 3600;

		int minutes = Mathf.FloorToInt(timeRemaining / 60f);
		timeRemaining -= minutes * 60;

		float seconds = timeRemaining;

		HoursTextField.text = hours.ToString("00");
		MinutesTextField.text = minutes.ToString("00");
		SecondsTextField.text = seconds.ToString("00");
	}
}
