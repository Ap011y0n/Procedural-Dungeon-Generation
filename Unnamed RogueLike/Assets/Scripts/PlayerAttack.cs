using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    // -- Instances used later to spawn bullets
    public GameObject mesh;

    public GameObject projectile;
    private GameObject new_projectile;

    public Collider weaponCollider;

    Animator animator;

    // -- Damage and Cooldown values
    public float bulletDamage;
    public float shot_cd;
    public bool active;

    // -- Corutine that will make cd shot
    private IEnumerator cooldown;

    // Awake is called before start
    void Awake()
    {
        bulletDamage = 10.0f;
        shot_cd = 3.0f;
        active = true;

        weaponCollider.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        cooldown = ShotCooldown(shot_cd);
        animator = mesh.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Melee attack
        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("isAttacking");
            //animator.Play("Attack");
            weaponCollider.enabled = true;
        }

        // -- OPTION 1: Checks if attack animation finished and disable weapon collider
       
            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                weaponCollider.enabled = false;

        // -- OPTION 2: Maybe will work a Ontriggerexit to disable collider
        
        else
        { //animator.SetBool("isAttacking", false); 
        }

        //Distance Attack
        if (Input.GetMouseButtonDown(1)) {

            //If shot is active then execute and calls corutine to set active to true again 'x' seconds later
            if (active)
            {
                //StartCoroutine(ShotCooldown(shot_cd));
                //active = false;
                UnityEngine.Vector3 projectile_pos = transform.position;
                projectile_pos.y += 1;
                new_projectile = Instantiate(projectile, projectile_pos, transform.rotation);
                Destroy(new_projectile, 2.0f);
            }

        }
    }

    private IEnumerator ShotCooldown(float waitTime)
    {
        active = true;
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
    }
}
