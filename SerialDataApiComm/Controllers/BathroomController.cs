using Newtonsoft.Json;
using SerialDataApiComm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDataApiComm.Controllers
{
	public class BathroomController
	{
		Models.BathroomStatusModel bathroomStatus;

		public BathroomController() {
			bathroomStatus = new Models.BathroomStatusModel();
			bathroomStatus.Sensor = new Models.SensorModel();
			bathroomStatus.Sensor.SensorsValues = new Models.SensorValuesModel();
		}

		#region Received data
		public async void ReceiveData(string receivedData)
		{
			//Json Transform
			try
			{
				Models.SensorModel sensorModel = JsonConvert.DeserializeObject<Models.SensorModel>(receivedData);

				//Generate message status
				bathroomStatus = updateBathroomStatus(sensorModel);

				//Send data to API Webhook
				await ApiController.SendDataToWebhook(bathroomStatus);
			}
			catch(Exception) {
				///TODO
			}

		}
		#endregion

		#region Update Status
		private Models.BathroomStatusModel updateBathroomStatus(Models.BathroomStatusModel bathroomStatusModel)
		{
			return Services.bathroomService.UpdateBathroomStatus(bathroomStatusModel);
		}
		private Models.BathroomStatusModel updateBathroomStatus()
		{
			return updateBathroomStatus(bathroomStatus);
		}
		private Models.BathroomStatusModel updateBathroomStatus(Models.SensorModel sensorModel)
		{
			//Ignore 0 values (The Model "set" command don't work?)
			if(sensorModel.SensorsValues.GasSensorValue != 0)
				bathroomStatus.Sensor.SensorsValues.GasSensorValue = sensorModel.SensorsValues.GasSensorValue;

			if (sensorModel.SensorsValues.PIRSensorValue != 0)
				bathroomStatus.Sensor.SensorsValues.PIRSensorValue = sensorModel.SensorsValues.PIRSensorValue;

			if (sensorModel.SensorsValues.SonicSensorValue != 0)
				bathroomStatus.Sensor.SensorsValues.SonicSensorValue = sensorModel.SensorsValues.SonicSensorValue;

			return updateBathroomStatus();
		}
		#endregion

	}
}
