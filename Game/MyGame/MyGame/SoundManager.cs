using System;
using System.Collections.Generic;
using GXPEngine;

namespace MyGame.MyGame;

public class SoundManager
{
    private static readonly string _path = "../../assets/SFX.Draft2";
    
    public static Sound music { get; set; }
    public static Sound jump { get; set; }
    public static Sound dash { get; set; }

    public SoundManager()
    {
        music = new Sound($"${_path}/POL-lone-wolf-short.wav", true);
        jump = new Sound($"{_path}/Dash.wav");
        dash = new Sound($"{_path}/jump.wav");
    }
}