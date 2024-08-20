using Godot;
using System;

public partial class DestroyableStation : BasicPhysicsObject
{
	[Export] public bool Burned;
	[Export] public bool Screwed;

	Node2D station;

	[Export] public bool shouldBeBurned;
	[Export] public bool shouldBeScrewed;

    public override void _Ready()
    {
		station = GetParent<Node2D>();
        base._Ready();
    }
    public override void _Process(double delta)
    {
		if(Screwed && shouldBeScrewed || Burned && shouldBeBurned) {
			station.Call("_destroy_station");
			GD.Print("woaw");
		}
        base._Process(delta);
    }

}
