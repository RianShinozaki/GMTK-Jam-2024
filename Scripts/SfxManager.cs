using Godot;
using System;

public partial class SfxManager : Node
{
    public static SfxManager Instance;

    [Export]
    public AudioStream[] ChefStreams;

    [Export]
    public AudioStream[] RobotStreams;

    [Export]
    public AudioStream[] GeneralStreams;

    [Export]
    public AudioStreamPlayer ChefPlayer;

    [Export]
    public AudioStreamPlayer RobotPlayer;

    [Export]
    public AudioStreamPlayer GeneralPlayer;

    [Export]
    public float MinVolume = -80f;

    [Export]
    public float MaxVolume = 6f;

    public override void _Ready()
    {
        if (Instance != null) {
            QueueFree();
            return;
        }
        Instance = this;
        PlayChefWalking();
    }
    public void PlayChefWalking() 
    {
        ChefPlayer.Stop();
        ChefPlayer.Stream = ChefStreams[0];
        ChefPlayer.Play();
    }
    public void PlayChefLadder()
    {
        ChefPlayer.Stop();
        ChefPlayer.Stream = ChefStreams[1];
        ChefPlayer.Play();
    }
    public void PlayChefStirring()
    {
        ChefPlayer.Stop();
        ChefPlayer.Stream = ChefStreams[2];
        ChefPlayer.Play();
    }
    public void PlayChefChopping()
    {
        ChefPlayer.Stop();
        ChefPlayer.Stream = ChefStreams[3];
        ChefPlayer.Play();
    }
    public void PlayRobotWheel() 
    {
        RobotPlayer.Stop();
        RobotPlayer.Stream = RobotStreams[0];
        RobotPlayer.Play();
    }
    public void PlayRobotGrab()
    {
        RobotPlayer.Stop();
        RobotPlayer.Stream = RobotStreams[1];
        RobotPlayer.Play();
    }
    public void PlayShattering() 
    {
        GeneralPlayer.Stop();
        GeneralPlayer.Stream = GeneralStreams[0];
        GeneralPlayer.Play();
    }
    public void PlayBubbling()
    {
        GeneralPlayer.Stop();
        GeneralPlayer.Stream = GeneralStreams[1];
        GeneralPlayer.Play();
    }
    public void SetSFXVolume(float volume) 
    {
        float NormalizedVolume = volume * (MaxVolume - MinVolume) + MinVolume;
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), NormalizedVolume);
    }
    public void StopChefSFX() 
    {
        ChefPlayer.Stop();
    }
    public void StopRobotSFX()
    {
        RobotPlayer.Stop();
    }
    public void StopGeneralSFX()
    {
        ChefPlayer.Stop();
    }

    public void StartChefSFX()
    {
        ChefPlayer.Play();
    }
    public void StartRobotSFX()
    {
        RobotPlayer.Play();
    }
    public void StartGeneralSFX()
    {
        ChefPlayer.Play();
    }
    public void StopAll() 
    {
        ChefPlayer.Stop();
        RobotPlayer.Stop();
        GeneralPlayer.Stop();
    }
    public void StartAll()
    {
        ChefPlayer.Play();
        RobotPlayer.Play();
        GeneralPlayer.Play();
    }
}
