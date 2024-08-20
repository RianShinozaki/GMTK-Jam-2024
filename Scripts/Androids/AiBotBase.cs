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
	QuickButtonContextMenu ArmsContextMenu;
	[Export]
	QuickButtonContextMenu LegsContextMenu;

	[ExportGroup("Art")]
	[Export]
	Node2D Sprite;
	[Export]
	AnimatedSprite2D HeadSprite;
	[Export]
	AnimatedSprite2D LegsSprite;
	[Export]
	AnimatedSprite2D ArmsSprite;

	const uint SOLIDONLY = 1u;
	const uint SOLIDANDROPE = 3u;

	double systemTime;
	double lastTurnAround;

	public System.Action<AiBotBase, float> UpdateOverride;

	[Signal]
	public delegate void BotProcessEventHandler(float delta);

	public void Init() {
		AndroidBase = (Android)AndroidBase.Duplicate(true);
		Array<Array> headData = AndroidBase.Head.GetOptions;
		Array<Array> armsData = AndroidBase.Arms.GetOptions;
		Array<Array> legsData = AndroidBase.Legs.GetOptions;

		//Push all limb options to the menus on the limbs
		for (int i = 0; i < headData.Count; i++) {
			HeadContextMenu.AddOption(headData[i][0].As<Texture2D>(), "HeadOption"+i, headData[i][1].As<Callable>());
		}

		for (int i = 0; i < armsData.Count; i++) {
			Texture2D tex = armsData[i][0].As<Texture2D>();
			string name = "ArmsOption"+i;
			Callable call = armsData[i][1].As<Callable>();
			ArmsContextMenu.AddOption(tex, name, call);
		}

		for (int i = 0; i < legsData.Count; i++) {
			LegsContextMenu.AddOption(legsData[i][0].As<Texture2D>(), "LegsOption"+i, legsData[i][1].As<Callable>());
		}

		HeadContextMenu.OnOptionClicked += OnContextOptionClicked;
		ArmsContextMenu.OnOptionClicked += OnContextOptionClicked;
		LegsContextMenu.OnOptionClicked += OnContextOptionClicked;

		HeadSprite.SpriteFrames = AndroidBase.Head.SpriteTexture;
		ArmsSprite.SpriteFrames = AndroidBase.Arms.SpriteTexture;
		LegsSprite.SpriteFrames = AndroidBase.Legs.SpriteTexture;

		AndroidBase.Head.InitPiece(this);
		AndroidBase.Arms.InitPiece(this);
		AndroidBase.Legs.InitPiece(this);
    }

	public override void _Process(double delta) {
		base._Process(delta);
		systemTime += delta;

		//Sure why not
		if (InputDirection != 0f) {
			Sprite.Scale = new Vector2(Mathf.Sign(InputDirection), 1f);
		}
	}


	//Movement and Physics
	//InputDirection is part of autonomous control
    public override void _PhysicsProcess(double delta) {
		//Turn around on walls
		if (IsOnWall() && systemTime - lastTurnAround >= 0.5f) {
			InputDirection *= -1f;
			lastTurnAround = systemTime;
		}

		if(Velocity.X == 0) 
			CollisionMask = SOLIDONLY;
		else
			CollisionMask = SOLIDANDROPE;

		if (UpdateOverride != null) {
			UpdateOverride.Invoke(this, (float)delta);
			return;
		}

		DefaultUpdate((float)delta);
	}


	//Hide menu options when clicked
	void OnContextOptionClicked() {
		HeadContextMenu.HideOptions();
		ArmsContextMenu.HideOptions();
		LegsContextMenu.HideOptions();
	}

	void DefaultUpdate(float delta) {
		Vector2 velocity = Velocity;

		if (!IsOnFloor()) {
			velocity += GetGravity() * delta;
		}

		float desiredDirection = Mathf.Clamp(InputSpeed, 0f, 1f) * Mathf.Sign(InputDirection);
		desiredDirection *= AndroidBase.Legs.BaseMovementSpeed;
		desiredDirection *= AndroidBase.GetWeightSpeedReduction();

		//Accel to max speed
		velocity.X = Mathf.MoveToward(velocity.X, desiredDirection, (float)delta * AndroidBase.Legs.BaseMovementSpeed);

		//Push all velocity changes to CharacterController Velocity vec
		Velocity = velocity;
		MoveAndSlide();
	}
}
