using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatusBarUI : MonoBehaviour
{
	[SerializeField]
	private Slider BoredomSlider;
	[SerializeField]
	private Slider SleepinessSlider;
	[SerializeField]
	private Slider AnxietySlider;
	[SerializeField]
	private Slider HungerSlider;

	public Status Status;

	public void Start() {
		Status = GameManager.Instance.Status;
	}

	// Update is called once per frame
	void Update()
    {
        BoredomSlider.value = Status.Boredom;
		SleepinessSlider.value = Status.Sleepiness;
		AnxietySlider.value = Status.Anxiety;
		HungerSlider.value = Status.Hunger;
    }
}
