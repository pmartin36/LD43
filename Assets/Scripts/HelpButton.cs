using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HelpButton : MonoBehaviour
{
	[SerializeField]
	private GameObject Popup;

	public void OnHoverEnter() {
		Popup.SetActive(true);
	}

	public void OnHoverExit() {
		Popup.SetActive(false);
	}
}