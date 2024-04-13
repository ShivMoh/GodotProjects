using Godot;
using System;

public partial class mesh_drawer : Node3D
{
	
	PackedScene tile;
	
	[Export]
	mage mage;

	Ja[,] tile_map = new Ja[10, 10];
	meta [] characters = { 
		new meta(
			m: 2,
			n: 2,
			character_path: "res://mage.tscn"
		),
		new meta(
			m: 2,
			n: 8,
			character_path: "res://mage.tscn"
		),

	};

	int max_moves = 200;

	int moves = 0;
	int cursorX = 0;
	int cursorY = 0;

	Vector2 [] graph_positions = new Vector2[100];

	bool init = true;
	int currentNode = -1;

	
	bool moving = false;

	int iteration = 0;
	
	int n = 10;

	bool characterSelected = false;
	Vector3  destinationTarget = Vector3.Zero;
	public override void _Ready()
	{
		tile = GD.Load<PackedScene>("res://tile.tscn");
		
		drawPlatform(n, new Vector3(-(5.0f * 10.0f), 0.0f, -(5.0f * 10.0f)));
		loadCharacters();
		placeCursor(0, 0);
	}

	public override void _PhysicsProcess(double delta)
	{

		if (init ) {
			destinationTarget = tile_map[(int) graph_positions[currentNode].X, (int) graph_positions[currentNode].Y].GlobalPosition;
			mage.GlobalPosition = destinationTarget;
			init = false;
		}

		if (!moving) {
			if (Input.IsActionJustPressed("ui_focus_next")) {
				selectCharacter(cursorX, cursorY);
			}

			if (Input.IsActionJustPressed("ui_text_delete")) {
				for(int x = 0; x <= currentNode; x++) {
					tile_map[(int) graph_positions[x].X, (int) graph_positions[x].Y].changeColor(new Color(1.0f, 0.0f, 1.0f));
				}
			}
			if (Input.IsActionJustPressed("ui_right")) {
				cursorY = Mathf.Min(cursorY + 1, n - 1);
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
				cursorX = Mathf.Min(cursorX + 1, n - 1);
				moves+=1;
				placeCursor(cursorX, cursorY);
			} 

			if (Input.IsActionJustPressed("ui_accept")) {
				moving = true;
				tile_map[cursorX, cursorY].has_mage = false;
				tile_map[cursorX, cursorY].mage = null;
			}
		}
		
		// for printing the active state of the grid
		if (Input.IsActionJustPressed("ui_cancel")) {
			for (int i = 0; i < 10; i++) {
				var str = "[";
				for (int j = 0; j < 10; j++) {
					
					str += (tile_map[i,j].active ? 1 : 0).ToString() + " | ";
				}
				str += "]";
				GD.Print(str);
			}
		}

		if (moving) {
			if 
			(
				Mathf.Round(mage.Position.X) == Mathf.Round(destinationTarget.X) &&
				Mathf.Round(mage.Position.Z) == Mathf.Round(destinationTarget.Z)	
			) {
				moveCharacter(iteration);
				iteration += 1;
			}
		}
		
	}
	
	public void selectCharacter(int m, int n) {
		if (tile_map[m, n].has_mage) {
			characterSelected = true;
			mage = tile_map[m, n].mage;
			destinationTarget = tile_map[m, n].GlobalPosition;
		} else if (characterSelected) {
			characterSelected = false;
			clearVectors();
			graph_positions = new Vector2[100];
			currentNode = -1;
			iteration = 0;
			tile_map[m, n].has_mage = true;
			tile_map[m, n].mage = mage;
			tile_map[m, n].changeColor(new Color(0.0f, 0.0f, 1.0f));
			placeCursor(m, n);

		}
	}

	public void clearVectors() {
		for (int i = 0; i <= currentNode; i++) {
			tile_map[(int) graph_positions[i].X, (int) graph_positions[i].Y].changeColor(new Color(0.0f, 0.0f, 1.0f));
			tile_map[(int) graph_positions[i].X, (int) graph_positions[i].Y].active = false;
		}
	}

	public Ja createMeshUnit(Vector3 position) {
		Ja tile_instance = tile.Instantiate<Ja>();
		AddChild(tile_instance);
		return tile_instance;
	}

	public void moveCharacter(int iter) {

		destinationTarget = tile_map[(int) graph_positions[iter].X, (int) graph_positions[iter].Y].GlobalPosition; 
		mage.toPosition = destinationTarget;
		mage.move = true;
		
		if (iter == currentNode) {
			moving = false;
		}
	}

	public void placeCursor(int m, int n) {

		if (!characterSelected) {
			
			if (currentNode != -1) {
				tile_map[(int) graph_positions[0].X, (int) graph_positions[0].Y].changeColor(new Color(0.0f, 0.0f, 1.0f));
			}
			tile_map[m, n].changeColor(new Color(0.0f, 1.0f, 0.0f));
			currentNode = 0;
			graph_positions[currentNode] = new Vector2(m, n);
			return;
		}

		if (moves >= (max_moves + 1)) {
			GD.Print("Maximum moves exceeded");
			return;
		}

		// the way to determine if its a revert, is if the node
		// your moving to is active
		
		// rule: you can't go on any node that is active unless that node is the previous node
		// rule: to revert, is to go back to the previous node, and deactivate the node at the prior coordinates

		if (new Vector2(m, n) == graph_positions[Mathf.Max(currentNode - 1, 0)] && currentNode != -1) {
			// move back
			tile_map[(int) graph_positions[currentNode].X, (int) graph_positions[currentNode].Y].changeColor(new Color(0.0f, 0.0f, 1.0f));
			Vector2 current = graph_positions[currentNode];
			// grid[(int) current.X, (int) current.Y] = 0;
			tile_map[(int) current.X, (int) current.Y].active = false;
			graph_positions[currentNode] = Vector2.Zero;
			if (currentNode > 0) {
				currentNode--;
			}

		} else if (tile_map[m, n].active) {
			// to not move if square has already been moved on
			cursorX = (int) graph_positions[currentNode].X;
			cursorY = (int) graph_positions[currentNode].Y;
			return;
		}

		if (currentNode >= 0 && !tile_map[m, n].active) {
			// to have the color of the current node be different
			tile_map[m, n].changeColor(new Color(0.0f, 1.0f, 0.0f));
			tile_map[m, n].active = true;
			currentNode++;		
			graph_positions[currentNode] = new Vector2(m, n);

			// changes the previous one back to platform
			tile_map[(int) graph_positions[currentNode - 1].X, (int) graph_positions[currentNode - 1].Y].changeColor(new Color(1.0f, 0.0f, 0.0f));
		}

	}

	public mage instantiateCharacter(string path, int m, int n) {
		PackedScene character_scene = GD.Load<PackedScene>(path);
		mage instance = character_scene.Instantiate<mage>();
		instance.Position = tile_map[m, n].GlobalPosition;		
		tile_map[m, n].mage = instance;
		tile_map[m, n].has_mage = true;
		AddChild(instance);

		return instance;
	}
	public void loadCharacters() {
		for (int i = 0; i < characters.Length; i++) {
			instantiateCharacter(
				characters[i].character_path,
				characters[i].m,
				characters[i].n
			);
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
	
				start += new Vector3(10.0f, 0.0f, 0.0f);
			}
			start = new Vector3(startPosition.X, startPosition.Y, start.Z + 10.0f);
		}
	}

}
