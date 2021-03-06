﻿using System;
using System.Net;
using System.Text;
using System.Threading;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// This object represents a UDP server or listener object.
    /// </summary>
    public class UDPListener
    {
        public const int DefaultPortNumber = 22112;

        public const string LocalHostString = "127.0.0.1";

        /// <summary>
        /// A object that represents a networks end point
        /// This object basicly just stops packets from passing by
        /// This object usally has this object checking all UDP packets it gets
        /// so it isn't a very strict endpoint
        /// </summary>
        IPEndPoint EndPoint = null;

        /// <summary>
        /// This object represents the client at the end of the network
        /// acting as a listener
        /// </summary>
        readonly System.Net.Sockets.UdpClient EndpointClient = null;

        /// <summary>
        /// This feild will keep the the listener running until the end unless 
        /// there are any future issues
        /// </summary>
        bool IsConnected = false;

        /// <summary>
        /// This will contain the object that can recive messages from this
        /// object while it is operating
        /// </summary>
        IDisplayMessage messageSystem = null;

        /// <summary>
        /// A lock to create a queue for messages if they are recived 
        /// in rapid succession. found in the responce function.
        /// </summary>
        private static object ResponceLock = new object();

        /// <summary>
        /// The default constuctor for hte UDP listener
        /// </summary>
        public UDPListener()
        {
            // Set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // set up the end point client
            this.EndpointClient = new System.Net.Sockets.UdpClient(DefaultPortNumber);
        }

        /// <summary>
        /// a version of the default constuctor that allows for messages to be sent to the UI
        /// </summary>
        /// <param name="messageSystem">
        /// This will be a object that has implemented the IDisplayMessage interface. 
        /// This object will beable to recive messages created by this sytem at run time.
        /// </param>
        public UDPListener(IDisplayMessage messageSystem)
        {
            // set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // The object that will be used to transmit messages
            this.messageSystem = messageSystem;

            // The endpoint client that will be used for all network functionality in this class
            this.EndpointClient = new System.Net.Sockets.UdpClient(DefaultPortNumber);
        }

        /// <summary>
        /// Allows the user to adjust the port number and have a message system
        /// </summary>
        /// <param name="portNumber">
        /// A int repesention of the port number this protocol will use
        /// </param>
        /// <param name="messageSystem">
        /// The message sytem that this system will use
        /// </param>
        public UDPListener(int portNumber, IDisplayMessage messageSystem)
        {
            // set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // The object that will be used to transmit messages
            this.messageSystem = messageSystem;

            // The endpoint client that will be used for all network functionality in this class
            this.EndpointClient = new System.Net.Sockets.UdpClient(portNumber);
        }

        /// <summary>
        /// Starts the object and creates a new thread 
        /// dedicated to listening
        /// </summary>
        public void Start()
        {
            // start a listening thread for the object
            IsConnected = true;
            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();
        }

        /// <summary>
        /// triggered from the start funciton and run in it's own thread.
        /// </summary>
        private void Listen()
        {
            // keep looping over the listener code until the IsConnnected veriable
            // changes or a exception is triggered
            try
            {
                while (this.IsConnected)
                {
                    Byte[] receiveBytes = this.EndpointClient.Receive(ref this.EndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    // if there is a message outlet send the data there
                    if (messageSystem != null)
                    {
                        messageSystem.DisplayMessage(MessageHelper.MessageType.Data, returnData);
                    }

                    // will activiate the responder if nessarcary 
                    Responce(returnData, ref this.EndPoint);
                }
            }
            catch (Exception ex)
            {
                if (messageSystem != null)
                {
                    this.messageSystem.DisplayMessage(MessageHelper.MessageType.Exception, "Exception: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// this method is triggered by the listen function. it's main job is to saftly call
        /// the respond mehtod if it exists.
        /// </summary>
        /// <param name="dataRecived">
        /// this is a string represention of the 
        /// </param>
        /// <param name="endpoint">
        /// Details about the other side of the coversation
        /// </param>
        private void Responce(string dataRecived, ref IPEndPoint endpoint)
        {
            //create a client to send the data back to
            //System.Net.Sockets.UdpClient client = new System.Net.Sockets.UdpClient(endpoint.Port, endpoint.AddressFamily);

            // create a lock on this so there are no collisons or erros
            lock (ResponceLock)
            {
                // Code to echo back a responce
                byte[] data = Encoding.Unicode.GetBytes(dataRecived);
                this.EndpointClient.Send(data, data.Length, endpoint);
            }
        }

        /// <summary>
        /// Stops the listener and deactivates the method
        /// </summary>
        public void Stop()
        {
            this.IsConnected = false;

            if (EndpointClient != null)
            {
                this.EndpointClient.Close();
            }

        }
    }
}

