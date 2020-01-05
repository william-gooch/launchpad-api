using System;
using System.Collections.Generic;
using System.Linq;
using RtMidi.Core;
using RtMidi.Core.Devices;

namespace LaunchpadAPI
{
    public struct LaunchpadColor
    {
        public byte Red, Green, Blue;

        public LaunchpadColor(byte red, byte green, byte blue) {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    public abstract class Launchpad
    {
        protected IMidiInputDevice inputDevice;
        protected IMidiOutputDevice outputDevice;

        protected Launchpad (IMidiInputDevice inputDevice, IMidiOutputDevice outputDevice) {
            this.inputDevice = inputDevice;
            this.outputDevice = outputDevice;
        }

        public abstract void SetLED(int x, int y, LaunchpadColor color);
        public abstract void ClearLED(int x, int y);
        public abstract void SetLEDs(IDictionary<(int x, int y), LaunchpadColor> changes);
        public abstract void ClearAll();
    }
}