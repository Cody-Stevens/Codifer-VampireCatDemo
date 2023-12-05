using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Food : MonoBehaviour
{

    public int healthValue = 1;
    public TextMeshProUGUI text;


    private void Start()
    {
      //  text.text = healthValue.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Pig>() != null)
            GiveHumanFood(collision.gameObject);
    }

    public void GiveHumanFood(GameObject human)
    {
        //human.GetComponent<Pig>().Eat(gameObject);
    }
}
