using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Xamarin.Forms;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Usb;
using Java.Util;
using INTEGRA_7_Xamarin.Droid;
using Mono;
using System.Runtime.InteropServices.WindowsRuntime;

[assembly: Dependency(typeof(MIDI))]

namespace INTEGRA_7_Xamarin.Droid
{
    // This midi class does not implement MIDI, but it is called the same way as for other platforms.
    // Here we only forward all MIDI calls to MainActivity which in turn uses USB to
    // send MIDI packets. The class that implements MIDI via USB is Android_Midi below.
    public class MIDI : TimerTask, IMidi, IGenericHandler
    {
        MainActivity mainActivity = null;
        MainPage MainPage_Portable = null;
        public byte MidiOutPortChannel { get; set; }
        public byte MidiInPortChannel { get; set; }
        public Int32 MidiOutPortSelectedIndex { get; set; }
        public Int32 MidiInPortSelectedIndex { get; set; }
        public INTEGRA_7_Xamarin.MainPage mainPage;
//        public USB usb { get; set; }
        public byte[] rawData;
        public Timer timer;
        public Boolean MessageReceived = false;
        private byte[] collectionBuffer = null;

        public void GenericHandler(object sender, object e)
        {
        }

        public MIDI() { }

        public void Init(INTEGRA_7_Xamarin.MainPage mainPage, String deviceName, Picker OutputDeviceSelector, Picker InputDeviceSelector, object MainActivity, byte MidiOutPortChannel, byte MidiInPortChannel)
        {
            mainActivity = (MainActivity)MainActivity;
            this.MainPage_Portable = mainPage;
            timer = new Timer();
            timer.ScheduleAtFixedRate(this, 1, 1);
        }

        public override void Run()
        {
            // We will come here once per millisecond to see if we have some USB incoming.
            // USB reads are initiated here, and if one is pending, we must not initiate a new one.
            // We buffer and analyze incoming data and asseble the MIDI messages.
            // Each complete message is used in a call to MidiInport_MessageReceived.
            //if (buffer[1] == 0xf0 && buffer[9] == 0x11) // Note: Offsets from MIDI SysEx standard due to embedded USB codes.
            USB usb = (USB)MainPage_Portable.platform_specific[0];
            if (usb != null && usb.InputEndpoint != null && usb.DeviceConnection != null)
            {
                // See if there is some data:
                byte[] inputBuffer = new byte[64];
                Int32 count;
                count = usb.DeviceConnection.BulkTransfer(usb.InputEndpoint, inputBuffer, 64, 5000);

                // Handle data if it was expected:
                if (count > 0 && MainPage_Portable.uIHandler.queryType != UIHandler.QueryType.NONE)
                {
                    collectionBuffer = AddBuffer(inputBuffer, count);

                    // Did we get the full message?
                    // Last 4 bytes starts with a USB code with value = 5, 6 or 7 rather than 4:
                    if (collectionBuffer[collectionBuffer.Length - 4] != 0x04)
                    {
                        rawData = RemoveUsbCodes(collectionBuffer);
                        collectionBuffer = null;
                        MidiInPort_MessageReceived(rawData);
                    }
                    //MessageReceived = true;
                }
            }
            if (MessageReceived)
            {
                // Just skip the keep-alive messages:
                if (!(rawData.Length == 1 && rawData[0] == 0xfe))
                {
                    // Alert mainPage
                    mainPage.uIHandler.rawData = rawData;
                    mainPage.uIHandler.MidiInPort_MessageRecceived();
                    MessageReceived = false;
                }
            }
        }

        private byte[] AddBuffer(byte[] buffer, Int32 count)
        {
            Int32 from = 0;
            Int32 to = 0;
            byte[] tempBuffer = null;
            if (collectionBuffer == null)
            {
                tempBuffer = new byte[count];
            }
            else
            {
                tempBuffer = new byte[collectionBuffer.Length + count];
                while (to < collectionBuffer.Length)
                {
                    tempBuffer[to++] = collectionBuffer[from++];
                }
            }
            from = 0;
            while (to < tempBuffer.Length)
            {
                tempBuffer[to++] = buffer[from++];
            }
            return tempBuffer;
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
            UsbTransmit(MakeUsbBuffer(new byte[] { 0x90, noteNumber, velocity }));
        }

        public void NoteOff(byte currentChannel, byte noteNumber)
        {
            UsbTransmit(MakeUsbBuffer(new byte[] { 0x80, noteNumber, 0x00 }));
        }

        public void SendControlChange(byte channel, byte controller, byte value)
        {
        }

        public void SetVolume(byte currentChannel, byte volume)
        {
        }

