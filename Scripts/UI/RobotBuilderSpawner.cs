using Godot;
using System;

public partial class RobotBuilderSpawner : Node2D
{
	[Export] public string AIBotPath;
	AiBotBase currentBot;
	public static RobotBuilderSpawner Instance;
    public override void _Ready()
    {
        base._Ready();
		CreateRobot(0);
		Instance = this;
    }
    public void CreateRobot(int preset) {
		if(currentBot != null) {
			currentBot.QueueFree();
		}
		currentBot = (AiBotBase)GD.Load<PackedScene>(AIBotPath).Instantiate();
		currentBot.AndroidBase = RobotStorage.Instance.robots[preset];
		currentBot.Init();
		AddChild(currentBot);
	}
}
