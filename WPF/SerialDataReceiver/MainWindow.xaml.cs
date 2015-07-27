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
using MahApps.Metro.Controls;
using SerialDataReceiver.Services;

namespace SerialDataReceiver
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		#region variables
		//Richtextbox
		FlowDocument flowDocument = new FlowDocument();
		Paragraph paragraph = new Paragraph();

		//Serial control
		private string serialPortName;
		private Services.SerialCommService serialCommService = new Services.SerialCommService();

		//Api comm (Remove this if you don't use the ApiComm)
		private SerialDataApiComm.Controllers.BathroomController apiCommBathroomController = new SerialDataApiComm.Controllers.BathroomController();
		#endregion

		public MainWindow()
		{
			InitializeComponent();

			//overwite to ensure state
			connectBtn.Content = "Connect";

			//Get all connected serial ports
			IEnumerable<string> serialPorts = Services.SerialCommService.GetOpenedSerialComms();
			commPortNames.ItemsSource = serialPorts;
			commPortNames.SelectedIndex = 0;
		}

		private void ConnectToComms(object sender, RoutedEventArgs e)
		{
			serialPortName = commPortNames.Text;


			if (serialCommService.Connect(serialPortName))
			{
				//Sets button state
				connectBtn.Content = "Disconnect";

				//Data received callback
				serialCommService.AttachDataReceivedCallback(new System.IO.Ports.SerialDataReceivedEventHandler(Receive));

			}
			else
			{
				serialCommService.Disconnect();
				connectBtn.Content = "Connect";
			}
		}

		#region Receiving

		private delegate void UpdateUiTextDelegate(string text);

		private void Receive(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
		{
			string receivedLine = serialCommService.ReadLine();

			if (receivedLine != null)
			{
				//Send Data to API (Remove this if you don't use the ApiComm)
				apiCommBathroomController.ReceiveData(receivedLine);

				//Collecting the characters received to our 'buffer' (string).
				Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(ReceiveCallback), receivedLine);
			}
		}
		private void ReceiveCallback(string text)
		{
			//Assign the value of the recieved_data to the RichTextBox.
			if (paragraph.Inlines.Count >= 12)
			{
				flowDocument.Blocks.Clear();
				paragraph.Inlines.Clear();
			}
			paragraph.Inlines.Add(text);
			flowDocument.Blocks.Add(paragraph);
			Commdata.Document = flowDocument;
		}

		#endregion
	}
}
