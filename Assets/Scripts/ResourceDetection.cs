using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ResourceDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public Dictionary<int, FamilyMember> potentialMates = new Dictionary<int, FamilyMember>();
    public Dictionary<int, float> noMateList = new Dictionary<int, float>();
    public int potentialMateLength;
  public FamilyMember f;

    private float retryMateAfterSeconds = 20f;

    void Start()
    {

        StartCoroutine(UpdateMateSelection());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator UpdateMateSelection()
    {
        while (true) {
            potentialMateLength = potentialMates.Count;
            CleanUpNoMateList();
            yield return new WaitForSeconds(CityManager.timeMultiplier);
        }
    }


    public FamilyMember tryFindPotentialMate()
    {
        if (potentialMates.Count > 0)
        {
            Debug.Log("Potential Mates: " + potentialMates.Count);
            List<FamilyMember> hottestMates = potentialMates.Values.OrderBy(littleGuyStats => 0f/*littleGuyStats.stats.endurance*/).ToList();
            int currPickIndex = hottestMates.Count - 1;
            bool foundPotentialMate = false;
            while (!foundPotentialMate && currPickIndex >= 0)
            {
                FamilyMember currLittleHumanStats = hottestMates[currPickIndex];
                int currHumanId = currLittleHumanStats.gameObject.GetInstanceID();
                if (!noMateList.ContainsKey(currHumanId))
                {
                    return hottestMates[currPickIndex];
                }
                currPickIndex -= 1;
            }
        }

        return null;
    }

    private void CleanUpNoMateList()
    {
        List<int> keysToRemove = new List<int>();
        foreach(KeyValuePair<int, float> keyValue in noMateList){
            if(Time.time - keyValue.Value > retryMateAfterSeconds){
                keysToRemove.Add(keyValue.Key);
            }
        }

        foreach (int mateId in keysToRemove){
            noMateList.Remove(mateId);
        }
    }

    private bool WantsToMate(Stats humanStats){
        return humanStats.isAdult;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Mate") && other.GetComponent<FamilyMember>() !=null)
        {
            int mateId = other.gameObject.GetInstanceID();
            FamilyMember fam = other.GetComponent<FamilyMember>();
         
            // check if littleGuy can mate
            // if mate is not already in list
            if (WantsToMate(fam.stats) && !potentialMates.ContainsKey(mateId))
            {
                Debug.Log(gameObject.name + " has " + potentialMates.Count + " potential mates");
                potentialMates.Add(mateId, fam);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mate"))
        {
            // check if littleGuy can mate
            // if mate is not already in list
            int mateId = other.gameObject.GetInstanceID();
            if (potentialMates.ContainsKey(mateId))
            {
                Debug.Log(gameObject.name + " has " + potentialMates.Count + " potential mates");
                potentialMates.Remove(mateId);
            }
        }
    }
}
