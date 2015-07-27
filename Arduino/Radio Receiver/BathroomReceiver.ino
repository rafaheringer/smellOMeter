#include <VirtualWire.h>
#include <string.h>

const int receive_pin = 2;

//Configurações do MQ2 - Sensor de Gás
const int gasToObserve = 2;		//0 = Propano; 1 = Butano; 2 = Metano; 3 = Hidrogenio; 4 = Alcool
int gasSensorValue = 0;
int gasConcentration;
int gasPPM;
int gasMinPPM;
int gasMaxPPM;

//Configurações dos IR
int IR1HasActivity = 1;							//1 - Passou e não voltou 0 - Não passou ou não voltou
int IR2HasActivity = 1;							//1 - Passou e não voltou 0 - Não passou ou não voltou

//Configurações do receptor
//byte message[VW_MAX_MESSAGE_LEN];
//byte messageLength = VW_MAX_MESSAGE_LEN;

//Outras configurações
bool debugMode = false;

void setup()
{
	//Mapeamento dos valores mínimo e máximo, a fim de usar a Modulação de Largura de Pulso 
	//Propano: 200 - 5000ppm
	//Butano: 300 - 5000ppm
	//Metano: 5000 - 20000ppm
	//Hidrogenio: 300 - 5000ppm
	//Alcool: 100 - 2000ppm
	//Faça o teste de base para verificar o mínimo de PPM do gás desejado
	switch (gasToObserve)
	{
	case 0:
		gasMinPPM = 200;
		gasMaxPPM = 5000;
		break;
	case 1:
		gasMinPPM = 300;
		gasMaxPPM = 5000;
		break;
	case 2:
		gasMinPPM = 5000;
		gasMaxPPM = 20000;
		break;
	case 3:
		gasMinPPM = 300;
		gasMaxPPM = 5000;
		break;
	case 4:
		gasMinPPM = 100;
		gasMaxPPM = 2000;
		break;
	}



	// Initialise the IO and ISR
	vw_set_rx_pin(receive_pin);
	vw_set_ptt_inverted(true); // Required for DR3100
	vw_setup(2000);	 // Bits per sec

	Serial.begin(9600);
	vw_rx_start();       //Começa o processo
}

byte count = 1;

void loop()
{
	vw_wait_rx();

	byte message[VW_MAX_MESSAGE_LEN];
	byte messageLength = VW_MAX_MESSAGE_LEN;


	//Confere se a mensagem veio inteira
	if (vw_get_message(message, &messageLength))
	{
		//Inicia o objeto
		Serial.print("{");
		Serial.print("\"SensorsValues\":{");

		//Pincodes:
		//1: MQ2 - Sensor de Gas
		//2: PIR - Primeiro sensor de presença
		//3: SONIC - Segundo sensor de presença

		//Espera o comando do MQ2
		int gasSensorValue = processResponse((char*)message, '1', messageLength);

		if (gasSensorValue) {
			//Conversão do gás
			//0 é o mínimo de saída do pino e 1023 sempre será o máximo do Arduino
			gasConcentration = map(gasSensorValue, 0, 1023, 0, 100);
			gasPPM = map(gasSensorValue, 0, 1023, gasMinPPM, gasMaxPPM);

			Serial.print("\"GasSensorValue\":");
			Serial.print(gasSensorValue);
			/*Serial.print(",gasPercent:");
			Serial.print(gasConcentration);
			Serial.print(",gasConcentration:");
			Serial.print(gasPPM)*/;
		}

		//Espera o comando do PIR (2 = ativo, 1 = inativo)
		int PIRSensorValue = processResponse((char*)message, '2', messageLength);

		if (PIRSensorValue) {
			Serial.print("\"PIRSensorValue\":");
			Serial.print(PIRSensorValue);
		}

		//Espera o comando do Sensor ultrasonico
		int SonicSensorValue = processResponse((char*)message, '3', messageLength);

		if (SonicSensorValue) {
			Serial.print("\"SonicSensorValue\":");
			Serial.print(SonicSensorValue);
		}

		digitalWrite(13, LOW);

		//Fecha o objeto
		Serial.print("}");
		Serial.print("}");
		Serial.println();
	}


}

int processResponse(char* message, char pinCode, byte messageLength) {
	char pin;
	char command[4] = {' ', ' ', ' ', ' '};

	for (int i = 0; i < messageLength; i++)
	{
		if (i == 0) {
			pin = char(message[i]);
		}

		else if (i > 1) {
			command[i - 2] = char(message[i]);
		}
	}

	if (pinCode == pin) {
		return atoi(command);
	}
	else {
		return 0;
	}
}

