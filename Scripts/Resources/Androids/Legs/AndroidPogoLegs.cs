using Godot;
using Godot.Collections;

[GlobalClass]
public partial class AndroidPogoLegs : AndroidPiece {
    [Export]
    public Texture2D TurnIcon, StopIcon;
    
    float inputCache = -1f;

    public override Array<Array> GetOptions => new Array<Array> {
        new Array {
            TurnIcon,
            Callable.From<Node>(Turn)
        },
        new Array {
            StopIcon,
            Callable.From<Node>(Stop)
        }
    };

    void Turn(Node context) {
        if (context is AiBotBase character && character.IsOnFloor()) {
            character.InputDirection *= -1f;
        }
    }
    
    void Stop(Node context) {
        if (context is AiBotBase character && character.IsOnFloor()) {
            if (character.InputSpeed != 0f) {
                inputCache = character.InputSpeed;
                character.InputSpeed = 0f;
                return;
            }

            character.InputSpeed = inputCache;
        } 
    }
}