        public void ProgramChange(byte currentChannel, String smsb, String slsb, String spc)
        {
            try
            {
                MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, (byte)(UInt16.Parse(smsb)));
                MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, (byte)(UInt16.Parse(slsb)));
                MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(UInt16.Parse(spc) - 1));
                UsbTransmit(MakeUsbBuffer(controlChangeMsb.Message));
                UsbTransmit(MakeUsbBuffer(controlChangeLsb.Message));
                UsbTransmit(MakeUsbBuffer(programChange.Message));
            }
            catch { }
        }

        public void ProgramChange(byte currentChannel, byte msb, byte lsb, byte pc)
        {
            try
            {
                MidiControlChangeMessage controlChangeMsb = new MidiControlChangeMessage(currentChannel, 0x00, msb);
                MidiControlChangeMessage controlChangeLsb = new MidiControlChangeMessage(currentChannel, 0x20, lsb);
                MidiProgramChangeMessage programChange = new MidiProgramChangeMessage(currentChannel, (byte)(pc - 1));
                UsbTransmit(MakeUsbBuffer(controlChangeMsb.Message));
                UsbTransmit(MakeUsbBuffer(controlChangeLsb.Message));
                UsbTransmit(MakeUsbBuffer(programChange.Message));
            }
            catch { }
        }

        public void SendSystemExclusive(byte[] bytes)
        {
            UsbTransmit(MakeUsbBuffer(bytes));
        }

        public void MidiInPort_MessageReceived(byte[] message)
        {
            MainPage_Portable.uIHandler.rawData = message;
            MainPage_Portable.uIHandler.MidiInPort_MessageRecceived();
        }

        private byte[] MakeUsbBuffer(byte[] bytes)
        {
            Int32 newBufferLength;
            Int32 blockCount;
            // USB protocol needs 32-bit packages.
            // First byte, the code index, describes the use of the following 3 bytes:
            // 0x04 SysEx starts or continues (i.e. all packages except the last one)
            // 0x05 (Single-byte System Common Message or) SysEx ends with following single byte.
            // 0x06 SysEx ends with following two bytes. 
            // 0x07 SysEx ends with following three bytes. 
            // When the length of the message is not a multiple of 23-bit pcackages, (0x05 and 0x06
            // messages above) it is padded with 0x00 bytes.
            byte lastCodeIndex = (byte)(0x04 + (bytes.Length % 3));
            blockCount = bytes.Length / 3;            // Number of 3-byte blocks except odd bytes at end
            if ((bytes.Length % 3) > 0)
            {
                blockCount++;
            }
            else
            {
                lastCodeIndex = 0x07;
            }

            newBufferLength = 4 * blockCount;
            byte[] usbBuffer = new byte[newBufferLength];
            Int32 from = 0;
            Int32 to = 0;
            for (to = 0; to < newBufferLength && from < bytes.Length; to++)
            {
                if ((to % 4) == 0)
                {
                    if (bytes[0] == 0x0f0 && to >= usbBuffer.Length - 4)
                    {
                        usbBuffer[to] = lastCodeIndex;
                    }
                    else
                    {
                        usbBuffer[to] = 0x04;
                    }
                }
                else
                {
                    usbBuffer[to] = bytes[from++];
                }
            }
            while (to < newBufferLength)
            {
                usbBuffer[to++] = 0x00;
            }
            return usbBuffer;
        }

        private byte[] RemoveUsbCodes(byte[] usbBuffer)
        {
            Int32 midiBufferSize = usbBuffer.Length * 3 / 4;        // Now includes unwanted end padding!
            Int32 lastUsbCode = usbBuffer[usbBuffer.Length - 4];    // 7 = no padding, 6 = 1 byte padding, 5 = 2 bytes padding.
            Int32 paddingSize = 7 - lastUsbCode;
            midiBufferSize -= paddingSize;
            byte[] midiBuffer = new byte[midiBufferSize];
            Int32 midiIndex = 0;
            Int32 usbIndex = 0;
            for (usbIndex = 0; midiIndex < midiBufferSize; usbIndex++)
            {
                if ((usbIndex % 4) > 0)
                {
                    midiBuffer[midiIndex++] = usbBuffer[usbIndex];
                }
            }
            return midiBuffer;
        }

        private void UsbTransmit(byte[] buffer)
        {
            USB usb = (USB)MainPage_Portable.platform_specific[0];
            if (usb != null && usb.HasPermission)
            {
                //UsbDeviceConnection deviceConnection = usb.Manager.OpenDevice(usb.Device);
                //if (usb.DeviceConnection.ClaimInterface(usb.Interface, true))
                {
                    usb.DeviceConnection.BulkTransfer(usb.OutputEndpoint, buffer, buffer.Length, 5000);
                    //if (buffer[1] == 0xf0 && buffer[9] == 0x11) // Note: Offsets from MIDI SysEx standard due to embedded USB codes.
                    //{
                    //    byte[] inputBuffer = new byte[64];
                    //    usb.DeviceConnection.BulkTransfer(usb.InputEndpoint, inputBuffer, 64, 5000);
                    //    rawData = RemoveUsbCodes(inputBuffer);
                    //    MessageReceived = true;
                    //}
                    //Java.Nio.ByteBuffer byteBuffer = (Java.Nio.ByteBuffer)Java.Nio.ByteBuffer. FromArray<byte>(buffer);
                    //usb.Request.Queue(byteBuffer);
                    //deviceConnection.ReleaseInterface(usb.Interface);
                    //deviceConnection.Close();
                    //deviceConnection.Dispose();
                    //try
                    //{
                    //    usb.DeviceConnection.RequestWait(5000);
                    //}
                    //catch (Exception e)
                    //{

                    //}
                }
            }
        }

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
    }

    internal class MidiProgramChangeMessage
    {
        public byte[] Message { get; set; }
        public MidiProgramChangeMessage(byte channel, byte patchNumber)
        {
            Message = new byte[2];
            Message[0] = (byte)(0xc0 | channel);
            Message[1] = patchNumber;
        }
    }

    internal class MidiControlChangeMessage
    {
        public byte[] Message { get; set; }
        public MidiControlChangeMessage(byte channel, byte controlNumber, byte value)
        {
            Message = new byte[3];
            Message[0] = (byte)(0xb0 | channel);
            Message[1] = controlNumber;
            Message[2] = value;
        }
    }

    // These are dummy classes to satisfy IMidi argument definitions that are actually not used.
    public class MidiMessageReceivedEventArgs
    {
    }

    public class MidiInPort
    {
    }
}
