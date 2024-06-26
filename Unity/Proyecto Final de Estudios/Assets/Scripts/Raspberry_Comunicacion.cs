using System;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Text;
using System.Threading;

public class Raspberry_Comunicacion : MonoBehaviour
{
    private TcpClient ClientControl,ClientImagen,ClientNavegacion;
    private string hostIP;
    private int PortControl,PortImagen,PortNavegacion;
    private NetworkStream NetStreamControl,NetStreamImagen,NetStreamNavegacion;
    private float SendClock = 0;
    private float SendTimer = 0.1f;
    private float JoinTimer = 0;
    public bool IsConnected = false;
    private bool IsConnecting = false;
    public string NavegacionDatos = "E;";
    private Thread ControlThread;
    private Thread ImagenThread;
    private Thread NavegacionThread;
    private Thread ReconnectThread;
    private bool ImageReady = false;
    public bool DataIsReady = false;
    public byte[] BytesImagen= new byte[0];
    public Mutex MutexImagen = new Mutex();
    public Mutex MutexNavegacion = new Mutex();
    public Mutex MutexControl = new Mutex();
    public Mutex MutexConnect = new Mutex();
    private bool ControlComReady = false;
    private string message;

    // Start is called before the first frame update
    void Start()
    {
        //Se definen las variables que definen al servidor en la raspberry
        ClientControl = new TcpClient();
        ClientImagen = new TcpClient();
        ClientNavegacion = new TcpClient();
        hostIP = "10.42.0.1";     //IP Raspberry
        PortControl = 40000;
        PortImagen = 40001;
        PortNavegacion = 40002;
        IsConnecting = true;
        //Se prepara el thread que conecta el cliente
        ThreadStart FunctionReconnect = new ThreadStart(Reconnect);
        ReconnectThread = new Thread(FunctionReconnect);
        ReconnectThread.Start();
    }

    void Connect()
    {
        try
        {
            //Se conectan los clientes
            ClientControl.Connect(hostIP, PortControl);
            ClientImagen.Connect(hostIP, PortImagen);
            ClientNavegacion.Connect(hostIP, PortNavegacion);
            MutexConnect.WaitOne();
            if (IsConnecting)
            {
                IsConnected = true;
            }
            MutexConnect.ReleaseMutex();
            //Se preparan los network stream
            NetStreamControl = ClientControl.GetStream();
            NetStreamImagen = ClientImagen.GetStream();
            NetStreamNavegacion = ClientNavegacion.GetStream();
            //Preparamos la función para asignar al thread
            ThreadStart FunctionControl = new ThreadStart(Control);
            ThreadStart FunctionImagen = new ThreadStart(Imagen);
            ThreadStart FunctionNavegacion = new ThreadStart(Navegacion);
            //Se crean los threads y le asignamos la función
            ControlThread = new Thread(FunctionControl);
            ImagenThread = new Thread(FunctionImagen);
            NavegacionThread = new Thread(FunctionNavegacion);
            //Se inician los threads
            ControlThread.Start();
            ImagenThread.Start();
            NavegacionThread.Start();
            JoinTimer = 0;
        }
        catch (Exception)
        {
            IsConnected = false;
        }
        IsConnecting = false;
    }

    void Control()
    {
        while (IsConnected)
        {
            if (SendClock > SendTimer)
            {
                if (!SocketConnected(ClientControl))
                {
                    IsConnected = false;
                }
                else
                {
                    try
                    {
                        ControlComReady = true;
                        while (ControlComReady) { }
                        //Se envia el mensaje
                        byte[] SendBytes = Encoding.UTF8.GetBytes(message);
                        NetStreamControl.Write(SendBytes, 0, SendBytes.Length);
                        SendClock = 0;
                    }
                    catch (Exception)
                    {
                        IsConnected = false;
                    }
                }
            }
        }
    }

