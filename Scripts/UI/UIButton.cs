using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class UIButton : TextureRect
{
	[Export] PartsGroup myGroup;
	[Export] TextureRect arrow;
	[Export] int myY;
    public override void _GuiInput(InputEvent @event)
    {
        base._GuiInput(@event);
		if(@event is InputEventMouseButton mouseEv) {
			if(mouseEv.ButtonIndex == MouseButton.Left && mouseEv.Pressed) {
				RobotBuilder.Instance.HidePartsGroups();
				myGroup.Visible = true;
				arrow.Position = new Vector2(arrow.Position.X, myY);
			}
		}
    }

}
