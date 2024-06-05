using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UDPSignalSender2 : MonoBehaviour

{
    private string ipAddress = "192.168.0.200"; // IP address of the receiver
    private int port = 25000; // Port to send the data

     public CollisionDetect collisiondetect;
     public double hardwareEnable = 0;

     public double wholeMass=96;

    public double wheelDistance = 0.5325;

    public double modeControl;

    public double targetReach=0;

    private UdpClient udpClient;


    double previousFriction;
    bool previousCollisionFound;
    double previousWholeMass;
    double previousHardwareEnable;
    double previousWheelDistance;
    double previousModeControl;
    double previousTargetReach;

    void Start()
    {
        udpClient = new UdpClient(25000);
        hardwareEnable = 1;
       
        // Check the active scene and set mode accordingly
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == "IRGLM_Froll_Measurment")
        {
            modeControl = 0;
        }
        else
        {
            modeControl = 1;
        }

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
        byte[] data = new byte[52]; // 6 double (6*8 Bytes) and one Boolean Data (4Bytes)
        System.BitConverter.GetBytes(hardwareEnable).CopyTo(data, 0);
        System.BitConverter.GetBytes(collisiondetect.friction).CopyTo(data, 8);
        System.BitConverter.GetBytes(collisiondetect.collisionfound).CopyTo(data, 16);
        System.BitConverter.GetBytes(wholeMass).CopyTo(data, 20);
        System.BitConverter.GetBytes(wheelDistance).CopyTo(data, 28);
        System.BitConverter.GetBytes(modeControl).CopyTo(data, 36);
        System.BitConverter.GetBytes(targetReach).CopyTo(data, 44);
        Debug.Log("Data ready ");

        // Check if data has changed
        if (previousFriction != collisiondetect.friction ||
            previousCollisionFound != collisiondetect.collisionfound ||
            previousWholeMass != wholeMass ||
            previousHardwareEnable != hardwareEnable ||
            previousWheelDistance != wheelDistance ||
            previousModeControl != modeControl ||
            previousTargetReach != targetReach)
        {
            // Send data
            udpClient.Send(data, data.Length, ipAddress, port);
            Debug.Log("Sent Hardware Enable: " + hardwareEnable);

            // Update previous data
            previousFriction = collisiondetect.friction;
            previousCollisionFound = collisiondetect.collisionfound;
            previousWholeMass = wholeMass;
            previousHardwareEnable = hardwareEnable;
            previousWheelDistance = wheelDistance;
            previousModeControl = modeControl;
            previousTargetReach = targetReach;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {

            targetReach = 1;
        }
    }


    void OnApplicationQuit()
    {
      hardwareEnable = 0;
        targetReach = 0;
        SendData();
    }

     void OnDestroy()
    {
        udpClient.Close();
    }
}
