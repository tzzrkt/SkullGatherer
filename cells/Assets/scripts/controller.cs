using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float maxSpeed;
    public Transform vecc;
    public Vector3 dir;
    public float x;
    public float z;
    public Transform mesh;
    public bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.MoveRotation(Quaternion.Euler(0, 45, 0));
    }
    public void Update()
    {
        if (rb.velocity != Vector3.zero) { isMoving = true; }
        else { isMoving = false; }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 origen = transform.position;

        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") + Input.GetAxis("Vertical"), 0, Input.GetAxis("Vertical") - Input.GetAxis("Horizontal")).normalized*speed;

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange = LimitSpeed(velocityChange);

        rb.AddForce(velocityChange.normalized, ForceMode.VelocityChange);

        if (targetVelocity == Vector3.zero)
        { rb.velocity = Vector3.zero; }

        mesh.transform.forward = Vector3.Lerp(mesh.transform.forward, new Vector3(rb.velocity.x, 0, rb.velocity.z), .75f);
    }


    private Vector3 LimitSpeed(Vector3 velocityChange)
    {
        velocityChange = velocityChange.magnitude > maxSpeed ? velocityChange.normalized * maxSpeed : velocityChange;
        velocityChange += new Vector3(0, -.9f, 0);
        return velocityChange;
    }
}
