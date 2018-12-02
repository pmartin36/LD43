using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeGameManager : ContextManager
{
	public override void OnEnable() {
		base.OnEnable();
		Debug.Log("Code Game Started");
	}

	public void Update() {
		
	}

	public override void Restart() {

	}

	public override void HandleInput(InputPackage p) {
		
	}

	public void LateUpdate() {
		
	}
}
