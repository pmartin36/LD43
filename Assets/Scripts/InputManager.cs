using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPackage {
	public bool Q { get; set; }
	public bool W { get; set; }
	public bool E { get; set; }
	public bool R { get; set; }
}

public class InputManager : MonoBehaviour
{
    void Update()
    {
		InputPackage p = new InputPackage();
		if(Input.GetKeyDown(KeyCode.Escape)) {
			GameManager.Instance.Exit();
		}

		p.Q = Input.GetKeyDown(KeyCode.Q);
		p.W = Input.GetKeyDown(KeyCode.W);
		p.E = Input.GetKeyDown(KeyCode.E);
		p.R = Input.GetKeyDown(KeyCode.R);

		GameManager.Instance.HandleInput(p);
	}
}
