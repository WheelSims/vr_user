using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using TMPro;

public class UDPSignalReceiver : MonoBehaviour
{
    private int port = 25100; // Port to listen on
    private UdpClient udpClient;
  
    // Define velocities

    public double linearVelocity=0;
    public double angularVelocity=0;
    private double currentincrementindex;
    private double lastincrementindex;
    private double emergencyStop;
    public TMP_Text speedText;
    

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
        currentincrementindex = System.BitConverter.ToDouble(receivedBytes, 0);
        angularVelocity = System.BitConverter.ToDouble(receivedBytes, 8);
        linearVelocity = System.BitConverter.ToDouble(receivedBytes,16);
        emergencyStop= System.BitConverter.ToDouble(receivedBytes,24);

        // Try to avoid jitter
        double thresholdlinear = 0.02;
        double thresholdangular = 0.01;

        if (linearVelocity > -thresholdlinear && linearVelocity < thresholdlinear)
            linearVelocity = 0.0;

        if (angularVelocity > -thresholdangular && angularVelocity < thresholdangular)
            angularVelocity = 0.0;

        Debug.Log("Emergency Stop " + emergencyStop);


        // Debug.Log("Received linear velocity data: " + linearVelocity+"Received angular velocity data: " + angularVelocity);

        
       

        // Continue listening for the next UDP packet
        udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
      
    }
    // Update is called once per frame
    void Update()
    {
        if (currentincrementindex != lastincrementindex)
        {
                      
            // Update the speed of the cube

            // Rotate around y - axis
            transform.Rotate(0, (float)(angularVelocity * Time.fixedDeltaTime*(-180/Mathf.PI)), 0,Space.World);

            // Move forward / backward
           transform.Translate(0, 0, (float)(linearVelocity * Time.fixedDeltaTime));
           

            lastincrementindex = currentincrementindex;
        }
        else if (currentincrementindex == lastincrementindex)
            {
                // Make the speed of the cube zero

                // Rotate around y - axis
                transform.Rotate(0, 0, 0, Space.World);

             // Move forward / backward
            transform.Translate(0, 0,0);
              // stop DBox
            

            lastincrementindex = currentincrementindex;
            }
            
             speedText.SetText(linearVelocity.ToString("0.00")+" m/s");

             if (emergencyStop==1)
             {
             //Application.Quit();
             UnityEditor.EditorApplication.isPlaying = false;
             }

    }
        
  
    private void OnDestroy()
    {
                udpClient.Close();
    }

    
}
