using System;
using System.Collections.Generic;
using LaunchpadAPI;

namespace LaunchpadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var (inputDevices, outputDevices) = LaunchpadCreator.GetDeviceNames();
            Console.WriteLine($"Input devices:\n- {string.Join("\n- ", inputDevices)}");
            Console.WriteLine($"Output devices:\n- {string.Join("\n- ", outputDevices)}");
            try {
                Launchpad lp = LaunchpadCreator.CreateFromNames("LPX MIDI ", "LPX MIDI ");
                lp.ClearAll();
                lp.SetLEDs(new Dictionary<(int, int), LaunchpadColor> {
                    [(0, 0)] = new LaunchpadColor(127, 0, 0),
                    [(1, 0)] = new LaunchpadColor(56, 127, 0),
                    [(1, 1)] = new LaunchpadColor(34, 0, 45),
                    [(0, 1)] = new LaunchpadColor(0, 127, 127),
                });
            } catch (NoSuchDeviceException e) {
                System.Console.WriteLine($"Could not load launchpad: {e.Message}");
            }
        }
    }
}
