using System;
using System.IO.Ports;
using System.Linq;
using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame;

public static class Gamepad
{
	private const float DEAD_ZONE = 0.2f;

    private static readonly SerialPort SerialPort;
    public static Vector2 _joystick { get; private set; }

	static Gamepad()
	{
		try
		{
			SerialPort = new SerialPort();
			SerialPort.PortName = "COM3"; //Set your board COM
			SerialPort.BaudRate = 9600;
			SerialPort.Open();
			SerialPort.DataReceived += DataReceivedHandler;
		}
		catch (Exception)
		{
			Console.WriteLine("Using keyboard instead of custom gamepad");
			SerialPort = null;
		}

		_joystick = new Vector2(0, 0);
	}

	private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
	{
		float x = 0.0f;
		float y = 0.0f;

		SerialPort sp = (SerialPort)sender;
		// Console.WriteLine(sp.ReadLine());

		if (sp != null)
		{
			string val = sp.ReadLine().Trim();
			string[] lines = val.Split('\n');
			string[] split = lines.Last().Split(',');
			if (split.Length != 5) return;
			int valX = int.Parse(split[0].Trim());
			int valY = int.Parse(split[1].Trim());
			x = Mathf.Map(valX, 0, 1023, -1.0f, 1.0f);
			y = Mathf.Map(valY, 0, 1023, -1.0f, 1.0f);
			if (Mathf.Abs(x) < DEAD_ZONE) x = 0.0f;
			if (Mathf.Abs(y) < DEAD_ZONE) y = 0.0f;
		}
		_joystick = new Vector2(x, y).Limit(1.0f);
	}

	public static void Update()
	{
		if (SerialPort != null) return;

		float x = 0.0f;

		if (Input.GetKey(Key.A))
		{
			x = -1.0f;
		}
		else if (Input.GetKey(Key.D))
		{
			x = 1.0f;
		}

		_joystick = new Vector2(x, 0.0f).Limit(1.0f);
	}
}
