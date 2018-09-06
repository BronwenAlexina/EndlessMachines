using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;

static public class NetworkClient// : MonoBehaviour
{

    //static public GameObject info;
    static public Sprite infoText, findingMatchText, findingServerText;

    static int connectionID;
    static int maxConnections = 100;
    static int reliableChannelID;
    static int unreliableChannelID;
    static int hostID;
    static int socketPort = 5701;
    static byte error;

    static bool isConnected = false;

    static int ourClientID;

    static string filePath = Application.persistentDataPath + "/UniqueID.txt";
    static int uniqueID;

    static float lastConnectTry;

    //// Use this for initialization
    //void Start()
    //{

    //}

    // Update is called once per frame
    public static void Update()
    {

        if (!isConnected)
        {
            if (Time.time - lastConnectTry > 15)
                Connect();
        }

        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    //Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;

                    if (uniqueID == 0)
                        SendMessageToServer(ClientNetworkRequests.CreateUniqueID + "");
                    else
                        SendMessageToServer(ClientNetworkRequests.BindUniqueID + "," + uniqueID);

                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    // Debug.Log("got msg = " + msg);
                    ProcessRecievedMsg(msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    //Debug.Log("disconnected.  " + recConnectionID);
                    isConnected = false;
                    lastConnectTry = Time.time;
                    break;
            }
        }
    }

    public static void Connect()
    {
        lastConnectTry = Time.time;
        //NetworkTransport.

        //NetworkServer.Reset();

        if (!isConnected)
        {

            //File.Delete(filePath);

            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line = null;

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] procStr = line.Split(',');

                        if (procStr[0] == "0")
                            uniqueID = System.Convert.ToInt32(procStr[1]);
                    }
                }

                //Debug.Log("Unique ID loaded " + uniqueID);
            }

            // Debug.Log("Finding Server");

            //info.GetComponent<SpriteRenderer>().sprite = findingServerText;

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            //Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "142.112.20.199", 5701, 0, out error);//"127.0.0.1"//192.168.2.11

            // Debug.Log(connectionID);

            isConnected = true;

            if (error == 0)
                ;//Debug.Log("Connected");
            else
            {
                isConnected = false;
                //Debug.Log("gdsgerghwerhger");
            }

        }
    }

    public static void Disconnect()
    {
        isConnected = false;
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }

    private static void SendMessageToServer(string msg)
    {
        // msg = uniqueID + "," + Time.fixedTime + "," + msg;

        if (isConnected)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(msg);
            NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
        }
    }

    public static void SendToServer(string msg)
    {
        SendMessageToServer(ClientNetworkRequests.DataToSave + "," + msg);
    }

    private static void ProcessRecievedMsg(string msg)//, int id)
    {
        //Debug.Log("msg recieved = " + msg + ".");//  connection id = " + id);

        string[] procStr = msg.Split(',');

        if (procStr[0] == "0")
        {
            uniqueID = System.Convert.ToInt32(procStr[1]);

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("0," + uniqueID);
            }
        }

    }

}


static public class ClientNetworkRequests
{
    public const int CreateUniqueID = 1;
    public const int BindUniqueID = 2;
    public const int DataToSave = 3;
}