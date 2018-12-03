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
	private bool StopChecking = false;

	public void Start() {
		RemainingTimer = GameManager.Instance.RemainingTimer;
	}

	public void LateUpdate() {
		if(!StopChecking) {
			if(RemainingTimer.Value <= 0) {
				Color c = new Color(0.84f, 0.35f, 0.3f);
				var texts = GetComponentsInChildren<TMP_Text>();
				foreach(var t in texts) {
					if(t.tag != "TimeRemainingHeader") {
						t.color = c;
					}
				}
				StopChecking = true;
			}
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
}
