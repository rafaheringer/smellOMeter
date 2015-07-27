
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDataApiComm.Services
{
	internal static class bathroomService
	{
		internal static Models.BathroomStatusModel UpdateBathroomStatus(Models.BathroomStatusModel bathroomStatusModel)
		{
			string message = String.Empty;

			bathroomStatusModel.Sensor.Connected = true;

			//Check if have anybody in bathroom
			bathroomStatusModel.isOccupied = SensorsService.SomeoneIsDetected(bathroomStatusModel.Sensor.SensorsValues);

			//Check Smell level
			bathroomStatusModel.smellOMeter = SensorsService.CheckSmellLevel(bathroomStatusModel.Sensor.SensorsValues);

			//Message
			message += bathroomStatusModel.isOccupied ? ":toilet: *Banheiro ocupado.* " : ":toilet: *Banheiro livre.* ";

			if (bathroomStatusModel.smellOMeter <= 10)
			{
				message += "Nunca se respirou um ar mais puro! :herb:";
			}
			else if (bathroomStatusModel.smellOMeter <= 30)
			{
				message += "Tudo bem, não tem com o que se preocupar. :thumbsup:";
			}
			else if (bathroomStatusModel.smellOMeter <= 50)
			{
				message += "Ainda está dando para aguentar. Vai fundo! :nose:";
			}
			else if (bathroomStatusModel.smellOMeter <= 70)
			{
				message += "A coisa está feia, é por sua conta e risco. :fearful:";
			}
			else if (bathroomStatusModel.smellOMeter > 70)
			{
				message += "ALERTA VERMELHO! :shit::shit: Se você tem amor ao seu pulmão, recomendo segurar a sua vontade. ::dizzy_face:";
			}



			bathroomStatusModel.statusMessage = message;

			return bathroomStatusModel;
		}
	}
}
