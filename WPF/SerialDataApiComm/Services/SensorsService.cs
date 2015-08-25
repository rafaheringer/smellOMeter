using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDataApiComm.Services
{
	internal static class SensorsService
	{
		private static int sonicSensorNeutralDistance = 118;
		private static int gasSensorMinLimit = 120;
		private static int gasSensorMaxLimit = 220;

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
			if (sensorsValues.GasSensorValue == 10)
				sensorsValues.GasSensorValue = 100;
			if (sensorsValues.GasSensorValue == 20)
				sensorsValues.GasSensorValue = 200;
			return (100 * (sensorsValues.GasSensorValue - gasSensorMinLimit)) / (gasSensorMaxLimit - gasSensorMinLimit);
		}
	}
}
