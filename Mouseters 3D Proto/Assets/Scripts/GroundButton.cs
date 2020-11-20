using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : MonoBehaviour
{
    public BallController bc;
    public void OnButtonPress()
    {
        if(bc.groundAbility == true)
        {
            bc.groundAbility = false;
        }
        else if (bc.groundAbility == false)
        {
            bc.groundAbility = true;
        }
    }
}
