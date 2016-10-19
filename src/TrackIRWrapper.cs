using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIRUnity;

namespace ForzaTrackIR
{
    public class TrackIRWrapper
    {
        #region private constants
        private const int MAX_TRACKIR_VALUE = Int16.MaxValue / 2;
        private const int MIN_TRACKIR_VALUE = Int16.MinValue / 2;
        #endregion

        #region Private fields

        #region Devices
        private TrackIRClient _trackIR;
        #endregion

        #region State
        private TrackIRClient.LPTRACKIRDATA _trackIRState = new TrackIRClient.LPTRACKIRDATA();
        private bool _active = false;
        #endregion

        #endregion

        #region Delegates
        public delegate void StateChanged(TrackIRClient.LPTRACKIRDATA state);
        public delegate void ConnectedDelegate();
        public delegate void DisconnectedDelegate();

        public StateChanged UpdateHandler;
        public ConnectedDelegate Connected;
        public DisconnectedDelegate Disconnected;
        #endregion

        #region static methods
        public static double ToDegrees(float value)
        {
            int trackIRRange = MAX_TRACKIR_VALUE - MIN_TRACKIR_VALUE;
            int logicalRange = 360;
            double scale = (double)logicalRange / (double)trackIRRange;
            return value * scale;
        }

        /*
        Fields in LPTRACKIRDATA:
            public ushort wNPStatus;
            public ushort wPFrameSignature;
            public uint dwNPIOData;
            public float fNPRoll;
            public float fNPPitch;
            public float fNPYaw;
            public float fNPX;
            public float fNPY;
            public float fNPZ;
            public float fNPRawX;
            public float fNPRawY;
            public float fNPRawZ;
            public float fNPDeltaX;
            public float fNPDeltaY;
            public float fNPDeltaZ;
            public float fNPSmoothX;
            public float fNPSmoothY;
            public float fNPSmoothZ;
        */
        /// <summary>
        /// Check if there's any difference between oldState and newState.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        private static bool IsChanged(TrackIRClient.LPTRACKIRDATA oldState, TrackIRClient.LPTRACKIRDATA newState)
        {
            return (
                (oldState.wNPStatus != newState.wNPStatus) ||
                (oldState.wPFrameSignature != newState.wPFrameSignature) ||
                (oldState.dwNPIOData != newState.dwNPIOData) ||
                (oldState.fNPRoll != newState.fNPRoll) ||
                (oldState.fNPPitch != newState.fNPPitch) ||
                (oldState.fNPYaw != newState.fNPYaw) ||
                (oldState.fNPX != newState.fNPX) ||
                (oldState.fNPY != newState.fNPY) ||
                (oldState.fNPZ != newState.fNPZ) ||
                (oldState.fNPRawX != newState.fNPRawX) ||
                (oldState.fNPRawY != newState.fNPRawY) ||
                (oldState.fNPRawZ != newState.fNPRawZ) ||
                (oldState.fNPDeltaX != newState.fNPDeltaX) ||
                (oldState.fNPDeltaY != newState.fNPDeltaY) ||
                (oldState.fNPDeltaZ != newState.fNPDeltaZ) ||
                (oldState.fNPSmoothX != newState.fNPSmoothX) ||
                (oldState.fNPSmoothY != newState.fNPSmoothY) ||
                (oldState.fNPSmoothZ != newState.fNPSmoothZ)
                );
        }

        #endregion

        #region Constructors
        public TrackIRWrapper()
        {
            _trackIR = GetTrackIR();
        }
        #endregion

        #region Public methods
        public void Poll()
        {
            try {
                TrackIRClient.LPTRACKIRDATA state;
                state = _trackIR.client_HandleTrackIRData();
                if (TrackIRWrapper.IsChanged(_trackIRState, state))
                {
                    if (UpdateHandler != null)
                    {
                        UpdateHandler(state);
                    }
                }
            }
            catch (NullReferenceException)
            {
                // We'll get one of these if TrackIR is not connected
                if (_active)
                {
                    _active = false;
                    if (Disconnected != null)
                    {
                        Disconnected();
                    }
                }
            }
        }

        /// <summary>
        /// Probe the TrackIR system to see if it's up.
        /// If we find that the system's state doesn't match our internal state,
        /// we'll call Connected or Disconnected.
        /// </summary>
        public void Probe()
        {
            try
            {
                _trackIR.TrackIR_Enhanced_Init();
                _trackIR.client_HandleTrackIRData();
                if (!_active)
                {
                    _active = true;
                    if (Connected != null)
                    {
                        Connected();
                    }
                }
            }
            catch (NullReferenceException)
            {
                // We'll get one of these if TrackIR is not connected
                if (_active)
                {
                    _active = false;
                    if (Disconnected != null)
                    {
                        Disconnected();
                    }
                }
            }
        }
        #endregion

        #region Private methods

        #region Device initialization
        /// <summary>
        /// Get the trackIR client
        /// </summary>
        /// <returns></returns>
        private TrackIRClient GetTrackIR()
        {
            TrackIRClient trackIR = new TrackIRClient();
            return trackIR;
        }
        #endregion

        #endregion
    }
}
