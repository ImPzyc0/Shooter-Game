using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : Movement
{

    public bool jump, changeRot = false;
    public float side, forward = 0;
    public float xRot, yRot = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
   

    protected override void GetMovementInput()
    {
        if(changeRot) transform.eulerAngles = new Vector3(xRot, yRot, transform.eulerAngles.z);
        changeRot = false;
        
        space = jump & gravityScript.grounded;
        
        velocity.x = speed * side;
        
        velocity.z = velocity.x == 0 && forward  > 0 ? oneDirectionalSpeed * forward : speed * forward;

        if (space) jump = false;
    }
}
