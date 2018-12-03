using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(MusicManager))]
public class GameManager : Singleton<GameManager> {

	public static float TIMESCALE = 180f;
	public static float TOTAL_TIME = 0.25f * 3600;
	public static event EventHandler<TaskChangeEventArgs> TaskChanged;

	public ContextManager ContextManager;
	private MusicManager musicManager;

	public Timer RemainingTimer;
	public Status Status;
	public CompletedTaskTracker TaskTracker;

	public PlayAreaMenu PlayAreaMenu;
	public ScoreScreen ScoreScreen;
	public CameraController camera;

	public QuitScreen QuitScreen;

	public PostGameCanvas PostGameCanvas;
	private bool gameOver = false;

	public bool SkipIntro = true;

	public void Awake() {
		musicManager = GetComponent<MusicManager>();
		Init();
	}

	public void BeginJam () {
		RemainingTimer.Paused = false;
		Status.Paused = false;
		ReturnToHub();
	}

	public void Init() {
		RemainingTimer = new Timer(TOTAL_TIME, true);
		Status = new Status();
		TaskTracker = new CompletedTaskTracker();
	}

	public void Update() {
		if(!gameOver) {
			RemainingTimer.Update(-Time.deltaTime * TIMESCALE);
			if(RemainingTimer.Expired) {
				gameOver = true;
				var ps = PostGameCanvas.PostGameScreen;
				ps.gameObject.SetActive(true);
				ps.Init(Status, TaskTracker);
			}
			Status.Update(Time.deltaTime * TIMESCALE);
		}
	}

	public void JammerQuit(string reason) {
		QuitScreen.Show(reason);
	}

	public void Exit() {
		Application.Quit();
	}

	public void HandleInput(InputPackage p) {
		ContextManager?.HandleInput(p);
	}

	public void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Init();
	}

	public void SetCurrentTask(Task t) {
		TaskChanged?.Invoke(this, new TaskChangeEventArgs(t));
	}

	public void ReturnToHub() {
		TaskChanged?.Invoke(this, new TaskChangeEventArgs(Task.None));
		PlayAreaMenu.Open();
	}

	public void BeginReliefActivity(ReliefActivity activity) {
		if(activity == ReliefActivity.Sleeping) {
			GoToSleep();
		}

		if( activity == Status.ReliefActivity ) {
			Status.BeginReliefActivity(ReliefActivity.None);
		}
		else {
			Status.BeginReliefActivity(activity);
		}
	}

	public void GoToSleep() {
		if(!camera.Sleeping) {
			StartCoroutine(Sleep());
		}
	}

	private IEnumerator Sleep() {
		camera.ShowSleep();
		yield return new WaitForSeconds(2f);

		RemainingTimer.Update(-8 * 3600);
		Status.Sleep();

		camera.StopShowSleep();
	}
}

public enum Task {
	None,
	Art,
	Code,
	Music
}

public class TaskChangeEventArgs: EventArgs {
	public Task Task { get; set; }
	public TaskChangeEventArgs(Task task) {
		Task = task;
	}
}