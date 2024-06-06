using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPSignalSender : MonoBehaviour

{
    private string ipAddress = "192.168.0.200"; // IP address of the receiver
    private int port = 25000; // Port to send the data

     public CollisionDetect collisiondetect;
     public double hardwareEnable = 0;

     public double wholeMass=96;

    private UdpClient udpClient;

      double previousFriction;
    bool previousCollisionFound;
    double previousWholeMass;
    double previousHardwareEnable;

    void Start()
    {
        udpClient = new UdpClient(25000);
        hardwareEnable = 1;
        SendData();
        Debug.Log("Start sending data ");
    }

    // Update is called once per frame
    void Update()
    {
        SendData();
    }
    void SendData()
    {
        // Serialize Data
        byte[] data = new byte[28]; // One Float (8*3Bytes) and one Boolean Data (4Bytes)
        System.BitConverter.GetBytes(hardwareEnable).CopyTo(data, 0);
        System.BitConverter.GetBytes(collisiondetect.friction).CopyTo(data, 8);
        System.BitConverter.GetBytes(collisiondetect.collisionfound).CopyTo(data, 16);
        System.BitConverter.GetBytes(wholeMass).CopyTo(data, 20);
        Debug.Log("Data ready ");
        
 if (previousFriction != collisiondetect.friction ||
            previousCollisionFound != collisiondetect.collisionfound ||
            previousWholeMass != wholeMass ||
            previousHardwareEnable != hardwareEnable)
        {
       // Send data
        udpClient.Send(data, data.Length, ipAddress, port);
        Debug.Log("Sent friction data: " + collisiondetect.friction);
         
         Debug.Log("Sent collision data: " + collisiondetect.collisionfound);
        Debug.Log("Sent Hardware Enable: " + hardwareEnable);

         // Update previous data
            previousFriction = collisiondetect.friction;
            previousCollisionFound = collisiondetect.collisionfound;
            previousWholeMass = wholeMass;
            previousHardwareEnable = hardwareEnable;
           
        }
    }

   

    void OnApplicationQuit()
    {
      hardwareEnable = 0;
       SendData();
    }

     void OnDestroy()
    {
                  CloseUDPClient();
    }

    void CloseUDPClient()
     {
        if (udpClient!=null)
        {
            try
            {
               
                udpClient.Close();
                udpClient.Dispose();
                udpClient=null;
            }
            catch (SocketException ex)
            {
                 Debug.LogError("Error closing UDP Client:"+ ex.Message);
            }
        }
     }
}
