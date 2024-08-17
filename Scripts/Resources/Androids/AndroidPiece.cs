using Godot;
using System;

[GlobalClass]
public abstract partial class AndroidPiece : Resource {
	[Export]
	public float Weight;

	[Export]
	public float Strength;
}
