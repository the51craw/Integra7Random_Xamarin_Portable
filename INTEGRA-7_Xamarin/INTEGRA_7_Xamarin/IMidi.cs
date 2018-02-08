using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRA_7_Xamarin
{
    public interface IMidi
    {
        void Init(String deviceName);

        void Init(String deviceName, INTEGRA_7_Xamarin.MainPage mainPage);

        //void Init(String deviceName, INTEGRA_7_Xamarin.MainPage mainPage, Picker OutputDeviceSelector, Picker InputDeviceSelector, /*CoreDispatcher Dispatcher,*/ byte MidiOutPortChannel, byte MidiInPortChannel);

        void NoteOn(byte currentChannel, byte noteNumber, byte velocity);

        void NoteOff(byte currentChannel, byte noteNumber);

        void SendControlChange(byte channel, byte controller, byte value);

        void SetVolume(byte currentChannel, byte volume);

        void ProgramChange(byte currentChannel, String smsb, String slsb, String spc);

        void ProgramChange(byte currentChannel, byte msb, byte lsb, byte pc);

        void SendSystemExclusive(byte[] bytes);

        byte[] SystemExclusiveDT1Message(byte[] Address, byte[] DataToTransmit);

        byte[] SystemExclusiveRQ1Message(byte[] Address, byte[] Length);

        void OutputDeviceChanged(Picker DeviceSelector);

        void InputDeviceChanged(Picker DeviceSelector);

        //void MidiInPort_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args);
    }
}
