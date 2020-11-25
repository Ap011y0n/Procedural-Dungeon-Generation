using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject mesh;
    public NavMeshAgent agent;
    public Camera camera;
    
    Animator animator;
    Rigidbody rigidbody;

    public float speed = 150.0f;
    public float RotateSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = mesh.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    void Update()
    {
        // Each frame a raycast gets rotation and position information.
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f);

        // -- Player always rotate facing mouse pointer
        Vector3 targetPosition = new Vector3(hit.point.x, agent.transform.position.y, hit.point.z);
        agent.transform.LookAt(targetPosition);

        //mouse angle
        Vector3 reference_target_position = targetPosition;
        reference_target_position.x -= transform.position.x;
        reference_target_position.z -= transform.position.z;

        Vector3 screen_player_position = camera.WorldToScreenPoint(transform.position);

        float angle = Mathf.Atan2(Input.mousePosition.y - screen_player_position.y, Input.mousePosition.x - screen_player_position.x);
        angle = angle * Mathf.Rad2Deg;
        angle -= 90;
        if (angle < 0) angle += 360;
      //  Debug.Log("Angle:" + angle.ToString());

        //Movement
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        {
            Vector3 movement = new Vector3(horizontal_input * speed, 0.0f, vertical_input * speed);
            //rigidbody.AddForce(movement);
            transform.position += movement * Time.deltaTime;
        }

        //Animation Direction
        Vector3 direction = new Vector3(horizontal_input, 0, vertical_input);
        direction = Quaternion.Euler(0, (angle - 90), 0) * direction;

        animator.SetFloat("direction_x", direction.normalized.x);
        animator.SetFloat("direction_z", direction.normalized.z);

        if ((horizontal_input != 0)||(vertical_input != 0))
        {
            animator.SetBool("isRunning", true);
           
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        //Rotation
        if (Input.GetKey(KeyCode.LeftArrow)) { agent.transform.Rotate(-Vector3.up * RotateSpeed * Time.deltaTime); }
        else if (Input.GetKey(KeyCode.RightArrow)) { agent.transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime); }

        // -- Movement through navmesh with right click and raycast info. Deprecated
        if (Input.GetMouseButtonDown(1))
        {
            //agent.destination = hit.point;
        }
    }
}