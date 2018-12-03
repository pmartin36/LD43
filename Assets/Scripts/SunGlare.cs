using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunGlare : MonoBehaviour
{
	private Image image;
	private float offset;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        offset -= Time.deltaTime * 0.1f;
		image.material.SetFloat("_Offset", offset);
    }
}
