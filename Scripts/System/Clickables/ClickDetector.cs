using Godot;
using System;

[GlobalClass]
public partial class ClickDetector : Area2D {
    [Export]
    public Node ContextNode;

    [Signal]
    public delegate void OnClickEventHandler(Node context);

    public override void _Ready() {
        base._Ready();

        InputEvent += OnInputEvent;
    }

    void OnInputEvent(Node viewport, InputEvent @event, long shapeID) {
        base._Input(@event);

        if (@event is InputEventMouseButton button && @event.IsPressed() && button.ButtonIndex == MouseButton.Left) {
            GD.Print(Name + " clicked!");
            EmitSignal(SignalName.OnClick, ContextNode);
        }
    }
}
