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

	public ContextManager ContextManager;
	public LevelManager LevelManager {
		get {
			return ContextManager as LevelManager;
		}
		set {
			ContextManager = value;
		}
	}

	public Timer RemainingTimer;
	public Status Status;
	public CompletedTaskTracker TaskTracker;

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
		if(p.Q) {
			TaskTracker.CompleteTask(Task.Art);
		}
		ContextManager?.HandleInput(p);
	}

	public void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}

public enum Task {
	Art,
	Code,
	Music
}