using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptArea : MonoBehaviour
{
	LayerMask m;
	BoxCollider2D box;
	Animator anim;
	private AudioSource audioSource;

	private AudioClip qClip;
	private AudioClip wClip;
	private AudioClip eClip;
	private AudioClip rClip;
	private AudioClip errorClip;

	// Start is called before the first frame update
	void Start()
    {
		box = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
        m = 1 << LayerMask.NameToLayer("Note");
    }

	public void Init(bool eating) {
		if (eating) {
			transform.localScale = new Vector3(1f, 1.75f, 0);
		}
		else {
			transform.localScale = Vector3.one * 1.75f;
		}
	}

	public void PlayNote(char s, bool correct) { 
		if(!correct) {
			errorClip = errorClip ?? Resources.Load<AudioClip>("Sounds/error");
			audioSource.clip = errorClip;
		} else if(s == 'Q') {
			qClip = qClip ?? Resources.Load<AudioClip>("Sounds/q");
			audioSource.clip = qClip;
		}
		else if(s == 'W') {
			wClip = wClip ?? Resources.Load<AudioClip>("Sounds/w");
			audioSource.clip = wClip;
		}
		else if(s == 'E') {
			eClip = eClip ?? Resources.Load<AudioClip>("Sounds/e");
			audioSource.clip = eClip;
		}
		else {
			rClip = rClip ?? Resources.Load<AudioClip>("Sounds/r");
			audioSource.clip = rClip;
		}

		audioSource.Play();
	}

	public int NoteMatch(InputPackage p) {
		char letter =
			p.Q ? 'Q' :
			p.W ? 'W' :
			p.E ? 'E' :
			p.R ? 'R' : default(char);
		if(letter == default(char)) {
			anim.SetInteger("Delta", 0);
			return 0;
		}

		PlayNote(letter, true);

		var noteHits = Physics2D.OverlapBox(transform.position, box.size * transform.localScale.x, 0, m);
		if(noteHits != null) {
			var note = noteHits.GetComponent<MusicNote>();
			if(note.Letter == letter) {
				note.Played();
				anim.SetInteger("Delta", 1);
				PlayNote(letter, true);
				return 1;
			}	
		}

		PlayNote(letter, false);
		anim.SetInteger("Delta", -1);
		return -1;
	}
}
