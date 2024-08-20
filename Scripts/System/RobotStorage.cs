using Godot;
using System;

public partial class RobotStorage : Node
{
	[Export]
	public Godot.Collections.Array<Android> robots;
	public int currentSelectedPreset = 0;

	public static RobotStorage Instance;
	public override void _Ready() {
		Instance = this;
		for(int i = 0; i < 5; i++) {
			robots[i] = (Android)robots[i].Duplicate();
		}
	}
}
