using System;
using System.Collections.Generic;
using System.Linq;
using RtMidi.Core;
using RtMidi.Core.Devices;

namespace LaunchpadAPI {

    public class NoSuchDeviceException : Exception {
        public NoSuchDeviceException() : base() { }
        public NoSuchDeviceException(string message) : base(message) { }
        public NoSuchDeviceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public static class LaunchpadCreator {
        public static Launchpad CreateFromDevices(IMidiInputDevice inputDevice, IMidiOutputDevice outputDevice) {
            return new LaunchpadX(inputDevice, outputDevice);
        }

        public static Launchpad CreateFromNames(string inputName, string outputName) {
            try {
                var iDeviceInfo = MidiDeviceManager.Default.InputDevices
                    .Where(c => c.Name == inputName).First();
                var iDevice = iDeviceInfo.CreateDevice();
                if(!iDevice.Open()) throw new Exception($"Could not create input device {inputName}");

                try {
                    var oDeviceInfo = MidiDeviceManager.Default.OutputDevices
                        .Where(c => c.Name == outputName).First();
                    var oDevice = oDeviceInfo.CreateDevice();
                    if(!oDevice.Open()) throw new Exception($"Could not create output device {outputName}");

                    return CreateFromDevices(iDevice, oDevice);
                } catch (InvalidOperationException e) {
                    throw new NoSuchDeviceException($"There is no output device named {outputName}", e);
                }
            } catch (InvalidOperationException e) {
                throw new NoSuchDeviceException($"There is no input device named {inputName}", e);
            }
        }

        public static (List<string> Input, List<string> Output) GetDeviceNames() {
            var inputNames = (
                from info in MidiDeviceManager.Default.InputDevices
                select info.Name).ToList();
            var outputNames = (
                from info in MidiDeviceManager.Default.OutputDevices
                select info.Name).ToList();
            return (inputNames, outputNames);
        }
    }
}