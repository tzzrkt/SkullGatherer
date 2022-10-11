using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addfor : MonoBehaviour
{
    public Transform player;
    public Rigidbody me;
    public float force;
    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 LimitSpeed(Vector3 velocityChange)
    {
        velocityChange = velocityChange.magnitude > maxSpeed ? velocityChange.normalized * maxSpeed : velocityChange;
        velocityChange += new Vector3(0, -.9f, 0);
        return velocityChange;
    }



    private void FixedUpdate()
    {
        Vector3 velocityChange = (player.position - me.position).normalized;
        Vector3 velocity = me.velocity;
        velocityChange = LimitSpeed(velocityChange);


        me.AddForce(velocityChange.normalized*force);


    }
}
