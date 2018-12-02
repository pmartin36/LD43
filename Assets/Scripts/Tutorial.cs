using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Tutorial : MonoBehaviour
{
	public TMP_Text Header;
	public TMP_Text Description;
	Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Show() {
		anim = anim ?? GetComponent<Animator>();
		anim.Play("ShowTutorial");
	}
}
