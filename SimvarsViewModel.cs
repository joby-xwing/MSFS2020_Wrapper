using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Net;
using System.Net.Sockets;

using Microsoft.FlightSimulator.SimConnect;
using System.Text;
using System.Reflection;

namespace Simvars
{
    public enum DEFINITION
    {
        Dummy = 0
    };

    public enum REQUEST
    {
        Dummy = 0
    };
    public enum DATA_DEFINE_ID
    {
        INTRUDER_1,
        INTRUDER_REQ,
        INTRUDER_LLA
    };

    public class SimvarRequest : ObservableObject
    {
        public DEFINITION eDef = DEFINITION.Dummy;
        public REQUEST eRequest = REQUEST.Dummy;
        public uint eObjectID = 0;  //0 corresponds to user

        public string sName { get; set; }

        public double dValue
        {
            get { return m_dValue; }
            set { this.SetProperty(ref m_dValue, value); }
        }
        private double m_dValue = 0.0;

        public string sUnits { get; set; }

        public bool bPending = true;
        public bool bStillPending
        {
            get { return m_bStillPending; }
            set { this.SetProperty(ref m_bStillPending, value); }
        }
        private bool m_bStillPending = false;
    };

    public class SimvarsViewModel : BaseViewModel, IBaseSimConnectWrapper
    {
        #region IBaseSimConnectWrapper implementation

        /// User-defined win32 event
        public const int WM_USER_SIMCONNECT = 0x0402;

        /// Window handle
        private IntPtr m_hWnd = new IntPtr(0);

        /// SimConnect object
        private SimConnect m_oSimConnect = null;

        public bool bConnected
        {
            get { return m_bConnected; }
            private set { this.SetProperty(ref m_bConnected, value); }
        }
        private bool m_bConnected = false;

        private uint m_iCurrentDefinition = 0;
        private uint m_iCurrentRequest = 0;

        public int GetUserSimConnectWinEvent()
        {
            return WM_USER_SIMCONNECT;
        }

        public void ReceiveSimConnectMessage()
        {
            m_oSimConnect?.ReceiveMessage();
        }

        public void SetWindowHandle(IntPtr _hWnd)
        {
            m_hWnd = _hWnd;
        }

        public void Disconnect()
        {
            Console.WriteLine("Disconnect");

            m_oTimer.Stop();
            bOddTick = false;

            if (m_oSimConnect != null)
            {
                /// Dispose serves the same purpose as SimConnect_Close()
                m_oSimConnect.Dispose();
                m_oSimConnect = null;
            }

            sConnectButtonLabel = "Connect";
            bConnected = false;

            // Set all requests as pending
            foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
            {
                oSimvarRequest.bPending = true;
                oSimvarRequest.bStillPending = true;
            }
        }

        #endregion

        #region UI bindings

        public string sConnectButtonLabel
        {
            get { return m_sConnectButtonLabel; }
            private set { this.SetProperty(ref m_sConnectButtonLabel, value); }
        }
        private string m_sConnectButtonLabel = "Connect";

        public bool bObjectIDSelectionEnabled
        {
            get { return m_bObjectIDSelectionEnabled; }
            set { this.SetProperty(ref m_bObjectIDSelectionEnabled, value); }
        }
        private bool m_bObjectIDSelectionEnabled = false;
        public SIMCONNECT_SIMOBJECT_TYPE eSimObjectType
        {
            get { return m_eSimObjectType; }
            set
            {
                this.SetProperty(ref m_eSimObjectType, value);
                bObjectIDSelectionEnabled = (m_eSimObjectType != SIMCONNECT_SIMOBJECT_TYPE.USER);
                ClearResquestsPendingState();
            }
        }
        private SIMCONNECT_SIMOBJECT_TYPE m_eSimObjectType = SIMCONNECT_SIMOBJECT_TYPE.USER;
        public ObservableCollection<uint> lObjectIDs { get; private set; }
        public uint iObjectIdRequest
        {
            get { return m_iObjectIdRequest; }
            set
            {
                this.SetProperty(ref m_iObjectIdRequest, value);
                ClearResquestsPendingState();
            }
        }
        private uint m_iObjectIdRequest = 0;
        

        public string[] aSimvarNames
        {
            get { return SimUtils.SimVars.Names; }
            private set { }
        }
        public string sSimvarRequest
        {
            get { return m_sSimvarRequest; }
            set { this.SetProperty(ref m_sSimvarRequest, value); }
        }
        private string m_sSimvarRequest = null;


