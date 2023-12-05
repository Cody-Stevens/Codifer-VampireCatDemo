using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TextMeshProUGUI statsText,ageText,healthText,strText,speedText;
    [SerializeField] private Image healthbar; 
    public bool isFriendly;
    public StatsWrapper Wrapper;
    public Stats stats
    {
        get { return Wrapper.Stats; }
        set { Wrapper.Stats = value; }
    }
    private void Awake()
    {

        //stats =  GetComponent<Stats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //stats = transform.parent.GetComponent<Attacker>().Stats;


        //  InvokeRepeating("SlowUpdate", 0, CityManager.timeMultiplier);
    }




    void  CheckType()
    {
        isFriendly = GetComponent<Enemy>()==null &&
            GetComponent<FamilyMember>() !=null;
        
    }
    // Update is called once per frame
    void Update()
    {
      
    }

    void SlowUpdate()
    {
        if (isFriendly)
        {
            statsText.text =
              "Strength: " + stats.strength + "\n" +
              //"Endurance: " + stats.endurance + "\n" +
              "Speed: " + stats.speed + "\n";
        }
    }

    private void OnMouseOver()
    {
        statsPanel.SetActive(true);
        
    }

    private void OnMouseExit()
    {
        statsPanel.SetActive(false);
    }


    public void UpdateStats()
    {
        if(stats== null)
        {

        }
        SetHealth();
        SetCoinText();
        SetStrength();
        SetSpeed();
     
    }


    private void SetStrength()
    {
        strText.text = stats.strength.ToString();
    }

    private void SetSpeed()
    {
        speedText.text = stats.speed.ToString();
    }
    private void SetHealth()
    {
        /*
        if (healthText == null) return;
        healthText.text =  stats.health +"/" + stats.maxHealth;

        healthbar.fillAmount = (float)stats.health / stats.maxHealth;
        if (stats.health > stats.maxHealth)
        {
            healthText.color = Color.green;
        }
        else
        {
            if (stats.health <= 3)
            {
                healthText.color = Color.red;
            }
            else
            {
                healthText.color = Color.white;
            }
        }
        */
    }
    private void SetCoinText()
    {
        if (ageText == null) return;

        ageText.text = "Coins: " + stats.coinValue.ToString();
        if (stats.coinValue < 40)
        {
            ageText.color = Color.gray;
        }
        else
        {
            if (stats.coinValue >= 100)
            {
                ageText.color = Color.red;
            }
            else
            {
                ageText.color = Color.green;
            }
        }
    }
}
