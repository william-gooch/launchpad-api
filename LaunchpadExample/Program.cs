using System;
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
            } catch (NoSuchDeviceException e) {
                System.Console.WriteLine($"Could not load launchpad: {e.Message}");
            }
        }
    }
}
