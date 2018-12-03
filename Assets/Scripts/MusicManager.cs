using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
	private AudioSource audioSource;
	private Coroutine rampCoroutine;

    // Start is called before the first frame update
    void Start()
    {
		GameManager.TaskChanged += TaskChanged;
        audioSource = GetComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.volume = 0;
		audioSource.loop = true;
		audioSource.clip = Resources.Load<AudioClip>("Sounds/Music");
	}

	private void OnDestroy() {
		GameManager.TaskChanged -= TaskChanged;
	}

	public void TaskChanged(object sender, TaskChangeEventArgs t) {
		if( t.Task == Task.Music ) {
			RampToVolume(0.05f);
		}
		else {
			RampToVolume(0.25f);
		}
	}

	public void RampToVolume(float volume) {
		if( Mathf.Abs(audioSource.volume - volume) < 0.05f) {
			return;
		}
		if(rampCoroutine != null){
			StopCoroutine(rampCoroutine);
		}

		if(!audioSource.isPlaying) {
			audioSource.Play();
		}
		rampCoroutine = StartCoroutine(RampVolume(volume));
	}

	private IEnumerator RampVolume(float volume) {
		float startingVolume = audioSource.volume;
		float startTime = 0f;
		while(startTime < 2.2f) {
			audioSource.volume = Mathf.Lerp(startingVolume, volume, startTime / 2f);
			startTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}
