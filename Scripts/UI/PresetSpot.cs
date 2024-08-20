using Godot;
using System;

public partial class PresetSpot : TextureRect
{
	[Export] int myIndex;
	RobotViewer myRoboViewer;

	public override void _Ready() {
		myRoboViewer = GetNode<RobotViewer>("RobotViewer");
		myRoboViewer.UpdateTextures(RobotStorage.Instance.robots[myIndex]);
	}

	public override void _GuiInput(InputEvent @event)
    {
		
		if(@event is InputEventMouseButton mouseEv) {
			if(mouseEv.ButtonIndex == MouseButton.Left && mouseEv.Pressed) {
				GD.Print("hello");
				RobotBuilder.Instance.currentPreset = myIndex;
				RobotBuilder.Instance.ChangeWhole(RobotStorage.Instance.robots[myIndex]);
			}
		}
    }
	public void UpdateTextures() {
		myRoboViewer.UpdateTextures(RobotStorage.Instance.robots[myIndex]);
	}
}
