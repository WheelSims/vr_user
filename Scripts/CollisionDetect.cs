using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDetect : MonoBehaviour

{
   private Rigidbody cubeRigidbody;

    public double defaultFriction = 0.0113;
    public double friction;
    public bool collisionfound=false;
  // public UDPSignalSender udpSignalSender;
   

    void Start()
    {
        friction = defaultFriction;
        cubeRigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
      if (collision.gameObject.tag=="Obstacle" || collision.gameObject.tag=="Human" || collision.gameObject.tag == "Target")
      {
           friction=0.25;
           collisionfound=true;
            }
        }
    

     void OnCollisionExit(Collision collisiondone)
    {
        friction = defaultFriction;
           collisionfound=false;
    }
    }
