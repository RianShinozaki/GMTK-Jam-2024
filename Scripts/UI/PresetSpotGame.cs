using Godot;
using System;

public partial class PresetSpotGame : TextureRect
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
				RobotStorage.Instance.currentSelectedPreset = myIndex;
			}
		}
    }
}