        public string[] aUnitNames
        {
            get { return SimUtils.Units.Names; }
            private set { }
        }
        public string sUnitRequest
        {
            get { return m_sUnitRequest; }
            set { this.SetProperty(ref m_sUnitRequest, value); }
        }
        private string m_sUnitRequest = null;

        public string sSetValue
        {
            get { return m_sSetValue; }
            set { this.SetProperty(ref m_sSetValue, value); }
        }
        private string m_sSetValue = null;

        public ObservableCollection<SimvarRequest> lSimvarRequests { get; private set; }
        public SimvarRequest oSelectedSimvarRequest
        {
            get { return m_oSelectedSimvarRequest; }
            set { this.SetProperty(ref m_oSelectedSimvarRequest, value); }
        }
        private SimvarRequest m_oSelectedSimvarRequest = null;

        public uint[] aIndices
        {
            get { return m_aIndices; }
            private set { }
        }
        private readonly uint[] m_aIndices = new uint[100] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                                                            10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                                                            20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                                                            30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
                                                            40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
                                                            50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
                                                            60, 61, 62, 63, 64, 65, 66, 67, 68, 69,
                                                            70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
                                                            80, 81, 82, 83, 84, 85, 86, 87, 88, 89,
                                                            90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
        public uint iIndexRequest
        {
            get { return m_iIndexRequest; }
            set { this.SetProperty(ref m_iIndexRequest, value); }
        }
        private uint m_iIndexRequest = 0;

        public bool bSaveValues
        {
            get { return m_bSaveValues; }
            set { this.SetProperty(ref m_bSaveValues, value); }
        }
        private bool m_bSaveValues = true;

        public bool bFSXcompatible
        {
            get { return m_bFSXcompatible; }
            set { this.SetProperty(ref m_bFSXcompatible, value); }
        }
        private bool m_bFSXcompatible = false;

        public bool bOddTick
        {
            get { return m_bOddTick; }
            set { this.SetProperty(ref m_bOddTick, value); }
        }
        private bool m_bOddTick = false;

        public ObservableCollection<string> lErrorMessages { get; private set; }


        public BaseCommand cmdToggleConnect { get; private set; }
        public BaseCommand cmdAddRequest { get; private set; }
        public BaseCommand cmdRemoveSelectedRequest { get; private set; }
        public BaseCommand cmdTrySetValue { get; private set; }
        public BaseCommand cmdLoadFiles { get; private set; }
        public BaseCommand cmdSaveFile { get; private set; }

        #endregion

        #region Real time

        private DispatcherTimer m_oTimer = new DispatcherTimer();

        #endregion

        UdpClient listener = null;
        IPEndPoint groupEP = null;

        public SimvarsViewModel()
        {
            lObjectIDs = new ObservableCollection<uint>();
            lObjectIDs.Add(1);

            lSimvarRequests = new ObservableCollection<SimvarRequest>();
            lErrorMessages = new ObservableCollection<string>();

            cmdToggleConnect = new BaseCommand((p) => { ToggleConnect(); });
            cmdAddRequest = new BaseCommand((p) => { AddRequest(null, null); });
            cmdRemoveSelectedRequest = new BaseCommand((p) => { RemoveSelectedRequest(); });
            cmdTrySetValue = new BaseCommand((p) => { TrySetValue(); });
            cmdLoadFiles = new BaseCommand((p) => { LoadFiles(); });
            cmdSaveFile = new BaseCommand((p) => { SaveFile(false); });

            m_oTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            m_oTimer.Tick += new EventHandler(OnTick);

            to_write[0] = new Dictionary<string, double>();
        }
        private void Connect()
        {
            Console.WriteLine("Connect");

            try
            {
                /// The constructor is similar to SimConnect_Open in the native API
                m_oSimConnect = new SimConnect("Simconnect - Simvar test", m_hWnd, WM_USER_SIMCONNECT, null, bFSXcompatible? (uint)1 : 0);

                /// Listen to connect and quit msgs
                m_oSimConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                m_oSimConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                /// Listen to exceptions
                m_oSimConnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                /// Catch a simobject data request
                m_oSimConnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);

                m_oSimConnect.OnRecvAssignedObjectId += new SimConnect.RecvAssignedObjectIdEventHandler(SimConnect_OnRecvAssignedObjectId);
            }
            catch (COMException ex)
            {
                Console.WriteLine("Connection to KH failed: " + ex.Message);
            }

        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.WriteLine("SimConnect_OnRecvOpen");
            Console.WriteLine("Connected to KH");

            sConnectButtonLabel = "Disconnect";
            bConnected = true;

