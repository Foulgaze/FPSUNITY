using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationControl : MonoBehaviour
{
    // Start is called before the first frame update
    
    private void Awake()
    {
       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
    private void Update()
    {
        Debug.Log(gameObject.transform.localPosition);
    }
}
