using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talents : MonoBehaviour
{
    public List<GameObject> talents;
    public GameObject content;
    public void ShowTalentSelection()
    {
        for(int i = 0; i < 3; i++)
        {
            Instantiate(talents[i], content.transform);
            
        }
    }

    public void ShowTalent()
    {
        
    }

   
}
