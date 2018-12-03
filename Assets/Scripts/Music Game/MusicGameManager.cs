using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGameManager : ContextManager
{
	public static Task Task = Task.Music;

	private NoteSpawner spawner;
	public AcceptArea AcceptArea;
	public Tutorial Tutorial;
	public ScoreScreen ScoreScreen;
	public int Score { get; private set; }

	public override void OnEnable() {
		base.OnEnable();
		Score = 0;
		spawner = spawner ?? GetComponent<NoteSpawner>();
		Init();

		if(!PlayedBefore) {
			StartCoroutine(ShowTutorialThenStart());
			PlayedBefore = true;
		}
		else {
			spawner.StartSpawning(SongOver);
		}
	}

	public override void Restart() {
		Score = 0;
		spawner.StartSpawning(SongOver);
		Init();
	}

	public override void Init() {
		base.Init();
		AcceptArea.Init(ReliefActivity == ReliefActivity.Eating);
	}

	public void SongOver() {
		float normaleScore = Score/45f;
		GameManager.Instance.TaskTracker.CompleteTask(Task, normaleScore);
		ScoreScreen.Show(Task, normaleScore);
	}

	public override void HandleInput(InputPackage p) {
		int delta = AcceptArea.NoteMatch(p);
		Score = Mathf.Max(Score + delta, 0);
	}

	private IEnumerator ShowTutorialThenStart() {
		Tutorial.Show();
		yield return new WaitForSeconds(5f);
		spawner.StartSpawning(SongOver);
	}
}
