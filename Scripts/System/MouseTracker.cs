using Godot;
using System;

public partial class MouseTracker : Node2D {

    public Vector2 MousePosition;

    public Vector2 MouseDelta {
        get => delta;
        set {
            LastMouseMovement = delta;
            delta = value;
        }
    }
    Vector2 delta;

    public Vector2 LastMouseMovement;

    public override void _Input(InputEvent @event) {
        base._Input(@event);

        if (@event is InputEventMouseMotion motion) {
            MousePosition = motion.GlobalPosition;
            MouseDelta = motion.Relative;
        }
    }

    public override void _Process(double delta) {
        base._Process(delta);

        if (MouseDelta == Vector2.Zero) {
            return;
        }

        GlobalPosition = MousePosition;

        Callable.From(() => {
            MouseDelta = Vector2.Zero;
        }).CallDeferred();
    }
}
