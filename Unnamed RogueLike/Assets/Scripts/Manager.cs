using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager: MonoBehaviour
{

    GameObject player;
    public GameObject Prefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
           
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                Instantiate(Prefab, hit.point, Quaternion.identity);  
        }
    }
}
