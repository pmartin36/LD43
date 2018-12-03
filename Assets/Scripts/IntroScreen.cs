using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
	public TMP_Text TimeFor;
	public Image LdLogo;
	public TMP_Text ThemeIs;
	public TMP_Text Sacrifices;
	public TMP_Text CompleteAsMany;
	public TMP_Text MentalHealth;
	public Button Button;

	private Animator anim;

    void Start()
    {
        var gm = GameManager.Instance;
		anim = GetComponent<Animator>();
		if(gm.SkipIntro) {
			TimeFor.gameObject.SetActive(true);
			LdLogo.gameObject.SetActive(true);
			ThemeIs.gameObject.SetActive(true);
			Sacrifices.gameObject.SetActive(true);
			CompleteAsMany.gameObject.SetActive(true);
			MentalHealth.gameObject.SetActive(true);
			Button.gameObject.SetActive(true);
		}
		else {
			anim.Play("IntroShow");
		}
		gm.SkipIntro = true;
    }

    public void StartJamming() {
		anim.SetBool("Hiding", true);
	}

	public void Uncover() {
		GameManager.Instance.RemainingTimer.Paused = false;
		GameManager.Instance.ReturnToHub();
	}
}
