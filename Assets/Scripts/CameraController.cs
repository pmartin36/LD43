using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public bool Sleeping = false;
	public float SleepPercent = 0;
	public Material EffectMaterial;

	private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
		GameManager.Instance.camera = this;
    }

    // Update is called once per frame
    void Update()
    {
        EffectMaterial.SetFloat("_Percent", SleepPercent/2f);
    }

	private void OnRenderImage(RenderTexture source, RenderTexture dest) {
		if(Sleeping) {
			Graphics.Blit(source, dest, EffectMaterial);
		}
		else {
			Graphics.Blit(source, dest);
		}
	}

	public void ShowSleep() {
		Sleeping = true;
		anim.Play("Sleep");
	}

	public void StopShowSleep() {
		anim.Play("Wake");
	}
}
