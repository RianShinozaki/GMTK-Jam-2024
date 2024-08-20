using Godot;
using System;

public partial class RobotSpawner : Node2D
{
	[Export] public string AIBotPath;
	AiBotBase currentBot;
	public static RobotSpawner Instance;
    public override void _Ready()
    {
        base._Ready();
		Instance = this;
    }
    public void CreateRobot(int preset, Vector2 Location) {

		currentBot = (AiBotBase)GD.Load<PackedScene>(AIBotPath).Instantiate();
		currentBot.AndroidBase = RobotStorage.Instance.robots[preset];
		currentBot.Init();
		AddChild(currentBot);
		currentBot.GlobalPosition = Location;

	}
}
