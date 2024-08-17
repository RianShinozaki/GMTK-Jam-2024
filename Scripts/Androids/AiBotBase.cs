using Godot;
using Godot.Collections;

public partial class AiBotBase : CharacterBody2D {
	[Export]
	public Android AndroidBase;

	public float InputSpeed = 1f;
	public float InputDirection = -1f;

	[ExportGroup("Pieces")]
	[Export]
	QuickButtonContextMenu HeadContextMenu;
	[Export]
	QuickButtonContextMenu TorsoContextMenu;
	[Export]
	QuickButtonContextMenu LegsContextMenu;

	[ExportGroup("Art")]
	[Export]
	Node2D Sprite;

	double systemTime;
	double lastTurnAround;

    public override void _Ready() {
        base._Ready();

		Array<Array> headData = AndroidBase.Head.GetOptions;
		Array<Array> torsoData = AndroidBase.Torso.GetOptions;
		Array<Array> legsData = AndroidBase.Legs.GetOptions;

		for (int i = 0; i < headData.Count; i++) {
			HeadContextMenu.AddOption(headData[i][0].As<Texture2D>(), "HeadOption"+i, headData[i][1].As<Callable>());
		}

		for (int i = 0; i < torsoData.Count; i++) {
			TorsoContextMenu.AddOption(torsoData[i][0].As<Texture2D>(), "HeadOption"+i, torsoData[i][1].As<Callable>());
		}

		for (int i = 0; i < legsData.Count; i++) {
			LegsContextMenu.AddOption(legsData[i][0].As<Texture2D>(), "HeadOption"+i, legsData[i][1].As<Callable>());
		}

		HeadContextMenu.OnOptionClicked += OnContextOptionClicked;
		TorsoContextMenu.OnOptionClicked += OnContextOptionClicked;
		LegsContextMenu.OnOptionClicked += OnContextOptionClicked;
    }

    public override void _Process(double delta) {
        base._Process(delta);
		systemTime += delta;

		if (InputDirection != 0f) {
			Sprite.Scale = new Vector2(Mathf.Sign(InputDirection), 1f);
		}
    }


    public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		if (!IsOnFloor()) {
			velocity += GetGravity() * (float)delta;
		}

		float desiredDirection = Mathf.Clamp(InputSpeed, 0f, 1f) * Mathf.Sign(InputDirection);
		desiredDirection *= AndroidBase.BaseMovementSpeed;
		desiredDirection *= AndroidBase.GetWeightSpeedReduction();

		velocity.X = Mathf.MoveToward(velocity.X, desiredDirection, (float)delta * AndroidBase.BaseMovementSpeed);

		if (IsOnWall() && systemTime - lastTurnAround >= 0.5f) {
			InputDirection *= -1f;
			lastTurnAround = systemTime;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	void OnContextOptionClicked() {
		HeadContextMenu.HideOptions();
		TorsoContextMenu.HideOptions();
		LegsContextMenu.HideOptions();
	}
}
