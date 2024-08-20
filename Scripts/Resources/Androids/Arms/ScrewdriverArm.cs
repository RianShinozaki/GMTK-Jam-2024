using System.Text.Json.Serialization;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class ScrewdriverArm : AndroidTorso {
    [Export]
    Color InteractColor = Colors.Green;

    [Export]
    Texture2D ScrewTexture;

    [Export(PropertyHint.Layers2DPhysics)]
    uint Layer = 512u;

    [Export]
    string InteractionDetectorPath;

    float speedCache;
    AndroidInteractionBase interactionBase;
    Timer timer;
    AiBotBase bot;
    BasicPhysicsObject objCache;

    public override Array<Array> GetOptions => new Array<Array>() {
        new Array() {
            ScrewTexture,
            Callable.From<Node>(ScrewObject)
        }
    };

    void ScrewObject(Node context) {
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
    }

    void OnInteractDone(Node context, bool success) {
        bot ??= (AiBotBase)interactionBase.GetParent();

        if (!success) {
            OnTimerDone();
            return;
        }

        objCache = (BasicPhysicsObject)context.GetParent();
        timer.Start();
    }

    void OnTimerDone() {
        if (objCache != null) {
            objCache.Set("Screwed", true);
            objCache = null;
        }

        bot.InputSpeed = speedCache;
        timer.Stop();
    }
}
