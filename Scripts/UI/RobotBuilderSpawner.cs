using Godot;
using System.Collections.Generic;

public partial class RobotBuilderSpawner : Node2D
{
	[Export] public string AIBotPath;
	Queue<AiBotBase> botQueue = new Queue<AiBotBase>();
	public static RobotBuilderSpawner Instance;
    public override void _Ready() {
        base._Ready();
		CreateRobot(0);
		Instance = this;
    }

    public void CreateRobot(int preset) {
		if(botQueue.Count >= 5) {
			botQueue.Dequeue().QueueFree();
		}
		AiBotBase currentBot = (AiBotBase)GD.Load<PackedScene>(AIBotPath).Instantiate();
		currentBot.AndroidBase = RobotStorage.Instance.robots[preset];
		currentBot.Init();
		AddChild(currentBot);
		botQueue.Enqueue(currentBot);
	}
}
