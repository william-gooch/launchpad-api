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
        
        public static LaunchpadColor BLACK = new LaunchpadColor(0, 0, 0);
        public static LaunchpadColor RED = new LaunchpadColor(127, 0, 0);
        public static LaunchpadColor GREEN = new LaunchpadColor(0, 127, 0);
        public static LaunchpadColor BLUE = new LaunchpadColor(0, 0, 127);
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
        public abstract void SetLEDs(LaunchpadColor[,] leds);
        public abstract void SetAllLEDs(LaunchpadColor color);
        public abstract void ClearAll();
    }
}