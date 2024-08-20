using Godot;
using System;

[GlobalClass]
public partial class Android : Resource {
	[Export]
	public float BaseMovementSpeed;

	public float GetStrength() {
		//Get Average of strengths
		float str = Head.Strength + Arms.Strength + Legs.Strength;
		str /= 3f;

		//Do magic math I figured out
		str = Mathf.Log(str)+2.3f;
		str /= 2.3f;
	
		return str;
	}

	public float GetWeightSpeedReduction() => Head.WeightSpeedReductionAmount * Arms.WeightSpeedReductionAmount * Legs.WeightSpeedReductionAmount;

	[Export]
	public AndroidHead Head;

	[Export]
	public AndroidTorso Arms;

	[Export]
	public AndroidLegs Legs;
}
