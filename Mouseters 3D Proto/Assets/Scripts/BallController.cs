using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool aiming = false;
    public bool camera_follow = false;
    public float force_per_unity = 750;
    //force per unit in drag vector lenth
    public float force_multiplier = 1;
    Vector3 force_to_add;
    Vector3 mousepos;
    Vector3 mouse_start_pos;
    LineRenderer line;
    Vector3 negative_force_vec;
    // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {   
        var ballpos = this.transform.position;
        if (camera_follow) {
            Camera.main.transform.SetPositionAndRotation(
                new Vector3(ballpos.x,Camera.main.transform.position.y,ballpos.z),
                Camera.main.transform.rotation);
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
            //create vec3 with from ball in screenspace and mouse pos
            // var forceVec = ballpos_screenspace - mousepos;
            // //TRANSLATE 
            // forceVec.x = -forceVec.x;
            // forceVec.z = -forceVec.y;
            // forceVec.y = 0f;
            // var potato = ballpos + forceVec;
            //negative_force_vec = new Vector3(ballpos.x-mousepos.x, 0.0f, ballpos.z-mousepos.y);
            //var endpos = Vector3.Normalize(potato);
            var lineend = new Vector3(mouseWorld.x,ballpos.y,mouseWorld.z);
            line.SetPosition(1,lineend);
            //line.endWidth = 0;
            //Debug.Log("WORLD-ballpos" +ballpos+"    |SCREEN-forceVec"+forceVec+"   |WORLD-dist"+distance+"   |WORLD-mousepos"+mouseWorld);
            var lineLen = Vector3.Distance(ballpos,lineend);
            Debug.Log("distance? "+lineLen);
            if (Input.GetMouseButtonUp(0) == true) {
                aiming = false;
                shoot(Vector3.Normalize(lineend-ballpos), lineLen);
            }
        }
        else {
            line.enabled = false;
        }
    }
    void shoot(Vector3 dir, float force) {
        this.GetComponent<Rigidbody>().AddForce(dir * (force * (force_multiplier * force_per_unity)));
    }

    void OnTriggerEnter (Collider other) {
        if(other.tag == "Hole") {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
    private Vector3 GetMouseAsWorldSpace(float depth) {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("ray origin" +ray.origin+"   |ray dir" +ray.direction+"   |ray" +(ray.origin + ray.direction)+"   |depth" +depth);
        //var screenvec = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //Debug.Log("vec "+screenvec+"   |ray "+ray);
        return ray.origin + ray.direction * depth;
    }
}
