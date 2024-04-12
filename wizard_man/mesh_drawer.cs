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

	Vector2 [] graph_positions = new Vector2[100];

	int currentNode = -1;
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

		if (Input.IsActionJustPressed("ui_text_delete")) {
			for(int x = 0; x < currentNode; x++) {
				tile_map[(int) graph_positions[x].X, (int) graph_positions[x].Y].changeColor(new Color(0.0f, 1.0f, 0.0f));
			}
		}
		if (Input.IsActionJustPressed("ui_right")) {
			cursorY = Mathf.Min(cursorY + 1, 9);
			moves+=1;
			placeCursor(cursorX, cursorY);
		} 

		if (Input.IsActionJustPressed("ui_left")) {
			
			cursorY = Mathf.Max(cursorY - 1, 0);
			moves+=1;
			placeCursor(cursorX, cursorY);
		} 

		if (Input.IsActionJustPressed("ui_up")) {
			
			cursorX = Mathf.Max(cursorX - 1, 0);
			moves+=1;
			placeCursor(cursorX, cursorY);
		} 
		
		if (Input.IsActionJustPressed("ui_down")) {
			
			cursorX = Mathf.Min(cursorX + 1, 9);
			moves+=1;
			placeCursor(cursorX, cursorY);
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

		// GD.Print(graph_positions[Mathf.Max(currentNode - 1, 0)]);

		// GD.Print("PREVIOUS NODE BEFORE: ");
		// GD.Print("Current Node Index", currentNode);	
		// GD.Print("Cursor", new Vector2(m, n));
		// GD.Print("Previous Node", graph_positions[Mathf.Max(currentNode - 1, 0)]);

		if (currentNode != -1) {

			GD.Print(new Vector2(m, n));
			GD.Print("CurrentNode", currentNode);
			GD.Print(graph_positions[currentNode]);
		}
		 
		if (new Vector2(m, n) == graph_positions[Mathf.Max(currentNode - 1, 0)] && currentNode != -1) {
			
			
			tile_map[(int) graph_positions[currentNode].X, (int) graph_positions[currentNode].Y].changeColor(new Color(0.0f, 0.0f, 1.0f));

			graph_positions[currentNode] = Vector2.Zero;

			if (currentNode > 0) {
				currentNode--;
				GD.Print("helloooooooo?");
			}

			// GD.Print("PREVIOUS NODE AFTER: ");
			// GD.Print("Current Node Index", currentNode);	
			// GD.Print("Cursor", new Vector2(m, n));
			// GD.Print("Previous Node", graph_positions[Mathf.Max(currentNode - 1, 0)]);
			grid[m, n] = 0;
			tile_map[m, n].active = false;
		} else if (!tile_map[m, n].active){
			grid[m, n] = 1;
			tile_map[m, n].active = true;
			tile_map[m, n].changeColor(new Color(1.0f, 0.0f, 0.0f));
			currentNode++;		
			graph_positions[currentNode] = new Vector2(m, n);
			GD.Print("This is running");
		}

		// GD.Print(new Vector2(m, n));
		// GD.Print(graph_positions[currentNode]);
		

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
