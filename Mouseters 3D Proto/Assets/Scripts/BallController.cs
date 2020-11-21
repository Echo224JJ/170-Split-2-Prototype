using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool groundAbility;
    private bool aiming = false;
    public bool camera_follow = false;
    public float force_per_unity = 750;
    //force per unit in drag vector lenth
    public float force_multiplier = 1;
    public float max_unities = 1.2f;
    Vector3 force_to_add;
    Vector3 mousepos;
    Vector3 mouse_start_pos;
    LineRenderer line;
    Vector3 negative_force_vec;
    Vector3 startingPos;
    Vector3 camera_initial_loc;
    // Start is called before the first frame update
    void Start()
    {
        groundAbility = false;
        line = this.GetComponent<LineRenderer>();
        line.startWidth = line.endWidth = 0.03f;
        camera_initial_loc = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        var ballpos = this.transform.position;
        if (camera_follow) {
            Camera.main.transform.SetPositionAndRotation(
                new Vector3(ballpos.x,Camera.main.transform.position.y,camera_initial_loc.z+ballpos.z),
                Camera.main.transform.rotation);
                //Debug.Log(ballpos.z-camera_initial_loc.y);
        }
        if (Input.GetMouseButtonDown(0) == true && (this.GetComponent<Rigidbody>().velocity == Vector3.zero)) {

            aiming = true; // ENTER AIMING MODE
            mouse_start_pos = Input.mousePosition;
        }
        //render line
        if (aiming) {
            //get ballpos in screenspace
            // Vector3 ballpos_screenspace = Camera.main.WorldToScreenPoint(ballpos); 
            //enable line and set its start pos to the ball
            line.enabled = true;
            line.SetPosition(0,ballpos);
            //get mouse pos
            mousepos = Input.mousePosition;
            var heading = ballpos - Camera.main.transform.position;
            var distance = Vector3.Dot(heading, Camera.main.transform.forward);
            var mouseWorld = GetMouseAsWorldSpace(distance);
            var lineend = new Vector3(mouseWorld.x,ballpos.y,mouseWorld.z);

            var lineLen = Vector3.Distance(ballpos,lineend);
            
            //implement max force
            if (lineLen > max_unities) {
                lineLen = max_unities;
                var newend = Vector3.Normalize(new Vector3(lineend.x, 0, lineend.z) - new Vector3(ballpos.x, 0, ballpos.z));
                newend.y = ballpos.y;
                //PrintVector3(newend,1);
                newend = (newend*max_unities) + new Vector3(ballpos.x, 0, ballpos.z);
                line.SetPosition(1,newend);
                
            }
            else {
                line.SetPosition(1,lineend);
                //PrintVector3(lineend,1);
            }

            //Debug.Log("distance? "+lineLen);
            if (Input.GetMouseButtonUp(0) == true) {    
                aiming = false;
                startingPos = this.transform.position;
                shoot(Vector3.Normalize(lineend-ballpos), lineLen);
            }
        }
        else {
            line.enabled = false;
        }
    }
    void shoot(Vector3 dir, float force) {
        var calculatedForce = (dir * (force * (force_multiplier * force_per_unity)));
        //Debug.Log("distance:  "+force+"||calculated force:  "+calculatedForce);
        this.GetComponent<Rigidbody>().AddForce(calculatedForce);
    }

    void OnTriggerEnter (Collider other) {
        if(other.tag == "Hole") {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        if(other.tag == "Water") {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.transform.position = startingPos;
        }
    }
    private Vector3 GetMouseAsWorldSpace(float depth) {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("ray origin" +ray.origin+"   |ray dir" +ray.direction+"   |ray" +(ray.origin + ray.direction)+"   |depth" +depth);
        //var screenvec = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //Debug.Log("vec "+screenvec+"   |ray "+ray);
        return ray.origin + ray.direction * depth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (groundAbility == true)
        {
            if (collision.gameObject.tag == "Rock")
            {
                Destroy(collision.gameObject);
            }
        }
    }
    void PrintVector3(Vector3 message, int type = 1) {
        if (type == 1)
            Debug.Log("X: " + message.x + "  Y: " + message.y + "  Z:" + message.z);
        if (type == 2)
            Debug.LogWarning("X: " + message.x + "  Y: " + message.y + "  Z:" + message.z);
        if (type == 3)
            Debug.LogError("X: " + message.x + "  Y: " + message.y + "  Z:" + message.z);
    }
}
