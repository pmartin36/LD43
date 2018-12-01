using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {
    
	public float Value { get; set; } = 0;
	public bool Paused { get; set; } = true;
	public bool Expired { get; private set; }

	public void Update(float deltaTime) {
		if(!Paused) {
			Value += deltaTime;
			if(Value <= 0) {
				Expired = true;
				Value = 0;
				Paused = true;
			}
		}
	}
}
