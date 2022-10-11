using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detect : MonoBehaviour
{
    public skullcounter skl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "player")
        { skl.touched = true; }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
