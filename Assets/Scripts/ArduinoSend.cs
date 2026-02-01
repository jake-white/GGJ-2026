using CollabXR;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoSend : SingletonBehavior<ArduinoSend>
{
    private SerialPort serialPort;
    public string portName = "COM5"; // Change to your Arduino's port (e.g., "COM3" or "/dev/ttyACM0")
    public int baudRate = 115200;
    public Dictionary<int, char> hoopTogglesOn;
    public Dictionary<int, char> hoopTogglesOff;
    public Dictionary<int, char> mothershipModes;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        serialPort.WriteTimeout = 500;
    }

    public void ToggleHoop(int id, bool on)
    {
        Dictionary<int, char> dict = on ? hoopTogglesOn : hoopTogglesOff;
        SendTrigger(dict[id].ToString());
    }


    // 0 off
    // 1 shielded
    // 2 shields broken
    // 3 x axis
    // 4 y axis
    // 5 z axis
    public void TriggerMothershipMode(int mode)
    {
        SendTrigger(mothershipModes[mode].ToString());
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
