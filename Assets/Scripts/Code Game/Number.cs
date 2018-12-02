using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Number : MonoBehaviour
{
	[SerializeField]
	private TMP_InputField fillableField;
	[SerializeField]
	private TMP_Text staticField;

	public bool Fillable { get; private set; }
	private RectTransform rt;
	public int Value {
		get {
			bool success = int.TryParse(Fillable ? fillableField.text : staticField.text, out int val);
			return success ? val : -999;
		}
	}

    public void Init( bool isFillable, float left, float right, int value = -1 ) {
		Fillable = isFillable;
		if(isFillable) {
			fillableField.gameObject.SetActive(true);
			staticField.gameObject.SetActive(false);
			fillableField.text = "";
		}
		else {
			fillableField.gameObject.SetActive(false);
			staticField.gameObject.SetActive(true);
			staticField.text = value.ToString();
		}

		rt = GetComponent<RectTransform>();
		rt.offsetMin = new Vector2( left, 0 );
		rt.offsetMax = new Vector2( -right, 0 );
	}
}
