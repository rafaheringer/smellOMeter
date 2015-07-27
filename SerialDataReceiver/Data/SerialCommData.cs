using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;

namespace SerialDataReceiver.Data
{
	public class SerialCommData
	{
		private SerialPort serial = new SerialPort();

		#region Init
		public SerialCommData(SerialPort serialPort)
		{
			serial = serialPort;
		}
		public SerialCommData()
		{
		}

		#endregion

		#region Connection
		public static IEnumerable<string> GetOpenedSerialComms()
		{
			return SerialPort.GetPortNames();
		}
		public bool IsConnected()
		{
			return serial.IsOpen;
		}
		public bool Connect(string serialPortName)
		{
			serial.PortName = serialPortName;
			return Connect();

		}
		public bool Connect()
		{
			try
			{
				serial.Open();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}

		}

		public bool Disconnect()
		{
			try
			{
				serial.Close();
				return true;
			}
			catch (Exception ex) {
				return false;
			}
			
		}
		#endregion

		#region Data received
		public string ReadData()
		{
			return serial.ReadExisting();
		}

		public string ReadLine() {
			return serial.ReadLine();
		}

		public void AttachDataReceivedCallback(System.IO.Ports.SerialDataReceivedEventHandler dataReceivedEvent)
		{
			serial.DataReceived += dataReceivedEvent;
		}
		#endregion
	}
}
