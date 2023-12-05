using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class FamilyMember : MonoBehaviour
{
	private enum FriendlyState
	{
		Patrolling=0,
		LookingForMate=1,
		WaitingForMate=2,
		GoingToMate=3,
		Mating=4,
	
	}

	private Vector2 target;
	public GameObject loveLink;
    public StatsWrapper Wrapper;
    public Stats stats
    {
        get { return Wrapper.Stats; }
        set { Wrapper.Stats = value; }
    }
	private Patrol patrol;
	private FriendlyState humanState = FriendlyState.Patrolling;
	private FamilyMember mate;

	public TextMeshProUGUI characterName;
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI ageText;
	[SerializeField] private TextMeshProUGUI humanStateText;
	public string[] names;

	private NavMeshAgent agent;


	private float mateStartedTime = -10;
	private float mateEndedTime = -10;
	private Vector2 mateTarget;
	private bool shouldGiveBirth = false;
	private FamilyMember potentialMate;
	private float birthTime;


	[SerializeField] private GameObject graphics;
	[SerializeField] private ResourceDetection resourceDetection;
	[SerializeField] private GameObject babyPrefab;
	[SerializeField] private float matingCooldown = 30f;
	[SerializeField] private float matingTime = 3f;
	[SerializeField] private float matingDistance = 3f;
	[SerializeField] private float retryAfterDenialTime = 3f;
	[SerializeField] private float timeAsChild = 18f;
	[SerializeField] private float childStartSize = 0.5f;
	[SerializeField] private float geneticMorphMax = 2f;
	[SerializeField] private AnimationCurve growthOverTime;
	[SerializeField] DisplayStats display;
	[SerializeField] public GameObject canvas;
	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		characterName.text = names[Random.Range(0, names.Length)];
		//stats = canvas.GetComponent<Stats>();
		display = canvas.GetComponent<DisplayStats>();
        patrol = GetComponent<Patrol>();
		birthTime = Time.time;
	
		StartCoroutine(HandleGrowth());

		//CityManager.Instance.allMembers.Add(this);
		
	}

	private void Update()
	{
		//if (target != Vector2.zero)
		//{
		//	transform.position = target;
		//}


		//if (Input.GetMouseButtonUp(0))
		//{
		//	target = Vector2.zero;
		//}

		
		HandleState();
		
		humanStateText.SetText(humanState.ToString());
		
	}

	public void AdjustSize(int num)
	{
       

        float percentGrown = num / timeAsChild;
		if (!stats.isAdult)
			graphics.transform.localScale = Vector3.one * growthOverTime.Evaluate(percentGrown);
		else
		{
			percentGrown = Mathf.Min(percentGrown, 3);
			graphics.transform.localScale = Vector3.one * percentGrown;
		}
    }

	IEnumerator HandleGrowth()
	{
	
		while (gameObject.activeSelf)
		{
            if (!stats.isAdult)
            {
				//increase coin value each mult of time while is not adult
				++stats.coinValue;



            }
		
            stats.isAdult = stats.coinValue > timeAsChild;
			
			AdjustSize(stats.coinValue);
     

			//stats.UpdateStats();
       
            //yield return new WaitForSeconds( CityManager.timeMultiplier);

			yield return new WaitForSeconds(1f);
			
		}
	}

	public void GetDead()
	{
        Destroy(gameObject);
        //CityManager.Instance.HandleStats();
    }

	private void HandleState()
	{
        switch (humanState)
        {
            case FriendlyState.Patrolling:
                // check if mating cooldown has ended
                if (stats.isAdult &&  mateEndedTime > matingCooldown)
                {
                    // if so check if there are mates near by
                    if (resourceDetection.potentialMates.Count > 0)
                    {
						
						  potentialMate = resourceDetection.tryFindPotentialMate();
                        if (potentialMate!= null)
                        {
                            // stop patroling script
                            patrol.enabled = false;
                            agent.SetDestination(transform.position);
                            humanState = FriendlyState.LookingForMate;
                        }
                    }
                }
                break;

            case FriendlyState.LookingForMate:
                // call mateCall animation 
                // found potential mate
                // sendMateRequest to LittleGuyManager
                bool receivedConsent = potentialMate.DoesConsentToMate(stats);
                // check if consented
                if (receivedConsent)
                {
                    // set move destination to mate spot
                    mate = potentialMate;
                    mateTarget = potentialMate.transform.position;
                    agent.SetDestination(mateTarget);
                    humanState = FriendlyState.GoingToMate;
                }
                else
                {
                    int potentialMateId = potentialMate.gameObject.GetInstanceID();
                    // add mate to noMateList
                    resourceDetection.noMateList.Add(potentialMateId, Time.time);
                    humanState = FriendlyState.Patrolling;
                    patrol.enabled = true;
                    patrol.BeginPatrolling();
                }

                // move to WaitingForConsent
                break;

            case FriendlyState.GoingToMate:
                // if reached mating spot
                if (agent.remainingDistance < matingDistance)
                {
                    // play mating animation
                    agent.SetDestination(transform.position);
                    mateStartedTime = Time.time;
                    humanState = FriendlyState.Mating;
                    mate.BeginMating();
                    shouldGiveBirth = true;
                }
                break;

            case FriendlyState.Mating:
                //	if mating time elapsed
                if (Time.time - mateStartedTime > matingTime)
                {
                    mateEndedTime = Time.time;
                    //play patroling animation
                    // make babies
                    if (shouldGiveBirth && mate!=null)
                    {
                        Debug.Log("Make Babies!");
                        GiveBirth(stats, mate.stats);
                    }

                    humanState = FriendlyState.Patrolling;
                    shouldGiveBirth = false;
                    patrol.enabled = true;
                    patrol.BeginPatrolling();
                }
                break;

            case FriendlyState.WaitingForMate:
                break;

            default:
                break;
        }
		matingCooldown -= Time.deltaTime;
    }


    public bool DoesConsentToMate(Stats mater)
	{
		bool doesConsent = CanMate() && WillMate(mater);
		if (doesConsent) humanState = FriendlyState.WaitingForMate;
		return doesConsent;
	}

	public void BeginMating()
	{
		patrol.enabled = false;
		agent.SetDestination(transform.position);
		mateStartedTime = Time.time;
		humanState = FriendlyState.Mating;
	}

	private void GiveBirth(Stats popStats, Stats mumStats)
	{
		int numBabies = Random.Range(1, 3);
		float babySpawnOffset = 2f;
		float currOffset = -1f;

		if(popStats == null)
		{
			Debug.Log("No pop");
			return;
		}
        if (mumStats == null)
        {
            Debug.Log("No mum");
			return;
        }
 
		// birth babies
		for (int i = 0; i < numBabies; i++)
		{
			GameObject currBaby = Instantiate(babyPrefab,null);

			// set babies stats based off their parents
			Stats babyStats = currBaby.GetComponent<FamilyMember>().stats;
			//babyStats.endurance = RandomBetweenTwoStats(popStats.endurance, mumStats.endurance);
			babyStats.speed = RandomBetweenTwoStats(popStats.speed, mumStats.speed);
			babyStats.strength = RandomBetweenTwoStats(popStats.strength, mumStats.strength);
			babyStats.isAdult = false;
			babyStats.coinValue = 0;
			currBaby.GetComponent<FamilyMember>().birthTime = Time.time;
			currBaby.GetComponent<Patrol>().enabled = true;
			//currBaby.GetComponent<FamilyMember>().matingCooldown = 100 / stats.endurance;

			// set babies position
			currBaby.transform.position =
				new Vector2(transform.position.x, transform.position.y) +
				Vector2.down * 2 +
				Vector2.left * currOffset * babySpawnOffset;

			currOffset += 1;
		}
        humanState = FriendlyState.Patrolling;
        shouldGiveBirth = false;
        patrol.enabled = true;
        patrol.BeginPatrolling();
        //matingCooldown = 100/stats.endurance ;
	}

	private float RandomBetweenTwoStats(float statOne, float statTwo)
	{
		float chosenStat;
		float genecticMorfValue = Random.Range(-geneticMorphMax, geneticMorphMax);
		if (Random.Range(0, 1) <= 0.5f)
		{
			chosenStat = statOne;
		}
		else
		{
			chosenStat = statTwo;
		}

		// add genetic mutation (randomly change stats a little bit)
		chosenStat += genecticMorfValue;
		if (chosenStat < 0)
		{
			chosenStat = 0;
		}

		if (chosenStat > 10)
		{
			chosenStat = 10;
		}

		return chosenStat;
	}


	private bool MatingCooldownEnded()
	{
		return true;//matingCooldown-stats.endurance < 0;
	}

	private bool CanMate()
	{
		return
			stats.isAdult &&
			MatingCooldownEnded() &&
			//CityManager.Instance.allMembers.Count < 10 &&
			humanState != FriendlyState.Mating &&
			humanState != FriendlyState.WaitingForMate &&
			humanState != FriendlyState.GoingToMate;
	}
	private bool WillMate(Stats mater)
	{
		float randomNumber = Random.Range(0, 10);
		return true;//mater.endurance <= randomNumber;
	}

	private void OnMouseOver()
	{
		if (Input.GetMouseButton(0))
		{
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

	}
}
