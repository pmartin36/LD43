using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextManager : MonoBehaviour
{
	public bool PlayedBefore { get; set; } = false;
	public virtual void OnEnable() {
		GameManager.Instance.ContextManager = this;
	}
	public abstract void HandleInput(InputPackage p);
	public abstract void Restart();
}
