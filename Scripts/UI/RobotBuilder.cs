using Godot;
using System;
using System.Text.RegularExpressions;

public enum AndroidPart {
	Head,
	Arms,
	Legs
}
public partial class RobotBuilder : Node2D
{
	public RobotViewer viewer;
	[Export] public Android currentAndroid;
	public static RobotBuilder Instance;
	[Export] Godot.Collections.Array<Control> partsGroup;
	public int currentPreset;
	[Export] public Control PresetList;
	public bool firstTime = true;

    public override void _Ready()
    {
        base._Ready();
		viewer = GetNode<RobotViewer>("RobotViewer");
		if(currentAndroid != null) {
			currentAndroid = (Android)currentAndroid.Duplicate();
		}
		Instance = this;
		HidePartsGroups();
    }

	public void ChangeParts(AndroidPart part, Resource res) {
		switch(part) {
			case AndroidPart.Head:
				currentAndroid.Head = (AndroidHead)res;
				break;
			case AndroidPart.Arms:
				currentAndroid.Arms = (AndroidTorso)res;
				break;
			case AndroidPart.Legs:
				currentAndroid.Legs = (AndroidLegs)res;
				break;
		}
		viewer.UpdateTextures(currentAndroid);
		RobotStorage.Instance.robots[currentPreset] = currentAndroid;
		PresetList.GetChild<PresetSpot>(currentPreset).UpdateTextures();
		RobotBuilderSpawner.Instance.CreateRobot(currentPreset);
	}
	public void ChangeWhole(Android android) {
		currentAndroid = android;
		viewer.UpdateTextures(currentAndroid);
		RobotBuilderSpawner.Instance.CreateRobot(currentPreset);

	}
	public void HidePartsGroups() {
		foreach(Control group in partsGroup) {
			group.Visible = false;
		}
	}
}
