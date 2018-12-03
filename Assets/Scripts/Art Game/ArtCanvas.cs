using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ArtCanvas : MonoBehaviour {

	[SerializeField]
	private Button submitButton;
	private TMP_Text submitButtonText;

	[SerializeField]
	private TMP_Text EatingHelpText;

	private Coroutine EatHelp;

	public void SetSubmitButtonActive(bool active) {
		submitButtonText = submitButtonText ?? submitButton.GetComponentInChildren<TMP_Text>();
		submitButton.interactable = active;

		submitButtonText.color = active ? Color.white : new Color(0.25f, 0.25f, 0.25f);
	}

	public void ShowUndoInvalid() {
		if(EatHelp != null) {
			StopCoroutine(EatHelp);
		}
		EatHelp = StartCoroutine(ShowEatHelp());
	}

	private IEnumerator ShowEatHelp() {
		Color color = new Color(1, 0.41f, 0.41f);
		EatingHelpText.color = color;
		yield return new WaitForSeconds(0.5f);
		float t = 0;
		while(t < 1.1f) {
			EatingHelpText.color = Color.Lerp(color, Color.clear, t);
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}
