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
	Variant[] highlightCache;

	[Signal]
	public delegate void OnOptionClickedEventHandler(Node context);

    public virtual void InteractExitRange(Area2D area) {
        if (area is AndroidInteractableArea interactable) {
			interacts.Remove(interactable);
		}
    }


    public virtual void InteractEnterRange(Area2D area) {
        if (area is AndroidInteractableArea interactable && !interacts.Contains(interactable)) {
			interacts.Add(interactable);

			if (highlightCache != null) {
				highlightCache = highlightCache.Append(interactable).ToArray();
				((Node2D)interactable.GetParent()).Modulate = InteractionColor;
				interactable.GetNode<ClickDetector>("ClickDetector").OnClick += InteractableClicked;
			}
		}
    }

	public void HighlightInteracts() {
		highlightCache = new Variant[interacts.Count];
		interacts.CopyTo(highlightCache, 0);

		for (int i = 0; i < highlightCache.Length; i++) {
			Node2D node = (Node2D)highlightCache[i].As<Node>().GetParent();
			node.Modulate = InteractionColor;
		}
	}

	public void StartInteract() {
		HighlightInteracts();

		for (int i = 0; i < highlightCache.Length; i++) {
			AndroidInteractableArea interactable = (AndroidInteractableArea)highlightCache[i].As<Node2D>();
			interactable.GetNode<ClickDetector>("ClickDetector").OnClick += InteractableClicked;
		}
	}

	public void EndHighlight() {
		for (int i = 0; i < highlightCache.Length; i++) {
			((Node2D)highlightCache[i].As<Node>().GetParent()).Modulate = Colors.White;
		}

		highlightCache = null;
	}

	void InteractableClicked(Node context) {
		if (context is AndroidInteractableArea interaction) {
			interaction.Interact();

			for (int i = 0; i < highlightCache.Length; i++) {
				highlightCache[i].As<Node2D>().GetNode<ClickDetector>("ClickDetector").OnClick -= InteractableClicked;
			}
		}

		EndHighlight();
		EmitSignal(SignalName.OnOptionClicked, context);
	}

}
