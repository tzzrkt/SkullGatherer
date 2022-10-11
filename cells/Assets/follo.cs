using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follo : MonoBehaviour
{
    public Transform father;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,father.position,0.5f);
        transform.LookAt(player);
    }
}
