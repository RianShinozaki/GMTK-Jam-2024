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

	public override void _Ready()
	{
		RefreshSettings();
	}
	private void RefreshSettings() 
	{
		if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
		{
			Fullscreen.ButtonPressed = true;
		}
		else
		{
			Fullscreen.ButtonPressed = false;
		}

		if (DisplayServer.WindowGetVsyncMode() == DisplayServer.VSyncMode.Enabled)
		{
			VSync.ButtonPressed = true;
		}
		else
		{
			VSync.ButtonPressed = false;
		}

		Master.Value = MapRange(AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Master")), -80f, 6f, 0f, 100f);
		Music.Value = MapRange(AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Music (Master)")), -80f, 6f, 0f, 100f);
		SFX.Value = MapRange(AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("SFX")), -80f, 6f, 0f, 100f);
	}

	public void ShowSettingsMenu() 
	{
		MainMenu.Hide();
		RefreshSettings();
		SettingsMenu.Show();
	}
	public void ShowMainMenu() 
	{
		SettingsMenu.Hide();
		MainMenu.Show();
	}
	public void ApplySettings() 
	{
		float MasterNormalizedVolume = ((float)Master.Value / 100f) * (MaxVolume - MinVolume) + MinVolume;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), MapRange((float)Master.Value, 0f, 100f, -80f, 6f));

		float MusicNormalizedVolume = (float)Music.Value / 100f * (MaxVolume - MinVolume) + MinVolume;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music (Master)"), MapRange((float)Music.Value, 0f, 100f, -80f, 6f));

		float SFXNormalizedVolume = (float)SFX.Value / 100f * (MaxVolume - MinVolume) + MinVolume;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), MapRange((float)SFX.Value, 0f, 100f, -80f, 6f));

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
		GetTree().ChangeSceneToFile("res://Scenes/RobotBuilder.tscn");
	}
	float MapRange(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
	{
		return newMin + ((oldValue - oldMin) / (oldMax - oldMin)) * (newMax - newMin);
	}
}
