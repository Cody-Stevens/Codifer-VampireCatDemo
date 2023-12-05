using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HominMissle : MonoBehaviour
{
    
        public Transform target;
        public Rigidbody2D rigidBody;
        public float angleChangingSpeed;
        public float movementSpeed;
    public int damage = 1;
    private bool targetFound = false;
    private void Awake()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        if (FindObjectOfType<Enemy>() == null)
        {
            Destroy(gameObject);
            return;
        }
       target= FindObjectOfType<Enemy>().transform;
    }

    private  void FixedUpdate()
        {
     
        MoveToTarget();
        }

    private void MoveToTarget()
    {
        if (!targetFound) return;

          Vector2 direction = (Vector2)target.position - rigidBody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
        rigidBody.velocity = transform.up * movementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            HurtEnemy(collision.gameObject.GetComponent<Enemy>());
        }
    }

  

    private void HurtEnemy(Enemy enemy)
    {
        Debug.Log("Hit");
        //enemy.ModifyHealth(-damage);
        Destroy(gameObject);
    }



}
