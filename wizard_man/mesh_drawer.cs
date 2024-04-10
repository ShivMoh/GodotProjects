using Godot;
using System;

public partial class mesh_drawer : Node3D
{
	
	PackedScene tile;

	[Export]
	mage mage;

	Ja[,] tile_map = new Ja[10, 10];
	int [,] grid = new int[10, 10];	

	int max_moves = 30;

	int moves = 0;
	int cursorX = 0;
	int cursorY = 0;

	private int previousX = 0;
	private int previousY = 0;

	private int preX = 0;
	private int preY = 0;
	public override void _Ready()
	{
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

		previousX = cursorX;
		previousY = cursorY;

		if (Input.IsActionJustPressed("ui_right")) {
			
			cursorY = Mathf.Min(cursorY + 1, 9);
			GD.Print("I am going right");
			moves+=1;
			placeCursor(cursorX, cursorY);
			preY = cursorY - 1;
		} 

		if (Input.IsActionJustPressed("ui_left")) {
			
			cursorY = Mathf.Max(cursorY - 1, 0);
			GD.Print("I am going left");
			moves+=1;
			placeCursor(cursorX, cursorY);
			preY = cursorY + 1;
		} 

		if (Input.IsActionJustPressed("ui_up")) {
			
			cursorX = Mathf.Max(cursorX - 1, 0);
			GD.Print("I am going up");
			moves+=1;
			placeCursor(cursorX, cursorY);
			preX = cursorX + 1;
		} 
		
		if (Input.IsActionJustPressed("ui_down")) {
			
			cursorX = Mathf.Min(cursorX + 1, 9);
			GD.Print("I am going down");
			moves+=1;
			placeCursor(cursorX, cursorY);
			preX = cursorX - 1;
		} 

		
	}
	

	public Ja createMeshUnit(Vector3 position) {
		Ja tile_instance = tile.Instantiate<Ja>();
		tile_instance.mage = mage;
		AddChild(tile_instance);
		return tile_instance;
	}

	public void placeCursor(int m, int n) {

		// if (moves >= (max_moves + 1)) {
		// 	GD.Print("Maximum moves exceeded");
		// 	return;
		// }

		// the way to determine if its a revert, is if the node
		// your moving to is active
		
		// rule: you can't go on any node that is active unless that node is the previous node
		// rule: to revert, is to go back to the previous node, and deactivate the node at the prior coordinates

		if(tile_map[m, n].active && m == preX && n == preY) {
			grid[m, n] = 0;
			tile_map[previousX, previousY].active = false;
		} 
		
		if (!tile_map[m, n].active){
			grid[m, n] = 1;
			tile_map[m, n].active = true;
		}
	}

	public void initGrid(int n) {
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				grid[i, j] = 0;
			}
		}
	}

	public void drawPlatform(int n, Vector3 startPosition) {

		Vector3 start = startPosition;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
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
