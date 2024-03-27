using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPSignalSender : MonoBehaviour

{
    private string ipAddress = "192.168.0.200"; // IP address of the receiver
    private int port = 25000; // Port to send the data

     public CollisionDetect collisiondetect;
     public double hardwareenable = 0;

     public double wholemass=96;

    private UdpClient udpClient;

    void Start()
    {
        udpClient = new UdpClient(25000);
        hardwareenable = 1;
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
        System.BitConverter.GetBytes(hardwareenable).CopyTo(data, 0);
        System.BitConverter.GetBytes(collisiondetect.friction).CopyTo(data, 8);
        System.BitConverter.GetBytes(collisiondetect.collisionfound).CopyTo(data, 16);
        System.BitConverter.GetBytes(wholemass).CopyTo(data, 20);
        Debug.Log("Data ready ");
        

       // Send data
        udpClient.Send(data, data.Length, ipAddress, port);
        Debug.Log("Sent friction data: " + collisiondetect.friction);
         
         Debug.Log("Sent collision data: " + collisiondetect.collisionfound);
        Debug.Log("Sent Hardware Enable: " + hardwareenable);
    }

   

    void OnApplicationQuit()
    {
      hardwareenable = 0;
       SendData();
    }

     void OnDestroy()
    {
        udpClient.Close();
    }
}
