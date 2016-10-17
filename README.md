# forza-trackir-wrapper

This is wrapper for your XInput controller that maps TrackIR input to the right stick,
allowing you to freelook with TrackIR in Forza Horizon 3.

## Requirements

* [ScpServer](https://github.com/nefarius/ScpToolkit)

## Setup

1. Since there are no releases yet, download and compile the source.
2. Run ForzaTrackIR.exe.
3. **Important:** Make sure TrackIR is running. If it's not running, the application will crash. ;)
4. Click Start/Stop. You should hear Windows activating a new device.
5. Start Forza and play as usual, but with head tracking.

## Limitations

* This software is incomplete, so it doesn't work very well yet. You can't look around while inputting anything else with your controller. :(
* Hardcoded to replace the right stick on an XInput controller (Xbox 360, Xbox One). If you're using some other controller, tough luck.
* Only works with TrackIR.

## Acknowledgements

* Uses ScpDevice.cs wholesale from [XOutput](https://github.com/Stents-/XOutput).
* Uses [Unity-TrackIR-Plugin-DLL](https://github.com/medsouz/Unity-TrackIR-Plugin-DLL).
