using Godot;
using System;

[GlobalClass]
public partial class AndroidInteractableArea : TriggerShape {

	[Signal]
	public delegate void OnInteractEventHandler();

	public virtual void Interact() {
		EmitSignal(SignalName.OnInteract);
	 }
}
