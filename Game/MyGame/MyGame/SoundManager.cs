using GXPEngine;

namespace MyGame.MyGame;

public static class SoundManager
{
	private const string PATH = "../../assets/SFX.Draft2/";

	public static Sound music { get; private set; }
	public static Sound jump { get; private set; }
	public static Sound dash { get; private set; }
	public static Sound explosion { get; private set; }
	public static Sound loseLife { get; private set; }
	public static Sound weaponSwoosh { get; private set; }
	public static Sound shooter { get; private set; }

	public static void LoadAllSounds()
	{
		music = new Sound(PATH + "POL-lone-wolf-short.wav", true, true);
		jump = new Sound(PATH + "Dash.wav");
		dash = new Sound(PATH + "jump.wav");
		explosion = new Sound(PATH + "Explosion.wav");
		loseLife = new Sound(PATH + "losingLife.wav");
		weaponSwoosh = new Sound(PATH + "Weapon Swoosh 1.wav");
		shooter = new Sound(PATH + "Shooter.wav");
	}
}
