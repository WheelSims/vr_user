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

     public double wholeMass=94;
    public double wheelDistance = 0.60;
    public bool forceReset = false;

    private UdpClient udpClient;

      double previousFriction;
    bool previousCollisionFound;
    double previousWholeMass;
    double previousHardwareEnable;
    double previousWheelDistance;
    bool previousForceReset;

    void Start()
    {
        udpClient = new UdpClient(25000);
        hardwareEnable = 1;
        forceReset = true;
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
        byte[] data = new byte[40]; // One Float (8*4Bytes) and two Boolean Data (8Bytes)
        System.BitConverter.GetBytes(hardwareEnable).CopyTo(data, 0);
        System.BitConverter.GetBytes(collisiondetect.friction).CopyTo(data, 8);
        System.BitConverter.GetBytes(collisiondetect.collisionfound).CopyTo(data, 16);
        System.BitConverter.GetBytes(wholeMass).CopyTo(data, 20);
        System.BitConverter.GetBytes(wheelDistance).CopyTo(data, 28);
        System.BitConverter.GetBytes(forceReset).CopyTo(data, 36);
        Debug.Log("Data ready ");
        
 if (previousFriction != collisiondetect.friction ||
            previousCollisionFound != collisiondetect.collisionfound ||
            previousWholeMass != wholeMass ||
            previousHardwareEnable != hardwareEnable ||
            previousWheelDistance != wheelDistance ||
            previousForceReset != forceReset)
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
            previousWheelDistance = wheelDistance;
            previousForceReset = forceReset;


        }
    }

   

    void OnApplicationQuit()
    {
      hardwareEnable = 0;
      forceReset = false;
       SendData();
    }

     void OnDestroy()
    {
                  udpClient.Close();
    }

   
}
