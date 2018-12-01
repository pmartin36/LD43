using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCompleteEventArgs : EventArgs {
	public Task CompletedTask { get; set; }
	public int CurrentCompleted { get; set; }

	public TaskCompleteEventArgs(Task t, int current) {
		CompletedTask = t;
		CurrentCompleted = current;
	}
}

public class CompletedTaskTracker
{
	public static event EventHandler<TaskCompleteEventArgs> TaskCompleted;

	public int Code { get; set; }
	public int Music { get; set; }
	public int Art { get; set; }

	public void CompleteTask(Task t) {
		int newCount = 0;
		switch (t) {
			case Task.Art:
				newCount = ++Art;
				break;
			case Task.Code:
				newCount = ++Code;
				break;
			case Task.Music:
				newCount = ++Music;
				break;
		}
		TaskCompleted?.Invoke(this, new TaskCompleteEventArgs(t, newCount));
	}
}