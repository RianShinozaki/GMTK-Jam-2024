using System.Reflection.Metadata;
using Godot;
using Godot.Collections;

public partial class ClickableContextMenu : ClickDetector {
    [Export]
    public Array<QuickButtonContextMenu> ContextMenus;

    public override void _Ready() {
        base._Ready();

        OnClick += OnClickDetected;
        
        for(int i = 0; i < ContextMenus.Count; i++) {
            ContextMenus[i].OnShow += OnShow;
            ContextMenus[i].OnHide += OnHide;   
        }
    }

    void OnClickDetected(Node context) {
        if (ContextMenus[0].Showing) {
            return;
        }

        for(int i = 0; i < ContextMenus.Count; i++) {
            ContextMenus[i].ShowOptions();  
        }
    }

    //Right click to cancel menu
    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseEv) {
            if (ContextMenus[0].Showing && mouseEv.ButtonIndex == MouseButton.Right) {
                for(int i = 0; i < ContextMenus.Count; i++) {
                    ContextMenus[i].HideOptions();  
                }

            }
        }
        base._Input(@event);
    }

    void OnHide() => InputPickable = true;
    void OnShow() => InputPickable = false;
}
