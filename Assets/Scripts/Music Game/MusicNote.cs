using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
	public char Letter { get; private set; }

	SpriteRenderer spriteRenderer;
	private static Sprite QSprite;
	private static Sprite WSprite;
	private static Sprite ESprite;
	private static Sprite RSprite;

	public void Update() {
		transform.position += 2.5f * Vector3.left * Time.deltaTime;
	}

	public void Init(char letter) {
		spriteRenderer = GetComponent<SpriteRenderer>();
		Letter = letter;
		switch(letter) {
			case 'Q':
				QSprite = QSprite ?? Resources.Load<Sprite>("Sprites/QNote");
				spriteRenderer.sprite = QSprite;
				spriteRenderer.color = new Color(1, 0.42f, 0.79f);
				break;
			case 'W':
				WSprite = WSprite ?? Resources.Load<Sprite>("Sprites/WNote");
				spriteRenderer.sprite = WSprite;
				spriteRenderer.color = new Color(0.27f, 0.57f, 1);
				break;
			case 'E':
				ESprite = ESprite ?? Resources.Load<Sprite>("Sprites/ENote");
				spriteRenderer.sprite = ESprite;
				spriteRenderer.color = new Color(0.6f, 1, 0.6f);
				break;
			case 'R':
				RSprite = RSprite ?? Resources.Load<Sprite>("Sprites/RNote");
				spriteRenderer.sprite = RSprite;
				spriteRenderer.color = new Color(1, 1, 0.6f);
				break;
			default:
				break;
		}
	}

	public void Played() {
		foreach(Transform child in transform) {
			child.gameObject.SetActive(true);
			child.parent = null;
			Destroy(child.gameObject, 2f);
		}
		Destroy(this.gameObject);
	}
}
