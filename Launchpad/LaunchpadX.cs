using System;
using System.Collections.Generic;
using System.Linq;
using RtMidi.Core.Devices;
using RtMidi.Core.Messages;

namespace LaunchpadAPI
{
    class LaunchpadX : Launchpad
    {
        private static readonly byte[,] ledLayout = new byte[,] {
            { 91, 92, 93, 94, 95, 96, 97, 98, 99 },
            { 81, 82, 83, 84, 85, 86, 87, 88, 89 },
            { 71, 72, 73, 74, 75, 76, 77, 78, 79 },
            { 61, 62, 63, 64, 65, 66, 67, 68, 69 },
            { 51, 52, 53, 54, 55, 56, 57, 58, 59 },
            { 41, 42, 43, 44, 45, 46, 47, 48, 49 },
            { 31, 32, 33, 34, 35, 36, 37, 38, 39 },
            { 21, 22, 23, 24, 25, 26, 27, 28, 29 },
            { 11, 12, 13, 14, 15, 16, 17, 18, 19 },
        };

        public LaunchpadX(IMidiInputDevice inputDevice, IMidiOutputDevice outputDevice)
            : base(inputDevice, outputDevice)
        {
            Console.WriteLine("Launchpad X Initialized?");
        }

        public override void SetLED(int x, int y, LaunchpadColor color)
        {
            var msg = new SysExMessage(
                new byte[] {
                    0x00, 0x20, 0x29, 0x02, 0x0C, 0x03,
                    0x03, ledLayout[y,x], color.Red, color.Green, color.Blue
                }
            );
            outputDevice.Send(msg);
        }

        public override void ClearLED(int x, int y)
        {
            SetLED(x, y, new LaunchpadColor(0, 0, 0));
        }

        public override void SetLEDs(IDictionary<(int x, int y), LaunchpadColor> changes) {
            var bytes = new byte[6 + changes.Count * 5];
            Array.Copy(new byte[] { 0x00, 0x20, 0x29, 0x02, 0x0C, 0x03 }, 0, bytes, 0, 6);
            int changeNumber = 0;
            foreach(var change in changes) {
                byte padNumber = ledLayout[change.Key.y, change.Key.x];
                Array.Copy(new byte[] { 0x03, padNumber, change.Value.Red, change.Value.Blue, change.Value.Green }, 0,
                    bytes, 6 + changeNumber*5, 5);
                changeNumber++;
            }
            var msg = new SysExMessage(bytes);
            outputDevice.Send(msg);
        }

        public override void SetLEDs(LaunchpadColor[,] leds) {
            if(leds.GetLength(0) != 9 || leds.GetLength(1) != 9) {
                throw new ArgumentException("LEDs matrix is the wrong size, should be 9x9");
            }
            var bytes = new byte[6 + 5 * 81];
            Array.Copy(
                new byte[] { 0x00, 0x20, 0x29, 0x02, 0x0C, 0x03 },
                bytes, 6);
            for(int y = 0; y < 9; y++) {
                for (int x = 0; x < 9; x++) {
                    Array.Copy(
                        new byte[] { 0x03, ledLayout[y,x], leds[y,x].Red, leds[y,x].Green, leds[y,x].Blue }, 0,
                        bytes, 6 + 5 * (9*y + x), 5);
                }
            }
            var msg = new SysExMessage(bytes);
            outputDevice.Send(msg);
        }

        public override void SetAllLEDs(LaunchpadColor color)
        {
            var bytes = new byte[6 + 5 * 81];
            Array.Copy(
                new byte[] { 0x00, 0x20, 0x29, 0x02, 0x0C, 0x03 },
                bytes, 6);
            for(int y = 0; y < 9; y++) {
                for(int x = 0; x < 9; x++) {
                    Array.Copy(
                        new byte[] { 0x03, ledLayout[y,x], color.Red, color.Green, color.Blue }, 0,
                        bytes, 6 + 5 * (9*y + x), 5);
                }
            }
            var msg = new SysExMessage(bytes);
            outputDevice.Send(msg);
        }

        public override void ClearAll()
        {
            SetAllLEDs(LaunchpadColor.BLACK);
        }
    }
}