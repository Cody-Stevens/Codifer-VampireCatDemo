using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoveLinkTwo : MonoBehaviour
{
    public FamilyMember famMemberOne;
    public FamilyMember famMemberTwo;

    private LineRenderer lineRenderer;

	public Transform heartIcon;

	public TextMeshProUGUI babyTimerDisplay;
	public float babyTimer;
	public GameObject[] babies;

	bool once;
	public ParentalLink parentalLink;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		lineRenderer.SetPosition(0, famMemberOne.transform.position);
		lineRenderer.SetPosition(1, famMemberTwo.transform.position);
		Vector2 midpoint = (famMemberOne.transform.position + famMemberTwo.transform.position)/2;
		heartIcon.position = midpoint;

		if(babyTimer <= 0 && once == false){ 
			GameObject instance = Instantiate(babies[Random.Range(0, babies.Length)], new Vector2(transform.position.x, transform.position.y - 6f), Quaternion.identity);
			for (int i = 0; i < instance.transform.childCount; i++)
			{
				ParentalLink lineRend = Instantiate(parentalLink, transform.position, Quaternion.identity);
				lineRend.parents = heartIcon;
				lineRend.kid = instance.transform.GetChild(i).transform;
			}
			babyTimerDisplay.gameObject.SetActive(false);
			once = true;
		} else{ 
			if(once == false){
				babyTimer -= Time.deltaTime;
			}			
		}

		babyTimerDisplay.text = Mathf.RoundToInt(babyTimer).ToString();

	}
}
