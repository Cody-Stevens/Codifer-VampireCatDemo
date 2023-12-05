using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveLink : MonoBehaviour
{

	public LineRenderer lineRenderer;

	public FamilyMember familyMember;

	private LoveLinkManager manager;

	private void Update()
	{
		manager = FindObjectOfType<LoveLinkManager>();

		lineRenderer.SetPosition(0, transform.position);
		Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lineRenderer.SetPosition(1, cursorPos);

		if(Input.GetMouseButtonDown(1)){ 
			lineRenderer.gameObject.SetActive(false);
			manager.familyMemberOne = null;
			manager.familyMemberTwo = null;
		}
	}

	private void OnMouseDown()
	{
		
		if(manager.familyMemberOne == null){ 
			manager.familyMemberOne = familyMember;
			lineRenderer.gameObject.SetActive(true);
		} else if(manager.familyMemberOne != null && manager.familyMemberTwo == null)
		{
			manager.familyMemberTwo = familyMember;
			lineRenderer.gameObject.SetActive(true);
		}

	}
}
