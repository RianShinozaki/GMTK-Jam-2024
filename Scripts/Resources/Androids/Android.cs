using Godot;
using System;

[GlobalClass]
public partial class Android : Resource {
	[Export]
	public float BaseMovementSpeed;

	public float GetStrength() {
		//Get Average of strengths
		float str = Head.Strength + Torso.Strength + Legs.Strength;
		str /= 3f;

		//Do magic math I figured out
		str = Mathf.Log(str)+2.3f;
		str /= 2.3f;
	
		return str;
	}

	public float GetWeightSpeedReduction() => Head.WeightSpeedReductionAmount * Torso.WeightSpeedReductionAmount * Legs.WeightSpeedReductionAmount;

	[Export]
	public AndroidHead Head;

	[Export]
	public AndroidTorso Torso;

	[Export]
	public AndroidLegs Legs;
}
