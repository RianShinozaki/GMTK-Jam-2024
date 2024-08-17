using Godot;
using Godot.Collections;

public partial class ClickableContextMenu : ClickDetector {
    [Export]
    public QuickButtonContextMenu ContextMenu;

    public override void _Ready() {
        base._Ready();

        OnClick += OnClickDetected;
        ContextMenu.OnShow += OnShow;
        ContextMenu.OnHide += OnHide;
    }

    void OnClickDetected(Node context) {
        if (ContextMenu.Showing) {
            return;
        }

        ContextMenu.ShowOptions();
    }

    void OnHide() => InputPickable = true;
    void OnShow() => InputPickable = false;
}
