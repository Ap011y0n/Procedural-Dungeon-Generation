using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{

    //-- Health values
    public float max_health = 100.0f;
    public float current_health = 100.0f;
    public float hitDamage = 20.0f;
    public float bulletDamage = 10.0f;

    //-- Player GameObject
    public GameObject player;

    //-- Canvas and barlifes objects
    public Canvas healthCanvas;
    public Slider lifeSlider;

    //--Offset to place head up life
    public float offset = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        healthCanvas.transform.position = transform.position + transform.up * offset;

        // -- Check each frame if any entitie does have 0 or less life
        CheckLife();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            current_health -= hitDamage;       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            current_health -= bulletDamage;
        }
    }

    void CheckLife()
    {
        lifeSlider.value = current_health / 100.0f;

        if (current_health <= 0)
            Destroy(this.gameObject);
    }

    void TakeDamage(float damage)
    {
        current_health -= damage;
        CheckLife();
    }

}