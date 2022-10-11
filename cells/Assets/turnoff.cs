using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnoff : MonoBehaviour
{
    public skullcounter skcnt;
    // Start is called before the first frame update
    void Start()
    {
        skcnt = GameObject.Find("master").GetComponent<skullcounter>();   
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "player")
        {
            this.gameObject.SetActive(false);
            skcnt.col++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
