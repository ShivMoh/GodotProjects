using Godot;
using System;

public partial class Node2D : Godot.Node2D
{
	[Export]
	PopupMenu popupMenu;

	Vector2 lastMousePosition;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			// this.lastMousePosition = GetGlobalMousePosition();
			// Rect2I rect2 = new Rect2I(611, 231, popupMenu.Size.X, popupMenu.Size.Y);
			// popupMenu.Popup();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton button && button.IsPressed() && button.ButtonIndex == MouseButton.Right) {
		
			this.lastMousePosition = GetGlobalMousePosition();
			Rect2I rect2 = new Rect2I((int) lastMousePosition.X, (int) lastMousePosition.Y, popupMenu.Size.X, popupMenu.Size.Y);
			popupMenu.Popup(rect2);
			GD.Print(lastMousePosition);

		
		}
	}
}
