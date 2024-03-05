using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour

{
   private Rigidbody cubeRigidbody;

    public double friction=0.0113;
    public bool collisionfound=false;
  // public UDPSignalSender udpSignalSender;
   

    void Start()
    {
        cubeRigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
      
           friction=0.1;
           collisionfound=true;
            
        }
    

     void OnCollisionExit(Collision collisiondone)
    {
         friction=0.0113;
           collisionfound=false;
    }
    }
