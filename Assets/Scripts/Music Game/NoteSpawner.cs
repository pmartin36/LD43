using UnityEngine;
using System.Collections;
using System;

public class NoteSpawner : MonoBehaviour {

	public MusicNote NotePrefab;
	private float YMaxSpawn;
	private float YMinSpawn;
	private float XSpawn;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	private void CreateSpawnRange() {
		Camera c = Camera.main;
		YMaxSpawn = c.orthographicSize * 0.75f + 1;
		YMinSpawn = -c.orthographicSize * 0.75f + 1;
		XSpawn = c.orthographicSize * c.aspect * 0.75f;
	}

	public void StartSpawning(Action callback) {
		CreateSpawnRange();
		StartCoroutine(Spawn(callback));
	}

	public IEnumerator Spawn(Action callback) {
		bool eating = GameManager.Instance.Status.ReliefActivity == ReliefActivity.Eating;
		int notesCreated = 0;
		
		while(notesCreated < 30) {
			var position = new Vector3(XSpawn, UnityEngine.Random.Range(YMinSpawn, YMaxSpawn));
			MusicNote note = Instantiate<MusicNote>(NotePrefab, position, Quaternion.identity, null);

			int letterSeed = UnityEngine.Random.Range(0, 4);
			char letter = 
				letterSeed == 0 ? 'Q' :
				letterSeed == 1 ? 'W' :
				letterSeed == 2 ? 'E' :
				'R';

			note.Init(letter);
			notesCreated++;
			yield return new WaitForSeconds(1f);
		}
		yield return new WaitForSeconds(5f);
		callback();
	}
}
