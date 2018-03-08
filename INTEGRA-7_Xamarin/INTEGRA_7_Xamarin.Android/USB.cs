using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using INTEGRA_7_Xamarin.Droid;
using Android.Content;
using Android.Hardware.Usb;
using System.Collections.Generic;
using System;

namespace INTEGRA_7_Xamarin.Droid
{
    public class USB
    {
        public UsbManager Manager { get; set; }
        public UsbInterface Interface { get; set; }
        public UsbDevice Device { get; set; }
        public UsbEndpoint OutputEndpoint { get; set; }
        public UsbEndpoint InputEndpoint { get; set; }
        public Boolean HasPermission { get; set; }
        public UsbReceiver UsbReceiver { get; set; }

        public USB(UsbManager Manager)
        {
            Int32 deviceIndex = 0;
            Int32 deviceCount = 0;
            Int32 interfaceIndex = 0;
            Int32 endpointIndex = 0;
            UsbDevice[] usbDevices = null;
            UsbInterface[][] usbInterfaces = null;
            UsbEndpoint[][][] usbEndpoints = null;

            // The UsbManager can only be obtained from MainActivity!
            this.Manager = Manager;

            // Get the USB devices (normally one in an Android device):
            usbDevices = new UsbDevice[Manager.DeviceList.Count];
            foreach (KeyValuePair<string, UsbDevice> keyValuePair in Manager.DeviceList)
            {
                usbDevices[deviceIndex] = keyValuePair.Value;
                deviceIndex++;
            }
            deviceCount = deviceIndex;

            // Create usbInterface and usbEnpont lists:
            usbInterfaces = new UsbInterface[deviceCount][];
            usbEndpoints = new UsbEndpoint[deviceCount][][];

            // Loop all (one) devices and look for interfaces and endpoints:
            for (deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                if (usbDevices[deviceIndex].InterfaceCount > 0)
                {
                    if (usbDevices[deviceIndex].ProductName == "INTEGRA-7")
                    {
                        Device = usbDevices[deviceIndex];
                        usbInterfaces[deviceIndex] = new UsbInterface[usbDevices[deviceIndex].InterfaceCount];
                        for (interfaceIndex = 0; interfaceIndex < usbDevices[deviceIndex].InterfaceCount; interfaceIndex++)
                        {
                            usbInterfaces[deviceIndex][interfaceIndex] = usbDevices[deviceIndex].GetInterface(interfaceIndex);
                        }
                        usbEndpoints[deviceIndex] = new UsbEndpoint[usbDevices[deviceIndex].InterfaceCount][];
                        for (interfaceIndex = 0; interfaceIndex < usbDevices[deviceIndex].InterfaceCount; interfaceIndex++)
                        {
                            usbEndpoints[deviceIndex][interfaceIndex] = new UsbEndpoint[usbInterfaces[deviceIndex][interfaceIndex].EndpointCount];
                            Interface = usbInterfaces[deviceIndex][interfaceIndex];
                            for (endpointIndex = 0; endpointIndex < usbInterfaces[deviceIndex][interfaceIndex].EndpointCount; endpointIndex++)
                            {
                                usbEndpoints[deviceIndex][interfaceIndex][endpointIndex] = usbInterfaces[deviceIndex][interfaceIndex].GetEndpoint(endpointIndex);
                                if (usbEndpoints[deviceIndex][interfaceIndex][endpointIndex].Direction == Android.Hardware.Usb.UsbAddressing.Out)
                                {
                                    OutputEndpoint = usbEndpoints[deviceIndex][interfaceIndex][endpointIndex];
                                }
                                else
                                {
                                    InputEndpoint = usbEndpoints[deviceIndex][interfaceIndex][endpointIndex];
                                }
                            }
                        }
                    }
                }
            }
            if (Device != null && Interface != null && OutputEndpoint != null && InputEndpoint != null)
            {
                UsbReceiver = new UsbReceiver();
            }
        }
    }
}