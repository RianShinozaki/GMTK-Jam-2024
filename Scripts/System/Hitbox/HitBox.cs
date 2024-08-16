using Godot;
using Godot.Collections;

[GlobalClass]
public partial class HitBox : Area2D {
    [Export]
	public Dictionary StatusEffects;

    public override void _Ready() {
        base._Ready();
        StatusEffects ??= new Dictionary();
    }
}
