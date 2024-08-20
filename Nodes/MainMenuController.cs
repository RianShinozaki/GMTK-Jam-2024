using Godot;
using System;

public partial class MainMenuController : Control
{
    [Export]
    public Control MainMenu;

    [Export]
    public Control SettingsMenu;

    [Export] 
    public CheckBox Fullscreen;

    [Export] 
    public CheckBox VSync;

    [Export] 
    public HSlider Master;

    [Export] 
    public HSlider Music;

    [Export] 
    public HSlider SFX;

    [Export]
    public float MinVolume = -80f;

    [Export]
    public float MaxVolume = 6f;

    public void ShowSettingsMenu() 
    {
        MainMenu.Hide();
        SettingsMenu.Show();
    }
    public void ShowMainMenu() 
    {
        SettingsMenu.Hide();
        MainMenu.Show();
    }
    public void ApplySettings() 
    {
        float MasterNormalizedVolume = (float)Master.Value * (MaxVolume - MinVolume) + MinVolume;
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), MasterNormalizedVolume);

        float MusicNormalizedVolume = (float)Music.Value * (MaxVolume - MinVolume) + MinVolume;
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music (Master)"), MusicNormalizedVolume);

        float SFXNormalizedVolume = (float)SFX.Value * (MaxVolume - MinVolume) + MinVolume;
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), SFXNormalizedVolume);

        if (Fullscreen.ButtonPressed)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else 
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }

        if (VSync.ButtonPressed)
        {
            DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
        }
        else 
        {
            DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
        }
    }
    public void ExitGame() 
    {
        GetTree().Quit();
    }
    public void PlayGame() 
    {

    }
}
