using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;
using System.IO;


public partial class tile_map : TileMap
{


	[Export]
	Character selectedCharacter;

	[Export]
	Node2D container;

	Vector2I currentTileCoords;
	Vector2I previousTileCoords;

	List<Vector2I> path = new List<Vector2I>();

	bool move = false;
	int characterMoveIndex = 0;

	CharacterMeta[] characters = {
		new CharacterMeta(
			tileCoord: new Vector2I(5, 5),
			characterPath : "res://player_test.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(0, 7),
			characterPath : "res://player_test.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(5, 0),
			characterPath : "res://player_test.tscn"
		),
	};

	List<Character> loadedCharacters = new List<Character>();

	private Vector2I lastTile;
	
	public override void _Ready()
	{
		// var tiles = this.GetUsedCells(0);
		// GD.Print(this.GetCellTileData(0, tiles[0]));
		currentTileCoords = new Vector2I(0, 0);
		selectedCharacter = null;
		loadCharacters();
		placeCursor();
	}

	public override void _PhysicsProcess(double delta)
	{	
		previousTileCoords = currentTileCoords;

		if (Input.IsActionJustPressed("right")) {
			currentTileCoords.X += 1;
			this.placeCursor();
		}

		if (Input.IsActionJustPressed("left")) {
			currentTileCoords.X -= 1;
			this.placeCursor();
		}

		if (Input.IsActionJustPressed("up")) {
			currentTileCoords.Y -= 1;
			this.placeCursor();
		}

		if (Input.IsActionJustPressed("down")) {
			currentTileCoords.Y += 1;
			this.placeCursor();
		}

		if (Input.IsActionJustPressed("ui_text_delete")) {
			selectedCharacter.move = true;
			lastTile = path.Last();
			moveCharacter();
		}

		if (Input.IsActionJustPressed("select")) {
			selectCharacter();
		}

		if (selectedCharacter != null) {
			if (selectedCharacter.move) {
				moveCharacter();
			}
		}

	}

	// private void updateCharacterPosition(Vector2I toCoords) {
	// 	selectedCharacter.Position = targetPosition;
	// }

	private void moveCharacter() {

		bool reachDestination = 	Math.Abs(selectedCharacter.GlobalPosition.X - selectedCharacter.targetPosition.X) < 2.0f
									&& Math.Abs(selectedCharacter.GlobalPosition.Y - selectedCharacter.targetPosition.Y) < 2.0f;
	 
		if (reachDestination && path.Count() != 0 && characterMoveIndex < path.Count()) {
			GD.Print(characterMoveIndex);
			GD.Print(path.Count());
			Vector2I next_tile = path.ElementAt(characterMoveIndex); 
			
			selectedCharacter.targetPosition = this.MapToLocal(next_tile);			
			characterMoveIndex++;
		} 

		Vector2 lastCoordinates = this.MapToLocal(lastTile);

		bool reachLast = 	Math.Abs(selectedCharacter.GlobalPosition.X - lastCoordinates.X) < 2.0f
									&& Math.Abs(selectedCharacter.GlobalPosition.Y - lastCoordinates.Y) < 2.0f;
	 
		if (reachLast) {
			selectedCharacter.move = false;
			characterMoveIndex = 0;
			this.clearPath();
		}
	}

	private void clearPath() {
		
		ClearLayer(3);
		path.Clear();

		this.SetCell(
			3,
			currentTileCoords,
			7,
			new Vector2I(0, 0)
		);

	}
	
	private void selectCharacter() {
		var character = loadedCharacters.FirstOrDefault<Character>(character => character.GlobalPosition == MapToLocal(currentTileCoords), null);

		if (character is not null) {
			selectedCharacter = character;
			
		}
	}
	
	private void printQueue() {
		int index = 0;
		foreach (var element in path.ToList())
		{
			GD.Print("Index: ", index, "Element: ", element);
			index++;
		}
	}

	private void placeCursor() {
		int layer = 3;
		
		if (selectedCharacter == null) {
			this.EraseCell(
				layer,
				previousTileCoords
			);

			this.SetCell(
				layer,
				currentTileCoords,
				7,
				new Vector2I(1, 0)
			);
		} else {
			createPath();
		}

		// this.SetCell(
		// 	layer, 
		// 	currentTileCoords, 
		// 	7,
		// 	new Vector2I(0, 0)
		// );

	}

	private void createPath() {
		if (path.Count() == 0) {
			path.Add(previousTileCoords);
		}

		if (path.Count() != 0) {
			Vector2I removedElement = path.Last();
			path.RemoveAt(path.IndexOf(path.Last()));

			if (path.Count() != 0) {
				if (path.Last() == currentTileCoords) {
					this.EraseCell(
						3,
						previousTileCoords
					);

					this.SetCell(
							3,
							currentTileCoords,
							7,
							new Vector2I(0, 0)
					);

					return;
				}
			}

			path.Add(removedElement);
		
			
		} 

		if (path.Contains(currentTileCoords)) { 
			currentTileCoords = previousTileCoords;
			return;
		}

		setTiles();
		path.Add(currentTileCoords);

		GD.Print(path.ElementAt(0));
		// printQueue();
	}

	private void setTiles() {

		this.SetCell(
				3,
				previousTileCoords,
				7,
				new Vector2I(1, 0)
		);

		this.SetCell(
				3,
				currentTileCoords,
				7,
				new Vector2I(0, 0)
		);

	}

	private Character instantiateCharacter(Vector2I tileCoords, string path) {
		PackedScene characterScene = GD.Load<PackedScene>(path);
		Character instance = characterScene.Instantiate<Character>();
		instance.GlobalPosition = this.MapToLocal(tileCoords);	

		container.AddChild(instance);
	
		loadedCharacters.Add(instance);

		// GD.Print("character loaded", instance);
		return instance;
	}
	
	public void loadCharacters() {
		for (int i = 0; i < characters.Length; i++) {
			this.instantiateCharacter(
				characters[i].tileCoord,
				characters[i].characterPath
			);
		}

		// GD.Print(loadedCharacters);
	}

}
