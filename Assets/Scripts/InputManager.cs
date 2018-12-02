using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPackage {
	public bool Q { get; set; }
	public bool W { get; set; }
	public bool E { get; set; }
	public bool R { get; set; }
	public bool Z { get; set; }

	public bool LeftMouse { get; set; }
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

		p.Z = Input.GetKeyDown(KeyCode.Z);
		p.LeftMouse = Input.GetMouseButton(0);

		GameManager.Instance.HandleInput(p);
	}
}
