using Godot;
using System;

public partial class testShader : TileMap
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		(Material as ShaderMaterial).SetShaderParameter("darkness_factor", 0.1f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
