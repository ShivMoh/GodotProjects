using Godot;
using System;

public partial class Ja : StaticBody3D
{

	
	[Export]
	public bool active = false;
	
	public Vector2 coordinates = Vector2.Zero;

	public override void _Ready()
	{	

	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}


	// public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	// {
	// 	base._InputEvent(camera, @event, position, normal, shapeIdx);
	// 	GD.Print("Did this run");
	// 	if(@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) {
	// 		mage.setToPosition(GlobalPosition);
	// 		mage.move = true;
		
	// 	}	
	// }

	public void changeColor(Color color) {
		MeshInstance3D mes = GetNode<MeshInstance3D>("mesh");
		StandardMaterial3D material = mes.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		material.AlbedoColor = color;
	}
	
}


