using Godot;
using Godot.Collections;

[GlobalClass]
public partial class BasicPhysicsObject : CharacterBody2D {
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	[Export]
	public bool UseGravity = true;

	[ExportGroup("Interaction")]
	[Export]
	bool Interactable = true;

	[Export]
	public bool PickUpable = true;

	[Export]
	AndroidInteractableArea InteractionArea;

	[Export]
	Array InteractionDisables;

    public override void _Ready() {
        base._Ready();

		if (Interactable) {
			InteractionArea.OnInteract += OnInteract;
		}
    }

    public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		if (!IsOnFloor() && UseGravity) {
			velocity += GetGravity() * (float)delta;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	void OnInteract() {
		
	}
}
