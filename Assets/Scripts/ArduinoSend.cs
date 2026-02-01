using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoSend : SingletonBehavior<ArduinoSend>
{
    private SerialPort serialPort;
    public string portName = "COM5"; // Change to your Arduino's port (e.g., "COM3" or "/dev/ttyACM0")
    public int baudRate = 115200;
    public List<char> hoopTogglesOn;
    public List<char> hoopTogglesOff;
    public List<char> mothershipModes;

    protected override void Awake()
    {
        base.Awake();
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        serialPort.WriteTimeout = 500;
    }

    public void ToggleHoop(int id, bool on)
    {
        List<char> dict = on ? hoopTogglesOn : hoopTogglesOff;
        SendTrigger(dict[id].ToString());
    }


    // 0 blue glow
    // 1 green glow
    // 2 x axis
    // 3 y axis
    // 4 z axis
    // 5 off
    public void TriggerMothershipMode(int mode)
    {
        SendTrigger(mothershipModes[mode].ToString());
    }

    public void SendTrigger(string command)
    {
        Debug.Log("Attempting to send " + command);
        try
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write(command); // Send the command
                Debug.Log("Sent: " + command);
            }
        }
        catch(System.Exception e)
        {
            Debug.LogWarning("ARDUINO SEND ERROR: " + e.Message);
        }
    }
    public void OnApplicationQuit() {
        if (serialPort != null && serialPort.IsOpen) {
            serialPort.Close();
        }
    }

}
