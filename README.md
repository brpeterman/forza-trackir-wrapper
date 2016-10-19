# forza-trackir-wrapper

This is wrapper for your DS4 controller that maps TrackIR input to the right stick,
allowing you to freelook with TrackIR in Forza Horizon 3.

## Requirements

* ScpDriver - Grab the one packaged with [XOutput](https://github.com/Stents-/XOutput).

## Setup

1. Since there are no releases yet, download and compile the source.
2. Run ForzaTrackIR.exe.
3. Select your DS4 controller from the list. It should be named "Wireless Controller".
4. Make sure TrackIR is running. The Start button should not be accessible as long as TrackIR is not running.
5. Click Start. You should hear Windows activating a new device.
6. Start Forza and play as usual, but with head tracking.
7. When you're done, click Stop again to disconnect the virtual gamepad.

## Limitations

* Hardcoded to replace the right stick on a DS4 controller. It *might* work with a DS3. It will almost certainly not work correctly with anything else.
* Only works with TrackIR. Sorry, FaceTrackNoIR users.
* Vibration does not work.
* See [Issues](https://github.com/brpeterman/forza-trackir-wrapper/issues).

## Acknowledgements

* Uses ScpDevice.cs wholesale from [XOutput](https://github.com/Stents-/XOutput).
* Uses [Unity-TrackIR-Plugin-DLL](https://github.com/medsouz/Unity-TrackIR-Plugin-DLL).
