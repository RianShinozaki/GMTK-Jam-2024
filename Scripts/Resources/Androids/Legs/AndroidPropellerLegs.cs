using Godot;
using Godot.Collections;

[GlobalClass]
public partial class AndroidPropellerLegs : AndroidPiece {
    const uint SOLIDONLY = 1u;
	const uint SOLIDANDROPE = 3u;

    [Export]
    public Texture2D TurnIcon, StopIcon;
    
    float inputCache = -1f;

    public override Array<Array> GetOptions => new Array<Array> {
        new Array {
            TurnIcon,
            Callable.From<Node>(Turn)
        },
        new Array {
            StopIcon,
            Callable.From<Node>(Stop)
        }
    };

    public override void InitPiece(AiBotBase bot) {
        base.InitPiece(bot);
        bot.UpdateOverride = Update;
    }

    void Turn(Node context) {
        if (context is AiBotBase character && character.IsOnFloor()) {
            character.InputDirection *= -1f;
        }
    }
    
    void Stop(Node context) {
        if (context is AiBotBase character && character.IsOnFloor()) {
            if (character.InputSpeed != 0f) {
                inputCache = character.InputSpeed;
                character.InputSpeed = 0f;
                return;
            }

            character.InputSpeed = inputCache;
        } 
    }

    void Update(AiBotBase bot, float delta) {
        Vector2 velocity = bot.Velocity;

		if (!bot.IsOnFloor() && bot.InputSpeed == 0f) {
			velocity += bot.GetGravity() * delta;
		}

		float desiredDirection = Mathf.Clamp(bot.InputSpeed, 0f, 1f) * Mathf.Sign(bot.InputDirection);
		desiredDirection *= bot.AndroidBase.Legs.BaseMovementSpeed;
		desiredDirection *= bot.AndroidBase.GetWeightSpeedReduction();

		//Accel to max speed
		velocity.X = Mathf.MoveToward(velocity.X, desiredDirection, (float)delta * bot.AndroidBase.Legs.BaseMovementSpeed);

		//Push all velocity changes to CharacterController Velocity vec
		bot.Velocity = velocity;
		bot.MoveAndSlide();
    }
}
