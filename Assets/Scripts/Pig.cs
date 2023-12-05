using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Pig : BattleUnit
{
    public override float GetMaxHealth()
    {
        if (GameStats.Instance != null)
        {
            return base.GetMaxHealth() *(1+GameStats.Instance.HealthIncrease);
        }
        else return base.GetMaxHealth();
    }

    public override float GetStrength()
    {
        if (GameStats.Instance != null)
        {
            return base.GetStrength() * (1+GameStats.Instance.AttackIncrease);
        }
        else return base.GetMaxHealth();
    }

    //public GameObject coinParticles;
    //public FamilyMember f;
    //public GameObject selected;

    /*
    private void Start()
    {
        selected.SetActive(false);
        StartCoroutine(DealDamageOverTime());
        f = GetComponent<FamilyMember>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Debug.Log("Battle begin");
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            //if not already battling enemy add enemy to opponents
            if (!opponents.Contains(e))
            {
                opponents.Add(e);
                Debug.Log("Adding"+ e.name);
            }
        }
    }
   

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if (opponents.Contains(e))
            {
                opponents.Remove(e);
                Debug.Log("removing" + e.name);
            }
        }
    }

    public void ModifyMoney(int value)
    {
        stats.coinValue += value;
        f.AdjustSize(stats.coinValue);

    }

    public void OnMouseOver()
    {
        UnitController.overUnit = true;
        
    }
    public void OnMouseExit()
    {
        UnitController.overUnit = false;
    }

    public void OnMouseDown()
    {
        ToggleSelectedUnit();
    }

    public void Eat(GameObject food)
    {
        stats.health += food.GetComponent<Food>().healthValue;
        if (stats.health > stats.maxHealth)
        {
            int diff =   stats.health - stats.maxHealth;
            stats.coinValue+=diff;
           
            
        }
        stats.health = Mathf.Min(stats.maxHealth, stats.health);
        f.AdjustSize(stats.coinValue);
        stats.UpdateStats();
        Destroy(food);
    }


    public void ToggleSelectedUnit()
    {
        if (UnitController.selectedUnits.Contains(this))
        {
            UnitController.selectedUnits.Remove(this);
            selected.SetActive(false);
        }
        else
        {
            UnitController.Instance.ClearUnits();
            
            UnitController.selectedUnits.Add(this);
            selected.SetActive(true);
        }
    }

    public override void GetDead()
    {
        if (CityManager.Instance.allMembers.Contains(this.GetComponent<FamilyMember>()))
        {
            CityManager.Instance.allMembers.Remove(this.GetComponent<FamilyMember>());
        }

        if (UnitController.selectedUnits.Contains(this))
        {
            UnitController.selectedUnits.Remove(this);
        }
            CityManager.ModifyMoney(stats.coinValue);
        Instantiate(coinParticles);
        coinParticles.transform.localScale = Vector3.one * stats.coinValue / 3;
        coinParticles.transform.position = gameObject.transform.position;
        base.GetDead();
    }
    */
}
