using Godot;
using System;

public partial class sprite_2d : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		(Material as ShaderMaterial).SetShaderParameter("blue", 1.0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	
	}
}
