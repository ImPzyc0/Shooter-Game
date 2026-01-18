using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Movement")]

    [SerializeField] protected float speed;
    [SerializeField] protected float oneDirectionalSpeed;
    [SerializeField] protected CharacterController ccontroller;

    [Header("Jumping")]
    [SerializeField] protected Gravity gravityScript;
    [SerializeField] protected float jumpPower;

    protected Vector3 velocity;
    protected bool space;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //Get the velocity, moving forwards is always faster so if there is no side movement, velocity.z (forward) will be higher
        GetMovementInput();
        
        //Move acordingly
        ccontroller.Move(transform.forward * velocity.z * Time.fixedDeltaTime);
        ccontroller.Move(transform.right * velocity.x * Time.fixedDeltaTime);

        //If it is jumping, set the gravity scripts velocity 
        if(space)
        {
            gravityScript.velocity = jumpPower;
        }
		space = false;
        
        ClientSend.PlayerMovement(transform.position, transform.rotation);
    }

	void Update(){
		if(space) return;
		space = Input.GetKeyDown(KeyCode.Space) & gravityScript.grounded;
	}

    protected virtual void GetMovementInput()
    {
        velocity.x = speed * Input.GetAxis("Horizontal");
        
        velocity.z = velocity.x == 0 && Input.GetAxis("Vertical")  > 0 ? oneDirectionalSpeed * Input.GetAxis("Vertical") : speed * Input.GetAxis("Vertical");
        
    }

    public Vector3 GetVelocity()
    {
        //return new Vector3(0, 0, 0);
        return new Vector3(velocity.x, gravityScript.velocity, velocity.z);
    }
}
