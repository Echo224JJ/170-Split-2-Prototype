using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricButton : MonoBehaviour
{
    public BallController bc;
    public void OnButtonPress()
    {
        if (bc.electricAbility == true)
        {
            bc.electricAbility = false;
        }
        else if (bc.electricAbility == false)
        {
            bc.electricAbility = true;
        }
    }
}
