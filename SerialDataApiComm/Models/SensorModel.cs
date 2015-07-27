using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDataApiComm.Models
{
	public class BathroomStatusModel
	{
		public SensorModel Sensor { get; set; }
		public bool isOccupied { get; set; }
		public int smellOMeter { get; set; }
		public string statusMessage { get; set; }
	}

	public class SensorModel
	{
		public bool Connected { get; set; }
		public string Message { get; set; }
		public SensorValuesModel SensorsValues { get; set; }
	}

	public class SensorValuesModel
	{
		private int _gasSensorValue;
		private int _pirSensorValue;
		private int _sonicSensorValue;

		//MQ2
		public int GasSensorValue
		{
			get { return _gasSensorValue; }
			set { if (value != 0) _gasSensorValue = value; }
		}
		//PIR
		public int PIRSensorValue
		{
			get { return _pirSensorValue; }
			set { if (value != 0) _pirSensorValue = value; }
		}
		//SONIC
		public int SonicSensorValue
		{
			get { return _sonicSensorValue; }
			set { if (value != 0) _sonicSensorValue = value; }
		}
	}
}
