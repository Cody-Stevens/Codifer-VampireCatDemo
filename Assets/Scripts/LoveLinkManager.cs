using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveLinkManager : MonoBehaviour
{
    public FamilyMember familyMemberOne;
    public FamilyMember familyMemberTwo;

	public LineRenderer loveLink;

	private void Update()
	{
		if(familyMemberOne != null && familyMemberTwo != null){ 
			LineRenderer instance = Instantiate(loveLink, transform.position, Quaternion.identity);
			instance.SetPosition(0, familyMemberOne.transform.position);
			instance.SetPosition(1, familyMemberTwo.transform.position);
			instance.GetComponent<LoveLinkTwo>().famMemberOne = familyMemberOne;
			instance.GetComponent<LoveLinkTwo>().famMemberTwo = familyMemberTwo;
			Destroy(familyMemberOne.loveLink);
			Destroy(familyMemberTwo.loveLink);
			familyMemberOne = null;
			familyMemberTwo = null;
		}


	}
}
