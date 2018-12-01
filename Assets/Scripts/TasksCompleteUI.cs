using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TasksCompleteUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text CodeField;
	[SerializeField]
	private TMP_Text MusicField;
	[SerializeField]
	private TMP_Text ArtField;

	// Start is called before the first frame update
	void Start()
    {
        CompletedTaskTracker.TaskCompleted += TaskCompleted;
    }

    public void TaskCompleted(object sender, TaskCompleteEventArgs args) {
		switch (args.CompletedTask) {
			case Task.Art:
				ArtField.text = args.CurrentCompleted.ToString();
				break;
			case Task.Code:
				CodeField.text = args.CurrentCompleted.ToString();
				break;
			case Task.Music:
				MusicField.text = args.CurrentCompleted.ToString();
				break;
		}
	}
}