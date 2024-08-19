using Godot;
using System;

public partial class MusicController : AudioStreamPlayer
{
    [Export]
    public AudioStreamPlayer[] Players;
    
    [Export]
    public float MinVolume;

    [Export]
    public float MaxVolume;

    public static MusicController Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance != null) 
        {
            QueueFree();
            return;
        }
        Instance = this;
    }

    public void PlayTrack(TrackPart track) 
    {
        foreach (var player in Players) 
        {
            player.Play();
            (player.GetStreamPlayback() as AudioStreamPlaybackInteractive).SwitchToClip((int)track);
        }
    }
    public MusicController SetTrack(TrackType type, TrackPart part) 
    {
        (Players[(int)type].GetStreamPlayback() as AudioStreamPlaybackInteractive).SwitchToClip((int)part);
        return this;
    }
    public MusicController SetVolume(TrackType type, float volume) 
    {
        float NormalizedVolume = volume * (MaxVolume - MinVolume) + MinVolume;
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Players[(int)type].Bus), NormalizedVolume);
        return this;
    }
    public MusicController Resume() 
    {
        foreach (var player in Players)
        {
            player.Play();
        }
        return this;
    }
    public MusicController Pause()
    {
        foreach (var player in Players)
        {
            player.Stop();
        }
        return this;
    }
}
public enum TrackType 
{
    Base = 0,
    Melody = 1,
    Percussion = 2,
}
public enum TrackPart
{
    Intro,
    LoopA,
    LoopB,
    Chorus,
    Workshop,
    LoopB2,
}