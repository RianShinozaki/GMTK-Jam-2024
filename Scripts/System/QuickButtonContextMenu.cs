using Godot;
using Godot.Collections;

public partial class QuickButtonContextMenu : Node {
    [Export]
    public Node ContextNode;

    [Export]
    public Node2D ButtonPivot;

    public bool Showing;

    Array dataCache = new Array();

    [Signal]
    public delegate void OnShowEventHandler();

    [Signal]
    public delegate void OnHideEventHandler();


    public void AddOption(Texture icon, string optionName, Callable function) {
        dataCache.Add(new Array(){
            icon,
            optionName,
            function
        });
    }

    public void ClearOptions() => dataCache.Clear();

    public void ShowOptions() {
        if (dataCache == null || Showing) {
            return;
        }

        for (int i = 0; i < dataCache.Count; i++) {
            Sprite2D buttonSprite = new Sprite2D {
                Texture = dataCache[i].As<Array>()[0].As<Texture2D>()
            };
            float pos = i * buttonSprite.Texture.GetWidth()+1f;
            ClickDetector detector = new ClickDetector();
            detector.Connect(ClickDetector.SignalName.OnClick, dataCache[i].As<Array>()[2].As<Callable>());
            detector.OnClick += OnChildClicked;
            detector.ContextNode = ContextNode;
            CollisionShape2D clickShape = new CollisionShape2D() {
                Shape = new RectangleShape2D() {
                    Size = new Vector2(buttonSprite.Texture.GetWidth(), buttonSprite.Texture.GetHeight())
                }
            };

            ButtonPivot.AddChild(buttonSprite);
            buttonSprite.AddChild(detector);
            detector.AddChild(clickShape);
            buttonSprite.Position += Vector2.Right * pos;
            buttonSprite.Position += Vector2.Left * buttonSprite.Texture.GetWidth() * dataCache.Count / 2f;

            if (i > 0) {
                buttonSprite.Position += Vector2.Left;
            }
        }

        EmitSignal(SignalName.OnShow);
        Showing = true;
    }

    public void HideOptions() {
        for (int i = 0; i < ButtonPivot.GetChildCount(); i++) {
            ButtonPivot.GetChild(i).QueueFree();
        }

        EmitSignal(SignalName.OnHide);
        Showing = false;
    }

    void OnChildClicked(Node context) {
        HideOptions();
    }
}
