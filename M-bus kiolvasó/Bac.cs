using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.BACnet;
using System.Threading;
using System.Windows.Forms;
namespace M_bus_kiolvasó
{
    class Bac
    {

        public static BacnetClient bacnet_client;

        public static void startActivity(string localEndpoint)
        {
            //bacnet_client = new BacnetClient(new BacnetIpUdpProtocolTransport(0xBAC0, false, false, 1472, localEndpoint));
            bacnet_client = new BacnetClient(new BacnetIpUdpProtocolTransport(0xBAC0, false, false, 1472, localEndpoint));
            
            bacnet_client.Start();
        }

        public static BacnetPropertyIds getPropertyId(string prop)

        {
            BacnetPropertyIds result;

            switch (prop)
            {
                case "PV":
                    result = BacnetPropertyIds.PROP_PRESENT_VALUE;
                    break;
                case "List":
                    result = BacnetPropertyIds.PROP_OBJECT_LIST;
                    break;
                case "Name":
                    result = BacnetPropertyIds.PROP_OBJECT_NAME;
                    break;
                default:
                    result = BacnetPropertyIds.PROP_PRESENT_VALUE;
                    break;
            }
            return result;
        }

        public static BacnetObjectId bacnetNode(string objectType, uint instance)
        {
            BacnetObjectId bacnetobj = new BacnetObjectId();
            switch (objectType)
            {
                case "AI":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_ANALOG_INPUT;
                    break;
                case "AO":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_ANALOG_OUTPUT;
                    break;
                case "AV":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_ANALOG_VALUE;
                    break;
                case "BI":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_BINARY_INPUT;
                    break;
                case "BO":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_BINARY_OUTPUT;
                    break;
                case "BV":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_BINARY_VALUE;
                    break;
                case "MI":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_MULTI_STATE_INPUT;
                    break;
                case "MO":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_MULTI_STATE_OUTPUT;
                    break;
                case "MV":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_MULTI_STATE_VALUE;
                    break;
                case "DEV":
                    bacnetobj.type = BacnetObjectTypes.OBJECT_DEVICE;
                    break;
            }

            bacnetobj.Instance = instance;

            return bacnetobj;
        }

        public static BacnetAddress bacnetDevice(string ipAddress, ushort networkNumber)
        {
            BacnetAddress bacnetDev = new BacnetAddress(BacnetAddressTypes.IP, ipAddress, networkNumber);
            return bacnetDev;
        }

        public static string readAllAI(ushort networkNumber, string deviceIP, uint deviceInstance)
        {
            BacnetValue Value;
            IList<BacnetValue> NoScalarValue;
            string result = "";

            for (uint objectInstance = 0; objectInstance < 300; objectInstance++)
            {
                Application.DoEvents();
                try
                {
                    bacnet_client.ReadPropertyRequest(
                    bacnetDevice(deviceIP, networkNumber),
                    bacnetNode("AI", objectInstance),
                    getPropertyId("PV"),
                    out NoScalarValue);

                    result = result +
                             "AI" + "#" +
                             objectInstance + "#" +
                             readValue(1, deviceIP, deviceInstance, "AI", objectInstance, "Name") + "#" +
                             deviceIP + "#" +
                             deviceInstance + ";";
                }

                catch (Exception ex)
                {
                    var asd = ex.ToString();
                    break;
                }

            }


            return result;
        }

        public static string readValue(ushort networkNumber, string deviceIP, uint deviceInstance, string objectType, uint objectInstance, string property)
        {
            BacnetValue Value;
            IList<BacnetValue> NoScalarValue;

            bacnet_client.ReadPropertyRequest(
                bacnetDevice(deviceIP, networkNumber),
                bacnetNode(objectType, objectInstance),
                getPropertyId(property),
                out NoScalarValue);

            switch (property)
            {
                case "PV":
                    Value = NoScalarValue[0];
                    return Value.Value.ToString();

                case "Name":
                    Value = NoScalarValue[0];
                    return Value.Value.ToString();

                case "List":
                    string type = "";
                    string result = "";

                    var list = NoScalarValue;
                    foreach (var item in list)
                    {
                        string[] items = item.ToString().Split(new string[] { ":" }, StringSplitOptions.None);
                        switch (items[0])
                        {
                            case "OBJECT_ANALOG_INPUT":
                                type = "AI";
                                break;
                            case "OBJECT_ANALOG_VALUE":
                                type = "AV";
                                break;
                            default:
                                type = "";
                                break;
                        }
                        if (type != "")
                        {
                            uint instance = Convert.ToUInt16(items[1]);
                            result = result +
                                type + "#" +
                                instance + "#" +
                                //új cucc
                                readValue(1, deviceIP, deviceInstance, type, instance, "Name") + "#" +
                                deviceIP + "#" +
                                deviceInstance + ";";
                                //új cucc vége
                                //readValue(1, deviceIP, deviceInstance, type, instance, "Name") + ";";
                        }
                    }
                    return result;

                default:
                    return "-1";
            }
        }
    }
}
