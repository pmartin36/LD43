using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuitScreen : MonoBehaviour
{
	public GameObject Body;
	public TMP_Text ReasonField;

    void Start() {
        GameManager.Instance.QuitScreen = this;
    }

    public void Show(string reason) {
		Body.SetActive(true);
		ReasonField.text = $"You stopped creating your game {reason}";
	}

	public void Restart() {
		Body.SetActive(false);
		GameManager.Instance.ReloadLevel();
	}

	public void Quit() {
		GameManager.Instance.Exit();
	}
}
