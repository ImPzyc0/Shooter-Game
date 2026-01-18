using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] CharacterController ccontroller;
    [SerializeField] float gravityCheckRange;
    [SerializeField] float gravity = -9.81f;

    public float velocity { get; set; } = 0;

    public bool grounded { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hitInfo, gravityCheckRange);
        Debug.DrawRay(this.transform.position, Vector3.down, Color.green, 200);
        if(grounded && velocity <= 0)
        {
            velocity = 0;
            return;
        }

        //https://www.youtube.com/watch?v=yGhfUcPjXuE explains it well 
        velocity += gravity * Time.deltaTime * 0.5f;

        ccontroller.Move(new Vector3(0, velocity * Time.deltaTime, 0));

        velocity += gravity * Time.deltaTime * 0.5f;
    }
}
