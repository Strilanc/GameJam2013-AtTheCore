using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;


public class CommunicationChannel{
	
	public string remoteIp;
	public int remoteport;
	public string localIp;
	public int localport;
	
	UdpClient udpClient;
	Thread listenThread;
	DataDelegate dataDelegate;
	
	public bool done = false;
	public bool threadLoopStopped = true;
	public bool isInitialized = false;
	
	public bool didError = false;
	
	
	public void sendData(byte[] bytes, int byteSize){
		Debug.Log("SEND");
		if(!isInitialized){ throw new Exception();}
		Debug.Log("SEND2");
		udpClient.BeginSend(bytes,byteSize,null,null);
		Debug.Log("SENT");
	}
	
    private void ReceiveLoop()
    {
		if(!isInitialized){ throw new Exception();}
		threadLoopStopped = false;
		
		Debug.Log("StartListener");
        
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 5004);
        try
        {
            while (!done)
            {
                Debug.Log("Waiting for broadcast");
                byte[] bytes = udpClient.Receive(ref groupEP);
				Debug.Log("Waiting forProcessing");
				//dataDelegate.ProcessBytes(bytes);
               	Debug.Log("Received");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
			didError = true;
        }
        finally
        {
			Debug.Log("Closing UDP Client");
          	udpClient.Close();
			Debug.Log("UDP Client Closed");
			threadLoopStopped = true;
			if(!done){
			 //connect();	
			}
        }
    }
	
	
	
	public void init(string _remoteIp, int _remoteport, string _localIp, int _localport, DataDelegate deleg){
		Debug.Log("IN");
		dataDelegate = deleg;
		
		remoteIp = _remoteIp;
		localIp = _localIp;
		
		localport = _localport;
		remoteport = _remoteport;
		
		bind();
		
		isInitialized = true;
		Debug.Log ("OUT");
	}
	
	public void bind(){
		udpClient = new UdpClient("192.168.12.75",5005);
		
		listenThread = new Thread(new ThreadStart(ReceiveLoop));
		listenThread.Start();
			
	}
	
	public void Destroy(){
		Debug.Log ("Initiate Close");
		done = true;
		listenThread.Abort();
		udpClient.Close();
		Debug.Log ("Closed");
		
	}
	
	
	
}
