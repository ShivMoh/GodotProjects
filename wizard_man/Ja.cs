using Godot;
using System;

public partial class Ja : StaticBody3D
{

	[Export]
	mage mage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		base._InputEvent(camera, @event, position, normal, shapeIdx);
		
		if(@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) {
			GD.Print("Here", GlobalPosition);
			mage.setToPosition(GlobalPosition);
			mage.move = true;
		}
	}
}
