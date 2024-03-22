using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPSignalSender : MonoBehaviour

{
    private string ipAddress = "192.168.0.200"; // IP address of the receiver
    private int port = 25000; // Port to send the data

     public CollisionDetect collisiondetect;

    private UdpClient udpClient;

    void Start()
    {
        udpClient = new UdpClient(25000);
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
        byte[] data = new byte[12]; // One Float (8Bytes) and one Boolean Data (4Bytes)
        System.BitConverter.GetBytes(collisiondetect.friction).CopyTo(data, 0);
        System.BitConverter.GetBytes(collisiondetect.collisionfound).CopyTo(data, 8);
        Debug.Log("Data ready ");
        

        // Send data
        udpClient.Send(data, data.Length, ipAddress, port);
        Debug.Log("Sent friction data: " + collisiondetect.friction);
         
         Debug.Log("Sent collision data: " + collisiondetect.collisionfound);
    }

    void OnDestroy()
    {
        udpClient.Close();
    }
}
