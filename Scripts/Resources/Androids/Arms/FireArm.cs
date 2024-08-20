using System.Text.Json.Serialization;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class FireArm : AndroidTorso {
    [Export]
    Color InteractColor = Colors.Green;

    [Export]
    Texture2D BurnTexture;

    [Export(PropertyHint.Layers2DPhysics)]
    uint Layer = 1024u;

    [Export]
    string InteractionDetectorPath;

    float speedCache;
    AndroidInteractionBase interactionBase;
    Timer timer;
    AiBotBase bot;
    BasicPhysicsObject objCache;
    AnimatedSprite2D flamerSprite;

    public override Array<Array> GetOptions => new Array<Array>() {
        new Array() {
            BurnTexture,
            Callable.From<Node>(BurnObject)
        }
    };

    void BurnObject(Node context) {
        if (context is AiBotBase bot) {
            //Make the bot stop and wait for the pickable object to be picked
            speedCache = bot.InputSpeed;
            bot.InputSpeed = 0f;

            interactionBase.StartInteract();
        }
    }

    public override void InitPiece(AiBotBase bot) {
        base.InitPiece(bot);

        PackedScene scene = ResourceLoader.Load<PackedScene>(InteractionDetectorPath);
        interactionBase = (AndroidInteractionBase)scene.Instantiate();
        interactionBase.OnOptionClicked += OnInteractDone;
        interactionBase.InteractionColor = InteractColor;
        interactionBase.CollisionLayer = Layer;
        interactionBase.CollisionMask = Layer;
        bot.AddChild(interactionBase);

        timer = new Timer {
            WaitTime = 3f,
            Autostart = false
        };
        timer.Timeout += OnTimerDone;
        bot.AddChild(timer);

        flamerSprite = bot.GetNode<AnimatedSprite2D>("Robot/ArmSprite");
    }

    void OnInteractDone(Node context, bool success) {
        if (!success) {
            return;
        }

        bot ??= (AiBotBase)interactionBase.GetParent();
        objCache = (BasicPhysicsObject)context.GetParent();
        flamerSprite.Play("Fire-Start");
        flamerSprite.AnimationFinished += OnFlameStartDone;
        timer.Start();
    }

    void OnTimerDone() {
        objCache.Set("Burned", true);
        objCache = null;
        bot.InputSpeed = speedCache;
        flamerSprite.Play("default");
        timer.Stop();
    }

    void OnFlameStartDone() {
        flamerSprite.Play("Fire-Loop");
        flamerSprite.AnimationFinished -= OnFlameStartDone;
    }
}
