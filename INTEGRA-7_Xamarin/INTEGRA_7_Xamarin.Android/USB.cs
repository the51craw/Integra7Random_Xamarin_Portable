using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware.Usb;

namespace INTEGRA_7_Xamarin.Droid
{
    public class USB
    {
        private static String ACTION_USB_PERMISSION = "com.android.example.USB_PERMISSION";
        public PendingIntent mPermissionIntent = null;
        public UsbManager usbManager = null;
        public UsbInterface usbInterface = null;
        public UsbDevice usbDevice = null;
        public UsbEndpoint outputEndpoint = null;
        public UsbEndpoint inputEndpoint = null;

        public void Init(UsbManager usbManager)
        {
            Int32 deviceIndex = 0;
            Int32 deviceCount = 0;
            Int32 interfaceIndex;
            Int32 endpointIndex;
            UsbDevice[] usbDevices;
            UsbInterface[][] usbInterfaces = null;
            UsbEndpoint[][][] usbEndpoints = null;
            Int32 count = 0;

            this.usbManager = usbManager;

            // Get the USB devices (normally one in an Android device):
            usbDevices = new UsbDevice[usbManager.DeviceList.Count];
            foreach (KeyValuePair<string, UsbDevice> keyValuePair in usbManager.DeviceList)
            {
                usbDevice = keyValuePair.Value;
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
                        usbDevice = usbDevices[deviceIndex];
                        usbInterfaces[deviceIndex] = new UsbInterface[usbDevices[deviceIndex].InterfaceCount];
                        for (interfaceIndex = 0; interfaceIndex < usbDevices[deviceIndex].InterfaceCount; interfaceIndex++)
                        {
                            usbInterfaces[deviceIndex][interfaceIndex] = usbDevices[deviceIndex].GetInterface(interfaceIndex);
                        }
                        usbEndpoints[deviceIndex] = new UsbEndpoint[usbDevices[deviceIndex].InterfaceCount][];
                        for (interfaceIndex = 0; interfaceIndex < usbDevices[deviceIndex].InterfaceCount; interfaceIndex++)
                        {
                            usbEndpoints[deviceIndex][interfaceIndex] = new UsbEndpoint[usbInterfaces[deviceIndex][interfaceIndex].EndpointCount];
                            usbInterface = usbInterfaces[deviceIndex][interfaceIndex];
                            for (endpointIndex = 0; endpointIndex < usbInterfaces[deviceIndex][interfaceIndex].EndpointCount; endpointIndex++)
                            {
                                usbEndpoints[deviceIndex][interfaceIndex][endpointIndex] = usbInterfaces[deviceIndex][interfaceIndex].GetEndpoint(endpointIndex);
                                if (usbEndpoints[deviceIndex][interfaceIndex][endpointIndex].Direction == Android.Hardware.Usb.UsbAddressing.Out)
                                {
                                    outputEndpoint = usbEndpoints[deviceIndex][interfaceIndex][endpointIndex];
                                }
                                else
                                {
                                    inputEndpoint = usbEndpoints[deviceIndex][interfaceIndex][endpointIndex];
                                }
                            }
                        }
                    }
                }
            }

            if (usbDevice != null && usbDevice != null && usbInterface != null && outputEndpoint != null && inputEndpoint != null)
            {
                usbManager.RequestPermission(usbDevice, mPermissionIntent);
                bool hasPermision = usbManager.HasPermission(usbDevice);

                if (hasPermision)
                {
                    UsbDeviceConnection deviceConnection = usbManager.OpenDevice(usbDevices[0]);
                    if (deviceConnection.ClaimInterface(usbInterface, true))
                    {
                        byte[] buffer = new byte[] { 0x04, 0x90, 0x65, 0x64 };
                        count = deviceConnection.BulkTransfer(outputEndpoint, buffer, buffer.Length, 5000);
                        buffer = new byte[] { 0x04, 0x80, 0x65, 0x00 };
                        count = deviceConnection.BulkTransfer(outputEndpoint, buffer, buffer.Length, 5000);
                        deviceConnection.ReleaseInterface(usbInterface);
                        deviceConnection.Close();
                        deviceConnection.Dispose();
                    }
                }
            }
        }
    }
}