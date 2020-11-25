using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public float meleeDamage;

    LifeSystem enemy_life;
    Collider weapon;

    // Start is called before the first frame update
    void Start()
    {
        meleeDamage = 20.0f;
        weapon = this.gameObject.GetComponent<Collider>();
        weapon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Enemy"))
        {
            // -- Trying to access current health of each enemy that we hit
           // enemy_life = collider.gameObject.GetComponent<LifeSystem>();
           // enemy_life.health -= meleeDamage;
            collider.gameObject.SendMessage("TakeDamage", meleeDamage);
        }
    }
}
