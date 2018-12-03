using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskChangeListener : MonoBehaviour
{
	public Task ListeningForTask;
	[SerializeField]
	private ContextManager ContextToWake;

	private void Start() {
		GameManager.TaskChanged += OnTaskChange;
	}

	private void OnDestroy() {
		GameManager.TaskChanged -= OnTaskChange;
	}

	public void OnTaskChange(object sender, TaskChangeEventArgs t) {
		if(t.Task == ListeningForTask) {
			ContextToWake.gameObject.SetActive(true);
		}
		else {
			ContextToWake.gameObject.SetActive(false);
		}
	}
}
