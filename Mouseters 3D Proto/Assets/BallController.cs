using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool aiming = false;
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
        Camera.main.transform.SetPositionAndRotation(
            new Vector3(ballpos.x,Camera.main.transform.position.y,ballpos.z),
            Camera.main.transform.rotation);
        if (Input.GetMouseButtonDown(0) == true && (this.GetComponent<Rigidbody>().velocity == Vector3.zero)) {

            aiming = true; // ENTER AIMING MODE
            mouse_start_pos = Input.mousePosition;
        }
        //render line
        if (aiming) {
            //enable line and set its start pos to the ball
            line.enabled = true;
            ballpos = this.transform.position;
            line.SetPosition(0,ballpos);

            //get mouse pos rel to ball
            mousepos = Input.mousePosition;
            var moveDir = mouse_start_pos - mousepos;
            //TRANSLATE TO BALL POSITION
            moveDir.x = -moveDir.x;
            moveDir.z = -moveDir.y;
            moveDir.y = 0f;
            var potato = ballpos + moveDir;
            //negative_force_vec = new Vector3(ballpos.x-mousepos.x, 0.0f, ballpos.z-mousepos.y);
            var endpos = Vector3.Normalize(potato);
            line.SetPosition(1,ballpos + endpos);
            //Debug.Log("ballpos" +ballpos);
            //Debug.Log("endPos" +endpos);
            if (Input.GetMouseButtonUp(0) == true) {
                aiming = false;
                shoot(endpos);
            }
        }
        else {
            line.enabled = false;
        }
    }
    void shoot(Vector3 dir) {
        this.GetComponent<Rigidbody>().AddForce(dir * 500);
    }
}
