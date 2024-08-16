using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class HurtBox : Area2D
{
	const Variant.Type AddableVariants = Variant.Type.Float | Variant.Type.Int;

	[Signal]
	public delegate void OnDamageRecievedEventHandler(Dictionary statusEffects);

	Dictionary statCache;

	public override void _Ready() {
		AreaEntered += Entered;
		statCache = new Dictionary();
		OnDamageRecieved += OnDamage;
	}

    public override void _Process(double delta) {
        base._Process(delta);

		if (statCache.Count <= 0) {
			return;
		}

		//Calling deferred because collisions may happen both before and after this
		EmitSignal(SignalName.OnDamageRecieved, statCache.Duplicate());
    }

    void Entered(Area2D other) {
		if (other is not HitBox) {
			return;
		}

		HitBox hitBox = (HitBox)other;

		foreach(KeyValuePair<Variant, Variant> key in hitBox.StatusEffects) {
			if (key.Value.VariantType == Variant.Type.Nil) {
				continue;
			}

			if (!statCache.ContainsKey(key.Key)) {
				statCache.Add(key.Key, key.Value);
				continue;
			}

			//Converts all numbers to floats but whatever
			if (key.Key.VariantType == AddableVariants) {
				statCache[key.Key] = statCache[key.Key].As<float>() + key.Value.As<float>();
				continue;
			}

			statCache[key.Key] = key.Value;
		}

		//Makes the source default to the one with the most knockback
		if (hitBox.StatusEffects.ContainsKey("Knockback")) {
			float knockBack = hitBox.StatusEffects["Knockback"].As<float>();
			float cache = statCache["Knockback"].As<float>();

			if (knockBack > cache) {
				statCache["Source"] = other.GetParent();
			}

			cache += knockBack * 0.5f * 0.98f;
			statCache["Knockback"] = cache;

			return;
		}

		//Hard set the damage source to the last damage source, oh well
		statCache["Source"] = other.GetParent();
	}

	protected void OnDamage(Dictionary stats) {
		statCache.Clear();
	}
}
