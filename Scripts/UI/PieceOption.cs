using Godot;
using System;

public partial class PieceOption : TextureRect
{
	public AndroidPiece myPiece;
	public AndroidPart myPart;

	public void Init() {
		Texture = myPiece.SpriteTexture.GetFrameTexture("default", 0);
	}

	public override void _GuiInput(InputEvent @event)
    {
		if(@event is InputEventMouseButton mouseEv) {
			if(mouseEv.ButtonIndex == MouseButton.Left && mouseEv.Pressed) {
        		RobotBuilder.Instance.ChangeParts(myPart, myPiece);
			}
		}
    }
}
