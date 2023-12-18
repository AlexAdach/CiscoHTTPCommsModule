using CiscoHTTPComms.HTTPObjects;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoHTTPComms
{
    public delegate void HTTPButtonPressReceivedDelegate(SimplSharpString RoomKitMac, SimplSharpString WidgetID);


    public class Module
    {
        private HttpServer _server;
        public ushort DebugEnable { get; set; }

        public HTTPButtonPressReceivedDelegate HTTPButtonPressReceivedHandler { get; set; }


        public Module() 
        {
            CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(_ControllerProgramEventHandler);
        }
        
        /// <summary>
        /// Initialize the module from simpl.
        /// </summary>
        /// <param name="port">Port for the server to use.</param>
        /// <returns></returns>
        public ushort Initialize(ushort port)
        {
            _server = new HttpServer(OnHttpRequestReceived);
            _server.Port = port;
            _server.Open();
            return 1;
        }

        public void OnHttpRequestReceived(object sender, OnHttpRequestArgs args)
        {
            string requestPath = args.Request.Path;
            DebuggerMessage($"Request received. Path: {requestPath}");

            try
            {
                DebuggerMessage(args.Request.ContentString);

                var buttonPress = JsonConvert.DeserializeObject<HTTPButtonPress>(args.Request.ContentString);

                if(buttonPress != null)
                {
                    OnHTTPButtonPressReceived(buttonPress);
                }


                args.Response.Code = 200;
            }
            catch (Exception ex)
            {
                DebuggerMessage($"Error Parsing Message: {ex.Message}");
                args.Response.Code = 500;
            }
        }
        #region SimplPlusCallbacks
        /// <summary>
        /// Alert Simplplus that a roomkit pressed a button.
        /// </summary>
        /// <param name="button"></param>
        private void OnHTTPButtonPressReceived(HTTPButtonPress button)
        {
            HTTPButtonPressReceivedHandler?.Invoke(button.RoomkitMac, button.WidgetID);
        }
        #endregion SimplPlusCallbacks

        #region Debug&Dispose
        /// <summary>
        /// Display a message in toolbox debugger if Debug_Enabled = 1;
        /// </summary>
        /// <param name="message"></param>
        private void DebuggerMessage(string message)
        {
            if(DebugEnable == 1)
                CrestronConsole.PrintLine(message);
        }

        /// <summary>
        /// Used to dispose of managed objects when program stops. 
        /// </summary>
        /// <param name="programStatusEventType"></param>
        void _ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
        {
            switch (programStatusEventType)
            {
                case (eProgramStatusEventType.Paused):
                    //The program has been paused.  Pause all user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Resumed):
                    //The program has been resumed. Resume all the user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Stopping):
                    _server.Dispose();
                    break;
            }

        }
        #endregion Debug&Dispose
    }
}
