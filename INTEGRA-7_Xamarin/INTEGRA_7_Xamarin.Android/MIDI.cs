using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Xamarin.Forms;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Usb;
//using Java.Lang;
using INTEGRA_7_Xamarin.Droid;
using Mono;

[assembly: Dependency(typeof(MIDI))]

namespace INTEGRA_7_Xamarin.Droid
{
    //[Android.Runtime.Register("android/content/Context", DoNotGenerateAcw = true)]

    public class MIDI : Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IMidi
    {
        public byte MidiOutPortChannel { get; set; }
        public byte MidiInPortChannel { get; set; }
        public Int32 MidiOutPortSelectedIndex { get; set; }
        public Int32 MidiInPortSelectedIndex { get; set; }
        public INTEGRA_7_Xamarin.MainPage mainPage;
        UsbReceiver usbReceiver;
        public UsbManager usbManager { get; set; }
        UsbInterface usbInterface = null;
        UsbDevice usbDevice = null;
        UsbEndpoint outputEndpoint = null;
        UsbEndpoint inputEndpoint = null;
        MainActivity mainActivity = null;
        public MIDI() { }

        // Simpleconstructor that takes the name of the device:
        ~MIDI()
        {
            try
            {
            } catch { }
        }

        public void PreInit(object usbManager, object usbReceiver)
        {
            this.usbManager = (UsbManager)usbManager;
            this.usbReceiver = (UsbReceiver)usbReceiver;
        }

        public void Init(INTEGRA_7_Xamarin.MainPage mainPage, String deviceName, Picker OutputDeviceSelector, Picker InputDeviceSelector, object Dispatcher, byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            mainActivity = (MainActivity)Dispatcher;
            this.mainPage = mainPage;
        }

        public void UpdateMidiComboBoxes(Picker midiOutputComboBox, Picker midiInputComboBox)
        {
        }

        public void OutputDeviceChanged(Picker DeviceSelector)
        {
        }

        public void InputDeviceChanged(Picker DeviceSelector)
        {
        }

        public byte GetMidiOutPortChannel()
        {
            return MidiOutPortChannel;
        }

        public void SetMidiOutPortChannel(byte OutPortChannel)
        {
            MidiOutPortChannel = OutPortChannel;
        }

        public byte GetMidiInPortChannel()
        {
            return MidiInPortChannel;
        }

        public void SetMidiInPortChannel(byte InPortChannel)
        {
            MidiInPortChannel = InPortChannel;
        }

        public void NoteOn(byte currentChannel, byte noteNumber, byte velocity)
        {
            mainActivity.NoteOn(currentChannel, noteNumber, velocity);
        }

        public void NoteOff(byte currentChannel, byte noteNumber)
        {
            mainActivity.NoteOff(currentChannel, noteNumber);

        }

        public void SendControlChange(byte channel, byte controller, byte value)
        {
        }

        public void SetVolume(byte currentChannel, byte volume)
        {
        }

        public void ProgramChange(byte currentChannel, String smsb, String slsb, String spc)
        {
            //try
            //{
            //    MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, (byte)(UInt16.Parse(smsb)));
            //    MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, (byte)(UInt16.Parse(slsb)));
            //    MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(UInt16.Parse(spc) - 1));
            //    midiOutPort.SendMessage(controlChangeMsb);
            //    midiOutPort.SendMessage(controlChangeLsb);
            //    midiOutPort.SendMessage(programChange);
            //}
            //catch { }
        }

        public void ProgramChange(byte currentChannel, byte msb, byte lsb, byte pc)
        {
            //try
            //{
            //    MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, msb);
            //    MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, lsb);
            //    MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(pc - 1));
            //    midiOutPort.SendMessage(controlChangeMsb);
            //    midiOutPort.SendMessage(controlChangeLsb);
            //    midiOutPort.SendMessage(programChange);
            //}
            //catch { }
        }

        public void SendSystemExclusive(byte[] bytes)
        {
            //IBuffer buffer = bytes.AsBuffer();
            //MidiSystemExclusiveMessage midiMessageToSend = new MidiSystemExclusiveMessage(buffer);
            //midiOutPort.SendMessage(midiMessageToSend);
        }

        //public void MidiInPort_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        //{
        //}

        public byte[] SystemExclusiveRQ1Message(byte[] Address, byte[] Length)
        {
            byte[] result = new byte[17];
            result[0] = 0xf0; // Start of exclusive message
            result[1] = 0x41; // Roland
            result[2] = 0x10; // Device Id is 17 according to settings in INTEGRA-7 (Menu -> System -> MIDI, 1 = 0x00 ... 17 = 0x10)
            result[3] = 0x00;
            result[4] = 0x00;
            result[5] = 0x64; // INTEGRA-7
            result[6] = 0x11; // Command (DT1)
            result[7] = Address[0];
            result[8] = Address[1];
            result[9] = Address[2];
            result[10] = Address[3];
            result[11] = Length[0];
            result[12] = Length[1];
            result[13] = Length[2];
            result[14] = Length[3];
            result[15] = 0x00; // Filled out by CheckSum but present here to avoid confusion about index 15 missing.
            result[16] = 0xf7; // End of sysex
            CheckSum(ref result);
            return (result);
        }

        public byte[] SystemExclusiveDT1Message(byte[] Address, byte[] DataToTransmit)
        {
            Int32 length = 13 + DataToTransmit.Length;
            byte[] result = new byte[length]; 
            result[0] = 0xf0; // Start of exclusive message
            result[1] = 0x41; // Roland
            result[2] = 0x10; // Device Id is 17 according to settings in INTEGRA-7 (Menu -> System -> MIDI, 1 = 0x00 ... 17 = 0x10)
            result[3] = 0x00;
            result[4] = 0x00;
            result[5] = 0x64; // INTEGRA-7
            result[6] = 0x12; // Command (DT1)
            result[7] = Address[0];
            result[8] = Address[1];
            result[9] = Address[2];
            result[10] = Address[3];
            for (Int32 i = 0; i < DataToTransmit.Length; i++)
            {
                result[i + 11] = DataToTransmit[i];
            }
            result[12 + DataToTransmit.Length] = 0xf7; // End of sysex
            CheckSum(ref result);
            return (result);
        }

        public void CheckSum(ref byte[] bytes)
        {
            byte chksum = 0;
            for (Int32 i = 7; i < bytes.Length - 2; i++)
            {
                chksum += bytes[i];
            }
            bytes[bytes.Length - 2] = (byte)((0x80 - (chksum & 0x7f)) & 0x7f);
        }

        //public void OnDeviceOpened(MidiDevice device)
        //{
        //    throw new NotImplementedException();
        //}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
