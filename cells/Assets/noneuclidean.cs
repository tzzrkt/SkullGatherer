using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noneuclidean : MonoBehaviour
{
    public Rigidbody rb;
    public Transform maxX;
    public Transform minX;
    public Transform maxZ;
    public Transform minZ;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.position.x > maxX.position.x) { rb.position = new Vector3(minX.position.x + 1, rb.position.y, rb.position.z); }
        if (rb.position.x < minX.position.x) { rb.position = new Vector3(maxX.position.x - 1, rb.position.y, rb.position.z); }
        if (rb.position.z > maxZ.position.z) { rb.position = new Vector3(rb.position.x, rb.position.y, minZ.position.z + 1 ); }
        if (rb.position.z < minZ.position.z) { rb.position = new Vector3(rb.position.x, rb.position.y, maxZ.position.z - 1); }
    }
}
