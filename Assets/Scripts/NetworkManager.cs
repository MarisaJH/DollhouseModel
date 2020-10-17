using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
//using System.Threading;
using CielaSpike;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*
Based on https://forum.unity.com/threads/simple-udp-implementation-send-read-via-mono-c.15900/
*/

public class NetworkManager : MonoBehaviour
{
    public int port;
    private UdpClient client;

    [Serializable]
    private struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", x, y, z);
        }
        public static implicit operator Vector3(SerializableVector3 val) => new Vector3(val.x, val.y, val.z);
        public static implicit operator SerializableVector3(Vector3 val) => new SerializableVector3(val.x, val.y, val.z);
    }

    [Serializable]
    private struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public override string ToString()
        {
            return String.Format("({0}, {1}, {2}, {3})", x, y, z, w);
        }
        public static implicit operator Quaternion(SerializableQuaternion val) => new Quaternion(val.x, val.y, val.z, val.w);
        public static implicit operator SerializableQuaternion(Quaternion val) => new SerializableQuaternion(val.x, val.y, val.z, val.w);

    }

    [Serializable]
    private struct SerializableTransform
    {
        public SerializableVector3 position;
        public SerializableQuaternion rotation;
    }
    private SerializableTransform robotTransform;
    private byte[] data = null;

    // Start is called before the first frame update
    void Start()
    {
        this.StartCoroutineAsync(ReceiveData());
    }
    
    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.Log(robotTransform.position);
        UnityEngine.Debug.Log(robotTransform.rotation);

        transform.position = robotTransform.position;
        transform.rotation = robotTransform.rotation;
    }
   
    // receive thread
    IEnumerator ReceiveData()
    {
        Debug.Log("in ReceiveData()");
        client = new UdpClient(port);
        while (true)
        {
 
            try
            {
                // get data in bytes
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                data = client.Receive(ref anyIP);
                robotTransform = FromByteArray(data);
               
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
            yield return null;
        }
    }

    private SerializableTransform FromByteArray(byte[] byteArr)
    {
        BinaryFormatter bf = new BinaryFormatter();

        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(byteArr, 0, byteArr.Length);
            ms.Seek(0, SeekOrigin.Begin);
            SerializableTransform rt = (SerializableTransform)bf.Deserialize(ms);
            return rt;
        }
    }
}
