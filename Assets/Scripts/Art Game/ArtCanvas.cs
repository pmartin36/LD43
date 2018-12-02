using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ArtCanvas : MonoBehaviour {

	[SerializeField]
	private Button submitButton;
	[SerializeField]
	private TMP_Text EatingHelpText;

	public void SetSubmitButtonActive(bool active) {
		submitButton.interactable = active;
	}
}
