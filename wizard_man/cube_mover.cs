using Godot;
using System;

public partial class cube_mover : Node3D
{
	
	MeshInstance3D cube;

	Vector3 cubeCoords = new Vector3(1.0f, 1.0f, 1.0f);
	public override void _Ready()
	{
		
		cube = GetNode<MeshInstance3D>("cube");
		cubeCoords = cube.Position;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SpawnCube() {
		var default_cube = GD.Load<PackedScene>("res://default_cube.tscn");

		default_cube csdfd = default_cube.Instantiate<default_cube>();
		csdfd.Position = new Vector3(0.0f, 0.0f, 0.0f);
		AddChild(csdfd);
		cubeCoords.X += 0.001f;
		cube.Position = cubeCoords;
		
	}
}
