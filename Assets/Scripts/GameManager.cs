using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputManager))]
public class GameManager : Singleton<GameManager> {

	public static float TIMESCALE = 180f;
	public static float TOTAL_TIME = 48 * 3600;
	public static event EventHandler<TaskChangeEventArgs> TaskChanged;

	public ContextManager ContextManager;

	public Timer RemainingTimer;
	public Status Status;
	public CompletedTaskTracker TaskTracker;

	public PlayAreaMenu PlayAreaMenu;
	public ScoreScreen ScoreScreen;
	public CameraController camera;

	public void Awake() {
		RemainingTimer = new Timer();
		Status = new Status();
		TaskTracker = new CompletedTaskTracker();
	}

	public void Start() {
		RemainingTimer.Value = TOTAL_TIME;
		RemainingTimer.Paused = false;
	}

	public void Update() {
		RemainingTimer.Update(-Time.deltaTime * TIMESCALE);
		if(RemainingTimer.Expired) {
			// jam over, do scoring
		}

		Status.Update(Time.deltaTime * TIMESCALE);
	}

	public void Exit() {
		Application.Quit();
	}

	public void HandleInput(InputPackage p) {
		ContextManager?.HandleInput(p);
	}

	public void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void SetCurrentTask(Task t) {
		TaskChanged?.Invoke(this, new TaskChangeEventArgs(t));
	}

	public void ReturnToHub() {
		TaskChanged?.Invoke(this, new TaskChangeEventArgs(Task.None));
		PlayAreaMenu.Open();
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