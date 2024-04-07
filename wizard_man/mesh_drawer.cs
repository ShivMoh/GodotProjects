using Godot;
using System;

public partial class mesh_drawer : Node3D
{
	
	PackedScene mesh_unit;
	public override void _Ready()
	{
		mesh_unit = GD.Load<PackedScene>("res://mesh_unit.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_text_delete")) {
			createMeshUnit();
		}
	}

	public void createMeshUnit() {

		mesh_unit mesh_unit_instanace = mesh_unit.Instantiate<mesh_unit>();
		mesh_unit_instanace.Position = new Vector3(0.0f, 0.0f, 0.0f);
		AddChild(mesh_unit_instanace);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventMouseButton mouseButton ) {
			GD.Print("Position", mouseButton.Position);
		} 

		// GD.Print("Get ViewPort", GetViewport().GetVisibleRect().Size);
	}
}
