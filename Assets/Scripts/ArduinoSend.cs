using System.IO.Ports;
using UnityEngine;

public class ArduinoSend : MonoBehaviour
{
    private SerialPort serialPort;
    public string portName = "COM5"; // Change to your Arduino's port (e.g., "COM3" or "/dev/ttyACM0")
    public int baudRate = 115200;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        serialPort.WriteTimeout = 500;
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Alpha1)) { // Press the '1' key
            SendTrigger("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { // Press the '2' key
            SendTrigger("2");
        }*/
    }

    public void SendTrigger(string command)
    {
        if (serialPort.IsOpen)
        {
            serialPort.Write(command); // Send the command
            Debug.Log("Sent: " + command);
        }
    }
    public void OnApplicationQuit() {
        if (serialPort != null && serialPort.IsOpen) {
            serialPort.Close();
        }
    }

}
