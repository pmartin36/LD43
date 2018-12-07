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

	public int CodeTasksCompleted { get; private set; }
	public int MusicTasksCompleted { get; private set; }
	public int ArtTasksCompleted { get; private set; }

	public float CodeTaskAverage { get; private set; }
	public float MusicTaskAverage { get; private set; }
	public float ArtTaskAverage{ get; private set; }

	public void CompleteTask(Task t, float score) {
		int newCount = 0;
		switch (t) {
			case Task.Art:
				ArtTaskAverage = (ArtTaskAverage * ArtTasksCompleted + score) / (ArtTasksCompleted + 1);
				newCount = ++ArtTasksCompleted;
				break;
			case Task.Code:
				CodeTaskAverage = (CodeTaskAverage * CodeTasksCompleted + score) / (CodeTasksCompleted + 1);
				newCount = ++CodeTasksCompleted;
				break;
			case Task.Music:
				MusicTaskAverage = (MusicTaskAverage * MusicTasksCompleted + score) / (MusicTasksCompleted + 1);
				newCount = ++MusicTasksCompleted;
				break;
		}
		TaskCompleted?.Invoke(this, new TaskCompleteEventArgs(t, newCount));
	}
}