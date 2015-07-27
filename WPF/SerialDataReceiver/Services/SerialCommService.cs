using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;

namespace SerialDataReceiver.Services
{
	public class SerialCommService
	{
		private Data.SerialCommData serialCommData = new Data.SerialCommData();

		public SerialCommService() { }

		/// <summary>
		/// Get all serial ports listed on Windows
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<string> GetOpenedSerialComms()
		{
			return Data.SerialCommData.GetOpenedSerialComms();
		}

		public bool Connect(string serialPortName)
		{
			if (!serialCommData.IsConnected())
			{

				SerialPort serial = new SerialPort();
				serial.PortName = serialPortName;
				serial.BaudRate = Convert.ToInt32(9600); //Arduino serial Baud Rate
				serial.Handshake = System.IO.Ports.Handshake.None;
				serial.Parity = Parity.None;
				serial.DataBits = 8;
				serial.StopBits = StopBits.One;
				serial.ReadTimeout = 500;
				serial.WriteTimeout = 300;

				serialCommData = new Data.SerialCommData(serial);
			}

			return serialCommData.Connect();
		}

		public bool Disconnect() {
			return serialCommData.Disconnect();
		}

		public string ReadData() {
			if (serialCommData.IsConnected())
				return serialCommData.ReadData();
			else
				return null;
		}

		public string ReadLine() {
			if (serialCommData.IsConnected())
				return serialCommData.ReadLine();
			else
				return null;
		}

		public void AttachDataReceivedCallback(System.IO.Ports.SerialDataReceivedEventHandler eventHandlher) 
		{
			serialCommData.AttachDataReceivedCallback(eventHandlher);
		}
	}
}
