using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAreaMenu : MonoBehaviour
{
	public Image Cover;
	private Animator anim;

	[SerializeField]
	private GameObject DowntimeBox;
	private bool firstTime = true;

	public bool Transitioning;
	public Task TransitioningTask;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
		GameManager.Instance.PlayAreaMenu = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void TaskSelected(string task) {
		Enum.TryParse(task, out TransitioningTask);
		anim.Play("Cover");	
	}

	public void ReliefActivitySelected(string s) {
		Enum.TryParse(s, out ReliefActivity activity);
		GameManager.Instance.BeginReliefActivity(activity);
	}

	public void SetCurrentTask() {
		Transitioning = false;
		GameManager.Instance.SetCurrentTask(TransitioningTask);
	}

	public void Open() {
		Transitioning = true;
		if(firstTime) {
			DowntimeBox.SetActive(false);
		}
		else {
			DowntimeBox.SetActive(true);
		}
		firstTime = false;
		anim.Play("Uncover");
	}
}
