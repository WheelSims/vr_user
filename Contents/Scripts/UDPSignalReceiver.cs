using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;

public class UDPSignalReceiver : MonoBehaviour
{
    private int port = 25100; // Port to listen on
    private UdpClient udpClient;
  
    // Define velocities

    public double linearVelocity=0;
    public double angularVelocity=0;

    void Start()
    {
        udpClient = new UdpClient(port);
        print("Waiting for broadcast...");
        udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
        print("Receiving Data...");

       
    }

    private void ReceiveCallback(System.IAsyncResult ar)
    {
          IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
          byte[] receivedBytes = udpClient.EndReceive(ar, ref remoteEndPoint);

    // Convert byte data to Velocity
    
    angularVelocity = System.BitConverter.ToDouble(receivedBytes, 0);
    linearVelocity = System.BitConverter.ToDouble(receivedBytes, 8);
    // Try to avoid jitter
            double thresholdlinear = 0.1;
            double thresholdangular = 0.1;

            if (linearVelocity > -thresholdlinear && linearVelocity < thresholdlinear)
                linearVelocity = 0.0;

            if (angularVelocity > -thresholdangular && angularVelocity < thresholdangular)
                angularVelocity = 0.0;




        // Debug.Log("Received linear velocity data: " + linearVelocity+"Received angular velocity data: " + angularVelocity);

        
       

        // Continue listening for the next UDP packet
        udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
      
    }
    // Update is called once per frame
    void FixedUpdate()
    {
      

           // Rotate around y - axis
        transform.Rotate(0, (float)(angularVelocity * Time.fixedDeltaTime*(-180/Mathf.PI)), 0,Space.World);

        // Move forward / backward
        transform.Translate(0, 0, (float)(linearVelocity * Time.fixedDeltaTime));
    }

    private void OnDestroy()
    {
        udpClient.Close();
    }
}
