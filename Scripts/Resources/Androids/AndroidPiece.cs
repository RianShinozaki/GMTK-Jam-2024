
using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class AndroidPiece : Resource {
	[Export]
	public SpriteFrames SpriteTexture;

	[Export(PropertyHint.Range, "0,1")]
	public float WeightSpeedReductionAmount = 1f;

	[Export(PropertyHint.Range, "0,1")]
	public float Strength = 0.5f;

	public virtual Array<Array> GetOptions => new Array<Array>();

    public virtual void InitPiece(AiBotBase bot) { }
}
