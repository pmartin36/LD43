using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DowntimeButtonBorder : MonoBehaviour
{
	public ReliefActivity ButtonActivity;
	private Image image;

	private float opacity;

    // Start is called before the first frame update
    void Start()
    {
        Status.ReliefActivityChanged += ReliefChanged;
		image = GetComponent<Image>();
		image.material.SetFloat("_Opacity", 0);
	}

	private void OnDestroy() {
		Status.ReliefActivityChanged -= ReliefChanged;
	}

	public void Update() {
		image.material.SetFloat("_Opacity", opacity);
	}

	public void ReliefChanged(object sender, ReliefActivityEventArgs args) {	
		if(args.Activity == ButtonActivity) {
			opacity = 0.75f;
		}
		else {
			opacity = 0f;
		}
	}
}
