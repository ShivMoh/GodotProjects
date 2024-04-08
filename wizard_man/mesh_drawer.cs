using Godot;
using System;

public partial class mesh_drawer : Node3D
{
	
	PackedScene tile;

	[Export]
	public MeshInstance3D mesh;

	[Export]
	mage mage;

	Ja[,] tile_map = new Ja[10, 10];
	
	public override void _Ready()
	{
		tile = GD.Load<PackedScene>("res://tile.tscn");
		drawPlatform(10, new Vector3(-(5.0f * 10.0f), 0.0f, -(5.0f * 10.0f)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Ja createMeshUnit(Vector3 position) {

		Ja tile_instance = tile.Instantiate<Ja>();
		tile_instance.mage = mage;
		tile_instance.Position = position;
		AddChild(tile_instance);
		return tile_instance;
	}

	public void drawPlatform(int n, Vector3 startPosition) {
		Vector3 start = startPosition;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				Ja tile_instance = createMeshUnit(start);
				tile_map[i, j] = tile_instance;
				// mesh.GetAabb().Size.X
				start += new Vector3(10.0f, 0.0f, 0.0f);
			}
			start = new Vector3(startPosition.X, startPosition.Y, start.Z + 10.0f);
		}
	}
}
