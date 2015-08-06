#include <VirtualWire.h>

//Configurações MQ2 - Sensor de Gás
const int mqSensorPin = A2;

//Configurações do transmissor RF
const int transmissorPin = 8;
int transmissorLedState = LOW;
char Message[7];								//Mensagem a ser enviada

//Configurações do PIR
const int PIRSensorPin = 2;
int PIRHasActivity = 1;

//Configurações do HC-SR04 - Sensor ultrasônico
const int sonicEchoPin = 7;
const int sonicTrigPin = 6;
long sonicDuration;
long sonicDistance;

//Configurações gerais
long previousMillis = 0;			 // Variável de controle do tempo
long transmitterInterval = 3000;     // Tempo em ms do intervalo a ser executado

void setup()
{
	//Abre porta SERIAL
	Serial.begin(9600);

	//Define tipos de entrada/saida
	pinMode(mqSensorPin, INPUT);
	pinMode(PIRSensorPin, INPUT);
	pinMode(sonicTrigPin, OUTPUT);
	pinMode(sonicEchoPin, INPUT);
	
	//Configurações do transmissor
	vw_set_tx_pin(transmissorPin);		//Pino do transmissor
	vw_set_ptt_inverted(true);			//??
	vw_setup(2000);						//Bits por segundo
}

void loop()
{
	//A cada intervalo configurado
	unsigned long currentMillis = millis();    //Tempo atual em ms
	if (currentMillis - previousMillis > transmitterInterval) {
		previousMillis = currentMillis;    // Salva o tempo atual

		//////////////////////////////MQ2
		//Lê o valor
		int mqSensorValue = analogRead(mqSensorPin);

		//Serial.print("MQ2: ");
		//Serial.print(mqSensorValue);

		//////////////////////////////PIR
		//Lê o valor do PIR
		PIRHasActivity = digitalRead(PIRSensorPin);
		PIRHasActivity = PIRHasActivity == LOW ? 1 : 2;

		//Serial.print("   PIR:");
		//Serial.print(PIRHasActivity);
		
		//////////////////////////////ULTRASONIC
		//Ciclo:
		digitalWrite(sonicTrigPin, LOW);
		delayMicroseconds(2);

		digitalWrite(sonicTrigPin, HIGH);
		delayMicroseconds(10);

		digitalWrite(sonicTrigPin, LOW);
		sonicDuration = pulseIn(sonicEchoPin, HIGH);

		//Calcula a distância de acordo com o ciclo
		sonicDistance = sonicDuration / 58.2;

		//Serial.print("   SONIC:");
		//Serial.print(sonicDistance);
		//Serial.println();

		//////////////////////////////Transmissor
		//Pincodes:
		//1: MQ2 - Sensor de Gas
		//2: PIR - Primeiro sensor de presença (1 - inativo; 2 - ativo)
		//3: SONIC - Segundo sensor de presença

		//1
		sendMessage(1, mqSensorValue);

		//2
		sendMessage(2, PIRHasActivity);

		//3
		sendMessage(3, sonicDistance);

		//LED de debug
		//transmissorLedState = transmissorLedState == HIGH ? LOW : HIGH;
		//digitalWrite(transmissorLedPin, transmissorLedState);
	}
	
}


void sendMessage(int pinCode, int val) {
	//double startTime = millis(); 
	//Serial.println("Enviando...");

	/*Serial.print("Valor: ");
	Serial.print(val);
	Serial.print(" no pinCode: ");
	Serial.print(pinCode);*/

	char packetData[6] = { ' ', ' ', ' ', ' ', ' ', ' ' };			  //Mensagem a ser enviada
	char b[2];
	String str = String(pinCode); //converting integer into a string
	int count = 2;
	str.toCharArray(b, 2);
	packetData[0] = b[0];
	packetData[1] = '.';
	
	String digitString = String(val);
	char digits[sizeof digitString];
	digitString.toCharArray(digits, sizeof digitString);

	for (int i = 0; i<strlen(digits); i++){
		int k = digits[i] = digits[i] - 48;
		
		char b[2];
		String str = String(k); //converting integer into a string
		str.toCharArray(b, 2);
		packetData[count] = b[0];
		count++;
	}

	vw_send((uint8_t *)packetData, 6);
	vw_wait_tx();

	//double endTime = millis();
	//double timeTaken = (endTime - startTime); 
	//Serial.print(" Duracao do envio: ");
	//Serial.print(timeTaken);
	//Serial.print(" ms\n");
	Serial.println();
}