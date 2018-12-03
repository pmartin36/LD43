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
	private AudioSource audioSource;

    void Start()
    {
        var gm = GameManager.Instance;
		anim = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		// anim.Play("IntroSkip");
		if (gm.SkipIntro) {
			anim.Play("IntroSkip");
		}
		else {
			anim.Play("IntroShow");
		}
		gm.SkipIntro = true;
    }

	public void PlayThwack() {
		audioSource.Play();
	}

    public void StartJamming() {
		anim.SetBool("Hiding", true);
	}

	public void Uncover() {
		GameManager.Instance.BeginJam();
		Destroy(this.gameObject);
	}
}
