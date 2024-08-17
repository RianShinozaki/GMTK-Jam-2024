using Godot;
using Godot.Collections;

[GlobalClass]
public partial class GrabberArm : AndroidTorso {
    public TriggerDetector InteractableDetector;

    [Export]
    Texture2D GrabTexture;

    [Export]
    string InteractionDetectorPath;

    [Export]
    Vector2 PickupLocation;

    float speedCache;
    AndroidInteractionBase interactionBase;
    BasicPhysicsObject heldObject;


    public override Array<Array> GetOptions => new Array<Array>() {
        new Array() {
            GrabTexture,
            Callable.From<Node>(GrabObject)
        }
    };

    void GrabObject(Node context) {
        if (interactionBase == null) {
            GD.Print("Loading");
            PackedScene scene = ResourceLoader.Load<PackedScene>(InteractionDetectorPath);
            interactionBase = (AndroidInteractionBase)scene.Instantiate();
            interactionBase.OnOptionClicked += OnInteractDone;
            context.AddChild(interactionBase);
        }

        if (context == null) {
            return;
        }

        //Make sure we're working with an AI Bot
        if (context is AiBotBase bot) {
            if (heldObject != null) {
                heldObject.GetParent().RemoveChild(heldObject);
                bot.GetParent().AddChild(heldObject);
                heldObject.UseGravity = true;
                heldObject.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
                heldObject.GlobalPosition = new Vector2(PickupLocation.X * bot.InputDirection, PickupLocation.Y) + bot.GlobalPosition;
                heldObject = null;
                return;
            }

            //Make the bot stop and wait for the pickable object to be picked
            speedCache = bot.InputSpeed;
            bot.InputSpeed = 0f;

            interactionBase.StartInteract();
        }
    }

    void OnInteractDone(Node context) {
        BasicPhysicsObject pickup = (BasicPhysicsObject)context.GetParent();
        AiBotBase bot = (AiBotBase)interactionBase.GetParent();

        if (pickup.PickUpable) {
            pickup.GetParent().RemoveChild(pickup);
            bot.AddChild(pickup);
            pickup.Position = new Vector2(PickupLocation.X * bot.InputDirection, PickupLocation.Y);
            heldObject = pickup;
            pickup.Velocity = Vector2.Zero;
            pickup.UseGravity = false;
            pickup.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
        }

        bot.InputSpeed = speedCache;
    }
}
