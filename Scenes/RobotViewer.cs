using Godot;
using System;

public partial class RobotViewer : Control
{
	TextureRect Head;
	TextureRect Arms;
	TextureRect Torso;
	TextureRect Legs;

	[Export]
	Android android;

	public override void _Ready()
	{
		Head = GetNode<TextureRect>("Head");
		Torso = GetNode<TextureRect>("Torso");
		Arms = GetNode<TextureRect>("Arms");
		Legs = GetNode<TextureRect>("Legs");
		UpdateTextures(android);
	}

	public void UpdateTextures(Android newAndroid) {
		android = newAndroid;
		Head.Texture = android.Head.SpriteTexture.GetFrameTexture("default", 0);
		Arms.Texture = android.Arms.SpriteTexture.GetFrameTexture("default", 0);
		Legs.Texture = android.Legs.SpriteTexture.GetFrameTexture("default", 0);
	}
}
