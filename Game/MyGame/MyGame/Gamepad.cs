using System;
using System.IO.Ports;
using System.Threading;
using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame;

public static class Gamepad
{
	private static SerialPort _serialPort;

	private const float DEAD_ZONE = 0.2f;
	private const float DIST = 150f;

	public static Vector2 _joystick { get; private set; }

	static Gamepad()
	{
		_serialPort = new SerialPort();
		_serialPort.PortName = "COM4"; //Set your board COM
		_serialPort.BaudRate = 9600;
		_serialPort.Open();

		_joystick = new Vector2(0, 0);
	}

	public static void Update()
	{
		string val = _serialPort.ReadExisting().Trim();
		Console.WriteLine(val);
		int[] values = Array.ConvertAll(val.Split(','), int.Parse);
		Console.WriteLine(values);
		float x = Mathf.Map(values[0], 0, 1023, -DIST, DIST);
		if (x < DEAD_ZONE) x = 0.0f;
		float y = Mathf.Map(values[1], 0, 1023, -DIST, DIST);
		if (y < DEAD_ZONE) y = 0.0f;
		_joystick = new Vector2(x, y).Limit(1);
	}
}
