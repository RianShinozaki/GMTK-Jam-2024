using System.Linq;
using Godot;
using Godot.Collections;

public partial class AndroidInteractionBase : TriggerDetector {
	[Export]
	public Color InteractionColor = Colors.Aqua;

    public override void _Ready() {
        base._Ready();

		AreaEntered += InteractEnterRange;
		AreaExited += InteractExitRange;
		InputPickable = false;
    }

	Array interacts = new Array();
	Array highlightCache;

	[Signal]
	public delegate void OnOptionClickedEventHandler(Node context, bool success);

    public virtual void InteractExitRange(Area2D area) {
        if (area is AndroidInteractableArea interactable) {
			if (highlightCache != null) {
				ClickDetector click = interactable.GetNode<ClickDetector>("ClickDetector");
				click.OnClick -= InteractableClicked;
				click.InputPickable = false;
				highlightCache.Remove(interactable);
			}

			interacts.Remove(interactable);
		}
    }


    public virtual void InteractEnterRange(Area2D area) {
        if (area is AndroidInteractableArea interactable && !interacts.Contains(interactable)) {
			interacts.Add(interactable);

			if (highlightCache != null) {
				highlightCache.Add(interactable);
				((Node2D)interactable.GetParent()).Modulate = InteractionColor;
				ClickDetector click = interactable.GetNode<ClickDetector>("ClickDetector");
				click.OnClick += InteractableClicked;
				click.InputPickable = true;
			}
		}
    }

	public void HighlightInteracts() {
		highlightCache = new Array();
		
		for (int i = 0; i < interacts.Count; i++) {
			highlightCache.Add(interacts[i]);
		}

		for (int i = 0; i < highlightCache.Count; i++) {
			Node2D node = (Node2D)highlightCache[i].As<Node>().GetParent();
			node.Modulate = InteractionColor;
		}
	}

	public void StartInteract() {
		HighlightInteracts();

		for (int i = 0; i < highlightCache.Count; i++) {
			AndroidInteractableArea interactable = (AndroidInteractableArea)highlightCache[i].As<Node2D>();
			ClickDetector click = interactable.GetNode<ClickDetector>("ClickDetector");
			click.OnClick += InteractableClicked;
			click.InputPickable = true;
		}
	}

	public void EndHighlight() {
		for (int i = 0; i < highlightCache.Count; i++) {
			((Node2D)highlightCache[i].As<Node>().GetParent()).Modulate = Colors.White;
		}

		highlightCache = null;
	}

	void InteractableClicked(Node context) {
		if (context is AndroidInteractableArea interaction) {
			interaction.Interact();

			for (int i = 0; i < highlightCache.Count; i++) {
				ClickDetector click = highlightCache[i].As<Node2D>().GetNode<ClickDetector>("ClickDetector");
				click.OnClick -= InteractableClicked;
				click.InputPickable = false;
			}
		}

		EndHighlight();
		EmitSignal(SignalName.OnOptionClicked, context, true);
	}

	//Right click to cancel menu
    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseEv) {
            if (mouseEv.ButtonIndex == MouseButton.Right) {
                EndHighlight();
				EmitSignal(SignalName.OnOptionClicked, -1, false);

            }
        }
        base._Input(@event);
    }

}
