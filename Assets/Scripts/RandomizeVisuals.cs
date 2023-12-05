using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeVisuals : MonoBehaviour
{
    private SpriteRenderer rend;
	public Sprite[] sprites;
	public Color[] colors;

	public Animator anim;

	private void Start()
	{
		rend = GetComponent<SpriteRenderer>();
		rend.sprite = sprites[Random.Range(0, sprites.Length)];
		rend.color = colors[Random.Range(0, colors.Length)];

		if(anim != null){ 
			anim.speed = Random.Range(0.75f, 1.25f);
		}
	}
}
