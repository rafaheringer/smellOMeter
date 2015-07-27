using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDataApiComm.Services
{
	internal static class SensorsService
	{
		private static int sonicSensorNeutralDistance = 200;
		private static int gasSensorMinLimit = 30;
		private static int gasSensorMaxLimit = 400;

		internal static bool SomeoneIsDetected(Models.SensorValuesModel sensorValues)
		{
			//PIR Check
			if (sensorValues.PIRSensorValue == 2)
			{
				return true;
			}

			//ULTRASONIC Sensor check
			if (sensorValues.SonicSensorValue < sonicSensorNeutralDistance &&
				sensorValues.SonicSensorValue > 0)
			{
				return true;
			}


			return false;
		}

		internal static int CheckSmellLevel(Models.SensorValuesModel sensorsValues)
		{
			return (100 * (sensorsValues.GasSensorValue - gasSensorMinLimit)) / (gasSensorMaxLimit - gasSensorMinLimit);
		}
	}
}
