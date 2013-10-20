using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;

public class UdpAsyncConnection
{

	private Socket udpSock;
	private byte[] buffer;
	
	
	private string remoteIp;
	private int remotePort;
	
	private string localIp;
	private int localPort;
	
	
	private DataDelegate dataDelegate;
	
	public void setCallBackDelegate(DataDelegate _delegate){
		dataDelegate = _delegate;
	}
	
	public static void SendCallback(IAsyncResult ar)
	{	
  		UdpClient u = (UdpClient)ar.AsyncState;

		Debug.Log("number of bytes sent:");
		u.EndSend(ar);
  		//messageSent = true;
	}
	
	public void sendData(byte[] _bytes, int byteSize){
		
		udpSock.BeginSendTo(_bytes,0,byteSize,SocketFlags.None,new IPEndPoint(IPAddress.Parse(remoteIp),remotePort), new AsyncCallback(SendCallback),udpSock);
	}
	public void sendData(byteArrayMetaData bamd){
		
		sendData (bamd.bytes,bamd.bytesize);		
	}

	public void Initialize (String _remoteIp, int _remotePort, int _localPort)
	{
		
		Debug.Log ("UAC CTOR");
		localPort = _localPort;
		
		remoteIp = _remoteIp;
		remotePort = _remotePort;
		//Setup the socket and message buffer
		udpSock = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		udpSock.Bind (new IPEndPoint (IPAddress.Any, localPort));
		buffer = new byte[1024];

		//Start listening for a new message.
		EndPoint newClientEP = new IPEndPoint (IPAddress.Any, 0);
		udpSock.BeginReceiveFrom (buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, udpSock);
	}

	private void DoReceiveFrom (IAsyncResult iar)
	{
		try {
			//Get the received message.
			Socket recvSock = (Socket)iar.AsyncState;
			EndPoint clientEP = new IPEndPoint (IPAddress.Any, 0);
			int msgLen = recvSock.EndReceiveFrom (iar, ref clientEP);
			byte[] localMsg = new byte[msgLen];
			Array.Copy (buffer, localMsg, msgLen);

			//Start listening for a new message.
			EndPoint newClientEP = new IPEndPoint (IPAddress.Any, 0);
			udpSock.BeginReceiveFrom (buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, udpSock);

			//Handle the received message
			Debug.Log ("### REcieve");
			if(dataDelegate != null){
				dataDelegate.ProcessBytes(localMsg,localMsg.Length);
			}else{
				Debug.Log ("NO RECV DELEGATE SET");
			}	
			
		} catch (ObjectDisposedException) {
			//expected termination exception on a closed socket.
			// ...I'm open to suggestions on a better way of doing this.
			udpSock.Close ();
		}
	}
	
}