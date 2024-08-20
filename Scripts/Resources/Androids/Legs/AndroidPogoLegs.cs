using Godot;
using Godot.Collections;

[GlobalClass]
public partial class AndroidPogoLegs : AndroidLegs {
    [Export]
    float pogoWaitTime = 0.3f;

    [Export]
    float jumpForce = 16f;
    
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

    AnimatedSprite2D pogoSprite;
    float systemTime;
    float waitTimeStart;
    int state = 1;

    public override void InitPiece(AiBotBase bot) {
        base.InitPiece(bot);
        bot.UpdateOverride = Update;
        pogoSprite = bot.GetNode<AnimatedSprite2D>("Robot/LegsSprite");
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
        systemTime += delta;

        Vector2 velocity = bot.Velocity;

		if (!bot.IsOnFloor()) {
			velocity += bot.GetGravity() * delta;
		}

		switch(state) {
            case 0:
            MoveState_WaitForPogo(bot, ref velocity);
            break;
            case 1:
            MoveState_WaitForGround(bot, ref velocity);
            break;
        }

		//Push all velocity changes to CharacterController Velocity vec
		bot.Velocity = velocity;
		bot.MoveAndSlide();
    }

    void MoveState_WaitForPogo(AiBotBase bot, ref Vector2 velocty) {
        if (systemTime - waitTimeStart < pogoWaitTime && bot.InputSpeed != 0f) {
            velocty.X = 0f;
            return;
        }

        state++;
        velocty.Y = -jumpForce;
        pogoSprite.Play("pogo");
        pogoSprite.AnimationFinished += PogoAnimEnd;
    }

    void MoveState_WaitForGround(AiBotBase bot, ref Vector2 velocty) {
        float desiredDirection = Mathf.Clamp(bot.InputSpeed, 0f, 1f) * Mathf.Sign(bot.InputDirection);
		desiredDirection *= BaseMovementSpeed;
		desiredDirection *= bot.AndroidBase.GetWeightSpeedReduction();

        velocty.X = desiredDirection;

        if (bot.IsOnFloor()) {
            state--;
            waitTimeStart = systemTime;
            pogoSprite.PlayBackwards("pogo");
            pogoSprite.AnimationFinished += RetractAnimEnd;
        }
    }

    void PogoAnimEnd() {
        pogoSprite.Play("extend_hold");
        pogoSprite.AnimationFinished -= PogoAnimEnd;
    }

    void RetractAnimEnd() {
        pogoSprite.Play("retract_hold");
        pogoSprite.AnimationFinished -= RetractAnimEnd;
    }
}
