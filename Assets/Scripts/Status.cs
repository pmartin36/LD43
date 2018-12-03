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

	public bool Paused { get; set; }

	public ReliefActivity ReliefActivity { get; private set; }
	private Timer RemainingTimer;

	private Task CurrentTask;

	public Status() {
		Boredom = 0;
		Sleepiness = 0f;
		Anxiety = 0.2f;
		Hunger = 0f;
		Paused = true;

		RemainingTimer = GameManager.Instance.RemainingTimer;
		GameManager.TaskChanged += SwitchingTask;
	}

	~Status() {
		GameManager.TaskChanged -= SwitchingTask;
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
		if(Paused) return;

		// get totally bored after 8 hours
		Boredom += CurrentTask == Task.None ? 0 : deltaTime / (3600f * 8f);
		if(Boredom >= 1) {
			GameManager.Instance.JammerQuit("due to boredom");
		}
	
		// get sleepy after 16 hours
		Sleepiness += deltaTime / (3600f * 16f);
		if (Sleepiness >= 1) {
			GameManager.Instance.JammerQuit("because you fell asleep");
		}

		// get more anxious as we get closer to deadline
		Anxiety += ReliefActivity == ReliefActivity.Outside ?
			-deltaTime / (3600f * 4f) :
			deltaTime / (3600 * Mathf.Lerp(6f, 32f, RemainingTimer.Value / GameManager.TOTAL_TIME));
		if (Anxiety >= 1) {
			GameManager.Instance.JammerQuit("because you had a nervous breakdown");
		}

		// starve after 10 hours
		Hunger += ReliefActivity == ReliefActivity.Eating ?
			-deltaTime / (3600f * 4f):
			deltaTime / (3600f * 10f);
		if(Hunger >= 1) {
			GameManager.Instance.JammerQuit("because you were starving");
		}
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
