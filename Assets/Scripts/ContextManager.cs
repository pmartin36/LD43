using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextManager : MonoBehaviour
{
	public bool PlayedBefore { get; set; } = false;

	protected ReliefActivity ReliefActivity;
	[SerializeField]
	protected GameObject SunGlare;

	public virtual void OnEnable() {
		GameManager.Instance.ContextManager = this;
	}
	public virtual void OnDisable() {
		if(SunGlare != null)
			SunGlare.SetActive(false);
	}
	public virtual void Init() {
		ReliefActivity = GameManager.Instance.Status.ReliefActivity;
		SunGlare.SetActive( ReliefActivity == ReliefActivity.Outside );
	}
	public abstract void HandleInput(InputPackage p);
	public abstract void Restart();
}
