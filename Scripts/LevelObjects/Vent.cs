using Godot;
using System;

public partial class Vent : Sprite2D
{
	public bool open = true;

	private void _on_click_detector_on_click(Node context) {
		RobotSpawner.Instance.CreateRobot(0, GlobalPosition);
	}
}
