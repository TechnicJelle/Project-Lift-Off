using System.IO.Ports;
using System.Linq;
using GXPEngine;
using GXPEngine.Core;

namespace MyGame.MyGame;

public static class Gamepad
{
    private static readonly SerialPort SerialPort;

	private const float DEAD_ZONE = 0.2f;

	public static Vector2 _joystick { get; private set; }

	static Gamepad()
	{
		SerialPort = new SerialPort();
		SerialPort.PortName = "COM3"; //Set your board COM
		SerialPort.BaudRate = 9600;
		SerialPort.Open();

		_joystick = new Vector2(0, 0);
	}

	public static void Update()
	{
		string val = SerialPort.ReadExisting().Trim();
		string[] lines = val.Split('\n');
		string[] split = lines.Last().Split(',');
		if (split.Length != 3) return;
		int valX = int.Parse(split[0].Trim());
		int valY = int.Parse(split[1].Trim());
		float x = Mathf.Map(valX, 0, 1023, -1.0f, 1.0f);
		float y = Mathf.Map(valY, 0, 1023, -1.0f, 1.0f);
		if (Mathf.Abs(x) < DEAD_ZONE) x = 0.0f;
		if (Mathf.Abs(y) < DEAD_ZONE) y = 0.0f;
		_joystick = new Vector2(x, y).Limit(1.0f);
	}
}