    void Imagen()
    {
        byte[] Buffer = new byte[0];
        byte[] RecBytes = new byte[ClientImagen.ReceiveBufferSize];
        while (IsConnected)
        {
            if (!SocketConnected(ClientImagen))
            {
                IsConnected = false;
            }
            else
            {
                try
                {
                    //Se recibe el mensaje
                    if (NetStreamImagen.DataAvailable)
                    {
                        int BytesRecibidos = NetStreamImagen.Read(RecBytes, 0, RecBytes.Length);
                        byte[] Recibidos = new byte[1048576];
                        Array.Copy(RecBytes, Recibidos, BytesRecibidos);
                        Array.Resize(ref Recibidos, BytesRecibidos);
                        Buffer = Buffer.Concat(Recibidos).ToArray();
                        if (Encoding.UTF8.GetString(Buffer, Buffer.Length - 1, 1) == ";")
                        {
                            MutexImagen.WaitOne();
                            ImageReady = true;
                            BytesImagen = Buffer;
                            MutexImagen.ReleaseMutex();
                            Array.Resize(ref Buffer, 0);
                        }
                    }
                }
                catch (Exception)
                {
                    IsConnected = false;
                }
            }
        }
    }

    void Navegacion()
    {
        string Buffer = "";
        byte[] RecBytes = new byte[ClientNavegacion.ReceiveBufferSize];
        while (IsConnected)
        {
            if (! SocketConnected(ClientNavegacion))
            {
                IsConnected = false;
            }
            else
            {
                try
                {
                    //Se recibe el mensaje
                    if (NetStreamNavegacion.DataAvailable)
                    {
                        int BytesRecibidos = NetStreamNavegacion.Read(RecBytes, 0, RecBytes.Length);
                        Buffer = Buffer + Encoding.UTF8.GetString(RecBytes, 0, BytesRecibidos);
                        if (Buffer[Buffer.Length - 1] == ';')
                        {
                            MutexNavegacion.WaitOne();
                            NavegacionDatos = Buffer;
                            MutexNavegacion.ReleaseMutex();
                            Buffer = "";
                        }
                    }
                }
                catch (Exception)
                {
                    IsConnected = false;
                }
            }
        }
    }

    void Reconnect()
    {
        //Se desconectan los posibles clientes que no se hayan desconectado y se reconecta
        IsConnecting = true;
        try
        {       
            ControlThread.Join();
            ImagenThread.Join();
            NavegacionThread.Join();
        }
        catch (Exception) {
            IsConnected = false;
        }
        Connect();
    }

    public bool ImageIsReady()
    {
        if (ImageReady)
        {
            ImageReady = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Se controla si hay conexion y se da un timeout
        if (IsConnected == false && IsConnecting == false)
        {
            JoinTimer = JoinTimer + Time.deltaTime;
            if (JoinTimer > 5)
            {
                ThreadStart FunctionReconnect = new ThreadStart(Reconnect);
                ReconnectThread = new Thread(FunctionReconnect);
                ReconnectThread.Start();
                JoinTimer = 0;
            }
        }
        //Se lee el comando y se reinicia el timer
        else
        {
            if (ControlComReady)
            {
                MutexControl.WaitOne();
                message = GameObject.Find("Lector_USB").GetComponent<Lector_USB>().LeerComando();
                MutexControl.ReleaseMutex();
                ControlComReady = false;
            }
            SendClock = SendClock + Time.deltaTime;
            JoinTimer = 0;
        }
    }

    bool SocketConnected(TcpClient s)
    {
        bool B3 = s.Connected;                                  // True si la conexion se realizo con exito en primer lugar
        return B3;
    }

    void OnApplicationQuit()
    {
        MutexConnect.WaitOne();
        if (IsConnected)
        {
            //Se da la orden de desconexion y se cierran los threads
            IsConnecting = false;
            IsConnected = false;
            MutexConnect.ReleaseMutex();
            ReconnectThread.Join();
            ControlThread.Join();
            ImagenThread.Join();
            NavegacionThread.Join();
        }
        else
        {
            IsConnecting = false;
            IsConnected = false;
            MutexConnect.ReleaseMutex();
        }
        
    }
}

