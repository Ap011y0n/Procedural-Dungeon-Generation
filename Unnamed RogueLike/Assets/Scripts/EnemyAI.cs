using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{ 
    NavMeshAgent AI;
    GameObject player;
    public float speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        AI = GetComponent<NavMeshAgent>();
        AI.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        AI.SetDestination(player.transform.position);
    }

}