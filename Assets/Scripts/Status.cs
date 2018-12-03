using UnityEngine;
using System.Collections;
using System;
using System.Threading.Tasks;
using System.Threading;

public enum ReliefActivity {
	None,
	Sleeping,
	Eating,
	Outside
}

public class Status  {
	public static event EventHandler<ReliefActivityEventArgs> ReliefActivityChanged;

	private CancellationTokenSource cts;

	public float Boredom { get; set; }
	public float Sleepiness { get; set; }
	public float Anxiety { get; set; }
	public float Hunger { get; set; }

	public ReliefActivity ReliefActivity { get; private set; }
	private Timer RemainingTimer;

	private Task CurrentTask;

	public Status() {
		Boredom = 0;
		Sleepiness = 0f;
		Anxiety = 0.2f;
		Hunger = 0f;

		RemainingTimer = GameManager.Instance.RemainingTimer;
		GameManager.TaskChanged += SwitchingTask;
	}

	public void BeginReliefActivity(ReliefActivity a) {
		if( a == ReliefActivity ) return;

		float delay = 
			a == ReliefActivity.Eating ? (2 * 3600) * 1000 / GameManager.TIMESCALE :
			a == ReliefActivity.Outside ? (3 * 3600) * 1000 / GameManager.TIMESCALE :
			2000;
		ReliefActivity = a;

		if(cts != null) {
			cts.Cancel();
		}
		cts = new CancellationTokenSource();
		System.Threading.Tasks.Task.Delay((int)delay, cts.Token).ContinueWith(t => ReliefEnding(a));

		ReliefActivityChanged?.Invoke(this, new ReliefActivityEventArgs(ReliefActivity));
	}

	public void Update(float deltaTime) {
		// get totally bored after 4 hours
		Boredom += CurrentTask == Task.None ? 0 : deltaTime / (3600f * 4f);
	
		// get sleepy after 16 hours
		Sleepiness += deltaTime / (3600f * 16f);

		// get more anxious as we get closer to deadline
		Anxiety += ReliefActivity == ReliefActivity.Outside ?
			-deltaTime / (3600f * 4f) :
			deltaTime / (3600 * Mathf.Lerp(4f, 32f, RemainingTimer.Value / GameManager.TOTAL_TIME));

		// starve after 24 hours
		Hunger += ReliefActivity == ReliefActivity.Eating ?
			-deltaTime / (3600f * 4f):
			deltaTime / (3600f * 24f);
	}

	public void SwitchingTask(object sender, TaskChangeEventArgs args) {
		CurrentTask = args.Task;
		if(CurrentTask == Task.None) {
			Boredom = Mathf.Max(0, Boredom - 0.5f);
		}
	}

	public void Sleep() {
		Sleepiness = Mathf.Max(0, Sleepiness - 0.75f);
	}

	private void ReliefEnding(ReliefActivity a) {
		if (a == ReliefActivity || ReliefActivity == ReliefActivity.None) {
			ReliefActivity = ReliefActivity.None;
			ReliefActivityChanged?.Invoke(this, new ReliefActivityEventArgs(ReliefActivity));
		}
	}
}

public class ReliefActivityEventArgs : EventArgs {
	public ReliefActivity Activity { get; set; }
	public ReliefActivityEventArgs (ReliefActivity a) {
		Activity = a;
	}
}