            // Register pending requests
            foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
            {
                if (oSimvarRequest.bPending)
                {
                    oSimvarRequest.bPending = !RegisterToSimConnect(oSimvarRequest);
                    oSimvarRequest.bStillPending = oSimvarRequest.bPending;
                }
            }

            //set up udp host that emulates xplane
            listener = new UdpClient(49000);
            groupEP = new IPEndPoint(IPAddress.Any, 0);

            m_oTimer.Start();
            bOddTick = false;
        }

        /// The case where the user closes game
        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Console.WriteLine("SimConnect_OnRecvQuit");
            Console.WriteLine("KH has exited");

            Disconnect();
            listener.Close();
        }
        private void SimConnect_OnRecvAssignedObjectId(SimConnect sender, SIMCONNECT_RECV data)
        {
            //received new object with an ID, assume it is the most recently created aircraft
            //need to get the aircraft id from this object id...=
            uint object_id = (uint) data.GetType().GetField("dwObjectID").GetValue(data);
            string intruder_name = intruder_ids[intruder_ids.Count - 1];

            to_write[object_id] = new Dictionary<string, double>();

            spawned_intruder_ids[intruder_name] = object_id;
        }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;
            Console.WriteLine("SimConnect_OnRecvException: " + eException.ToString());

            lErrorMessages.Add("SimConnect : " + eException.ToString());
        }
        
        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            Console.WriteLine("SimConnect_OnRecvSimobjectDataBytype");

            uint iRequest = data.dwRequestID;
            uint iObject = data.dwObjectID;
            if (!lObjectIDs.Contains(iObject))
            {
                lObjectIDs.Add(iObject);
            }
            foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
            {
                if (iRequest == (uint)oSimvarRequest.eRequest && (!bObjectIDSelectionEnabled || iObject == m_iObjectIdRequest))
                {
                    double dValue = (double)data.dwData[0];
                    oSimvarRequest.dValue = dValue;
                    oSimvarRequest.bPending = false;
                    oSimvarRequest.bStillPending = false;
                }
            }
        }

        // May not be the best way to achive regular requests.
        // See SimConnect.RequestDataOnSimObject
        Dictionary<uint, Dictionary<String, double>> to_write = new Dictionary<uint, Dictionary<string, double>>();
        List<string> intruder_ids = new List<string>();
        Dictionary<string, uint> spawned_intruder_ids = new Dictionary<string, uint>();
        private void OnTick(object sender, EventArgs e)
        {
            Console.WriteLine("OnTick");

            //bOddTick = !bOddTick;

            foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
            {
                if (!oSimvarRequest.bPending)
                {
                    m_oSimConnect?.RequestDataOnSimObjectType(oSimvarRequest.eRequest, oSimvarRequest.eDef, 0, m_eSimObjectType);
                    oSimvarRequest.bPending = true;
                }
                else
                {
                    oSimvarRequest.bStillPending = true;
                }
            }

            //every frame we set each of these manually identified IDs to the most recently recieved value from the TCP master
            while (listener.Available > 0)
            {
                byte[] bytes = listener.Receive(ref groupEP);
                const int VEHX_BYTE_SIZE = 5 + 4 + 8 * 3 + 4 * 3;
                string to_string = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                if (bytes.Length == VEHX_BYTE_SIZE)
                {
                    if (to_string.StartsWith("VEHX"))
                    {
                        //packges set of the following:
                        int airplane_index = bytes[5];
                        double lat = BitConverter.ToDouble(bytes, 5 + 4) * Math.PI / 180.0;
                        double lon = BitConverter.ToDouble(bytes, 5 + 4 + 8) * Math.PI / 180.0;
                        double alt_wgs = BitConverter.ToDouble(bytes, 5 + 4 + 8 * 2);
                        float yaw = BitConverter.ToSingle(bytes, 5 + 4 + 8 * 3) * (float)Math.PI / 180.0f;
                        float pitch = BitConverter.ToSingle(bytes, 5 + 4 + 8 * 3 + 4) * (float)Math.PI / 180.0f;
                        float roll = BitConverter.ToSingle(bytes, 5 + 4 + 8 * 3 + 4 * 2) * (float)Math.PI / 180.0f;
                        Console.WriteLine($"{bytes.Length} vs {VEHX_BYTE_SIZE} | {airplane_index} {lat} {lon} {alt_wgs} {yaw} {pitch} {roll}");
                        if (airplane_index == 0)
                        {
                            //position ownship
                            to_write[0]["PLANE LATITUDE"] = lat;
                            to_write[0]["PLANE LONGITUDE"] = lon;
                            to_write[0]["PLANE ALTITUDE"] = alt_wgs;
                            to_write[0]["PLANE PITCH DEGREES"] = -1.0 * pitch;
                            to_write[0]["PLANE BANK DEGREES"] = -1.0 * roll;
                            to_write[0]["PLANE HEADING DEGREES TRUE"] = yaw;
                        }
                        else
                        {
                            string intruder_name = "n" + airplane_index;
                            if (!intruder_ids.Contains(intruder_name))
                            {
                                SIMCONNECT_DATA_INITPOSITION init_pos = default;
                                init_pos.Latitude = lat;
                                init_pos.Longitude = lon;
                                init_pos.Altitude = alt_wgs;
                                init_pos.Bank = roll;
                                init_pos.Heading = yaw;
                                init_pos.Pitch = pitch;
                                //airspeed shouldnt matter as sim is frozen, this object should be updated once a frame
                                m_oSimConnect.AICreateNonATCAircraft("Boeing 747-8i Asobo", intruder_name, init_pos, null);
                                intruder_ids.Add(intruder_name);
                            }
                            if (spawned_intruder_ids.ContainsKey(intruder_name))
                            {
                                uint objid = spawned_intruder_ids[intruder_name];
                                to_write[objid]["PLANE LATITUDE"] = lat;
                                to_write[objid]["PLANE LONGITUDE"] = lon;
                                to_write[objid]["PLANE ALTITUDE"] = alt_wgs;
                                to_write[objid]["PLANE PITCH DEGREES"] = -1.0 * pitch;
                                to_write[objid]["PLANE BANK DEGREES"] = -1.0 * roll;
                                to_write[objid]["PLANE HEADING DEGREES TRUE"] = yaw;
                            }
                        }
                    }
                }
                else if (to_string.StartsWith("DREF"))
                {
                    //the value in the dref message (see gazebo_xplane_plugin) is the 4 bytes after byte 5
                    float value = BitConverter.ToSingle(bytes, 5);
                    if (to_string.Contains("yoke_heading_ratio"))
                    {
                        to_write[0]["RUDDER POSITION"] = value;
                        to_write[0]["RUDDER PEDAL POSITION"] = value;
                    }
                    else if (to_string.Contains("yoke_pitch_ratio"))
                    {
                        to_write[0]["ELEVATOR POSITION"] = value;
                        to_write[0]["YOKE Y POSITION"] = value;
                    }
                    else if (to_string.Contains("yoke_roll_ratio"))
                    {
                        to_write[0]["AILERON POSITION"] = value;
                        to_write[0]["YOKE X POSITION"] = value;
                    }
                    else if (to_string.Contains("throttle_ratio_all"))
                    {
                        to_write[0]["GENERAL ENG THROTTLE LEVER POSITION:1"] = value;
                    }
                }
                else
                {
                    Console.WriteLine($"Unhandled broadcast from {groupEP} :");
                    Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                }
            }

            foreach (uint obj_id in to_write.Keys)
            {
                foreach (string key in to_write[obj_id].Keys)
                {
                    bool found = false;
                    foreach (SimvarRequest req in lSimvarRequests)
                    {
                        if (req.sName == key && req.eObjectID == obj_id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        AddRequest(key, "amp", obj_id);
                    }
                }
            }

            foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
            {
                if (to_write[oSimvarRequest.eObjectID].ContainsKey(oSimvarRequest.sName)) {
                    m_oSimConnect.SetDataOnSimObject(oSimvarRequest.eDef, oSimvarRequest.eObjectID > 0? oSimvarRequest.eObjectID : SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, to_write[oSimvarRequest.eObjectID][oSimvarRequest.sName]);
                }
            }
        }

        private void ToggleConnect()
        {
            if (m_oSimConnect == null)
            {
                try
                {
                    Connect();
                }
                catch (COMException ex)
                {
                    Console.WriteLine("Unable to connect to KH: " + ex.Message);
                }
            }
            else
            {
                Disconnect();
            }
        }

        private void ClearResquestsPendingState()
        {
            foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
            {
                oSimvarRequest.bPending = false;
                oSimvarRequest.bStillPending = false;
            }
        }

        private bool RegisterToSimConnect(SimvarRequest _oSimvarRequest)
        {
            if (m_oSimConnect != null)
            {
                /// Define a data structure
                m_oSimConnect.AddToDataDefinition(_oSimvarRequest.eDef, _oSimvarRequest.sName, _oSimvarRequest.sUnits, SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                /// IMPORTANT: Register it with the simconnect managed wrapper marshaller
                /// If you skip this step, you will only receive a uint in the .dwData field.
                m_oSimConnect.RegisterDataDefineStruct<double>(_oSimvarRequest.eDef);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void AddRequest(string _sOverrideSimvarRequest, string _sOverrideUnitRequest, uint object_id = 0)
        {
            Console.WriteLine("AddRequest");

            string sNewSimvarRequest = _sOverrideSimvarRequest != null ? _sOverrideSimvarRequest : ((m_iIndexRequest == 0) ? m_sSimvarRequest : (m_sSimvarRequest + ":" + m_iIndexRequest));
            string sNewUnitRequest = _sOverrideUnitRequest != null ? _sOverrideUnitRequest : m_sUnitRequest;

            SimvarRequest oSimvarRequest = new SimvarRequest
            {
                eDef = (DEFINITION)m_iCurrentDefinition,
                eRequest = (REQUEST)m_iCurrentRequest,
                eObjectID = object_id,
                sName = sNewSimvarRequest,
                sUnits = sNewUnitRequest
            };

            oSimvarRequest.bPending = !RegisterToSimConnect(oSimvarRequest);
            oSimvarRequest.bStillPending = oSimvarRequest.bPending;

            lSimvarRequests.Add(oSimvarRequest);

            ++m_iCurrentDefinition;
            ++m_iCurrentRequest;
        }

        private void RemoveSelectedRequest()
        {
            lSimvarRequests.Remove(oSelectedSimvarRequest);
        }

        private void TrySetValue()
        {
            Console.WriteLine("TrySetValue");

            if (m_oSelectedSimvarRequest != null && m_sSetValue != null)
            {
                double dValue = 0.0;
                if (double.TryParse(m_sSetValue, NumberStyles.Any, null, out dValue))
                {
                    m_oSimConnect.SetDataOnSimObject(m_oSelectedSimvarRequest.eDef, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, dValue);
                }
            }
        }

        private void LoadFiles()
        {
            Microsoft.Win32.OpenFileDialog oOpenFileDialog = new Microsoft.Win32.OpenFileDialog();
            oOpenFileDialog.Multiselect = true;
            oOpenFileDialog.Filter = "Simvars files (*.simvars)|*.simvars";
            if (oOpenFileDialog.ShowDialog() == true)
            {
                foreach (string sFilename in oOpenFileDialog.FileNames)
                {
                    LoadFile(sFilename);
                }
            }
        }

        private void LoadFile(string _sFileName)
        {
            string[] aLines = System.IO.File.ReadAllLines(_sFileName);
            for (uint i = 0; i < aLines.Length; ++i)
            {
                // Format : Simvar,Unit
                string[] aSubStrings = aLines[i].Split(',');
                if (aSubStrings.Length >= 2) // format check
                {
                    // values check
                    string[] aSimvarSubStrings = aSubStrings[0].Split(':'); // extract Simvar name from format Simvar:Index
                    string sSimvarName = Array.Find(SimUtils.SimVars.Names, s => s == aSimvarSubStrings[0]);
                    string sUnitName = Array.Find(SimUtils.Units.Names, s => s == aSubStrings[1]);
                    if (sSimvarName != null && sUnitName != null)
                    {
                        AddRequest(aSubStrings[0], aSubStrings[1]);
                    }
                    else
                    {
                        if (sSimvarName == null)
                        {
                            lErrorMessages.Add("l." + i.ToString() + " Wrong Simvar name : " + aSubStrings[0]);
                        }
                        if (sUnitName == null)
                        {
                            lErrorMessages.Add("l." + i.ToString() + " Wrong Unit name : " + aSubStrings[1]);
                        }
                    }
                }
                else
                {
                    lErrorMessages.Add("l." + i.ToString() + " Bad input format : " + aLines[i]);
                    lErrorMessages.Add("l." + i.ToString() + " Must be : SIMVAR,UNIT");
                }
            }
        }

        private void SaveFile(bool _bWriteValues)
        {
            Microsoft.Win32.SaveFileDialog oSaveFileDialog = new Microsoft.Win32.SaveFileDialog();
            oSaveFileDialog.Filter = "Simvars files (*.simvars)|*.simvars";
            if (oSaveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter oStreamWriter = new StreamWriter(oSaveFileDialog.FileName, false))
                {
                    foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
                    {
                        // Format : Simvar,Unit
                        string sFormatedLine = oSimvarRequest.sName + "," + oSimvarRequest.sUnits;
                        if (bSaveValues)
                        {
                            sFormatedLine += ",  " + oSimvarRequest.dValue.ToString();
                        }
                        oStreamWriter.WriteLine(sFormatedLine);
                    }
                }
            }
        }

        public void SetTickSliderValue(int _iValue)
        {
            m_oTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(_iValue));
        }
    }
}
