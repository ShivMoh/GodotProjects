using Godot;
using System;

public partial class mesh_instance_3d : MeshInstance3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// (Mesh.SurfaceGetMaterial(0).NextPass as ShaderMaterial).SetShaderParameter("height_scale", 0.0);

		(Mesh.SurfaceGetMaterial(0) as ShaderMaterial).SetShaderParameter("height_scale", 0.5);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
