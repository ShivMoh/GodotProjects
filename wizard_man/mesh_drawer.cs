using Godot;
using System;

public partial class mesh_drawer : Node3D
{
	
	PackedScene tile;

	[Export]
	mage mage;

	Ja[,] tile_map = new Ja[10, 10];
	int [,] grid = new int[10, 10];	

	int cursorX = 0;
	int cursorY = 0;
	public override void _Ready()
	{
		GD.Print("WHHHHHAA");
		tile = GD.Load<PackedScene>("res://tile.tscn");
		initGrid(10);
		drawPlatform(10, new Vector3(-(5.0f * 10.0f), 0.0f, -(5.0f * 10.0f)));
		placeCursor(0, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		
	}
	

	public Ja createMeshUnit(Vector3 position) {

		Ja tile_instance = tile.Instantiate<Ja>();
		tile_instance.mage = mage;
		AddChild(tile_instance);
		return tile_instance;
	}

	public void placeCursor(int m, int n) {
		grid[m, n] = 1;
		cursorX = m;
		cursorY = n;
		tile_map[m, n].active = true;
	}

	public void initGrid(int n) {
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				grid[i, j] = 0;
			}
		}
	}

	public void drawPlatform(int n, Vector3 startPosition) {
				GD.Print("WHHHHHAA");

		Vector3 start = startPosition;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				GD.Print("hellllOoo??");
				Ja tile_instance = createMeshUnit(start);
				tile_instance.coordinates = new Vector2(i, j);
				tile_map[i, j] = tile_instance;
				tile_map[i, j].Position = start;
				// tile_map[i, j].NodeActivated += ActivateNode;
				
				// mesh.GetAabb().Size.X
				start += new Vector3(10.0f, 0.0f, 0.0f);
			}
			start = new Vector3(startPosition.X, startPosition.Y, start.Z + 10.0f);
		}
	}


	// public void ActivateNode(Vector2 coords) {
	// 	tile_map[(int) coords.X, (int) coords.Y].changeColor(new Color(0, 0, 1));
	// }

	

	public void activate(Ja mesh) {
		MeshInstance3D mes = mesh.GetNode<MeshInstance3D>("mesh");
		StandardMaterial3D material = mes.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		material.AlbedoColor = new Color(0, 0, 1);

		// StandardMaterial3D material = new StandardMaterial3D();
		// material.AlbedoColor = new Color(0, 0, 1);
		// mesh.GetNode<MeshInstance3D>("mesh").MaterialOverride = material;
	}
}
