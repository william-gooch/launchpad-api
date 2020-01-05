using System;
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

        public override void ClearAll()
        {
            var msgData = new byte[6 + 5 * 81];
            Array.Copy(
                new byte[] { 0x00, 0x20, 0x29, 0x02, 0x0C, 0x03 },
                msgData, 6);
            for(int y = 0; y < 9; y++) {
                for(int x = 0; x < 9; x++) {
                    msgData[6 + 5*(9*y + x) + 0] = 0x03;
                    msgData[6 + 5*(9*y + x) + 1] = ledLayout[y,x];
                    msgData[6 + 5*(9*y + x) + 2] = 0;
                    msgData[6 + 5*(9*y + x) + 3] = 0;
                    msgData[6 + 5*(9*y + x) + 4] = 0;
                }
            }
            var msg = new SysExMessage(msgData);
            outputDevice.Send(msg);
        }
    }
}