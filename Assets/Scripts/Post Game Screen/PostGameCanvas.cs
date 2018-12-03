using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameCanvas : MonoBehaviour
{
    public PostGameScreen PostGameScreen;

	public void Start() {
		GameManager.Instance.PostGameCanvas = this;
	}
}
