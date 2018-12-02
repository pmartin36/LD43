using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScreen : MonoBehaviour
{
	[SerializeField]
	public TMP_Text Header;
	[SerializeField]
	public TMP_Text Score;

	[SerializeField]
	private GameObject Body;

    public void Show(Task t, float score) {
		Header.text = $"New {t.ToString()} Asset Created";
		Score.text = $"{Mathf.Round(score*100)}%";

		Body.SetActive(true);
	}

	public void Restart() {
		Body.SetActive(false);
		GameManager.Instance.ContextManager.Restart();
	}

	public void NewTask() {
		Body.SetActive(false);
		GameManager.Instance.ReturnToHub();
	}
}
