using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptArea : MonoBehaviour
{
	LayerMask m;
	BoxCollider2D box;
	Animator anim;

    // Start is called before the first frame update
    void Start()
    {
		box = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
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

		var noteHits = Physics2D.OverlapBox(transform.position, box.size * transform.localScale.x, 0, m);
		if(noteHits != null) {
			var note = noteHits.GetComponent<MusicNote>();
			if(note.Letter == letter) {
				note.Played();
				anim.SetInteger("Delta", 1);
				return 1;
			}	
		}
		
		anim.SetInteger("Delta", -1);
		return -1;
	}
}
