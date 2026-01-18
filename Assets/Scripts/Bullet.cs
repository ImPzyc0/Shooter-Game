using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] protected float speed;

    public float distance { set; protected get; }
    protected Vector3 start;

    private void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (distance < Vector3.Distance(transform.position, start))
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
    }
}
