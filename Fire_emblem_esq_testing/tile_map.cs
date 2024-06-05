using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class tile_map : TileMap
{


	[Export]
	Character selectedCharacter;

	[Export]
	Node2D container;

	Vector2I currentTileCoords;
	Vector2I previousTileCoords;

	Queue<Vector2I> path = new Queue<Vector2I>();

	bool move = false;

	CharacterMeta[] characters = {
		new CharacterMeta(
			tileCoord: new Vector2I(0, 0),
			characterPath : "res://player_test.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(0, 1),
			characterPath : "res://player_test.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(1, 0),
			characterPath : "res://player_test.tscn"
		),
	};

	private Character[] loadedCharacters;

	private Vector2I lastTile;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// var tiles = this.GetUsedCells(0);
		// GD.Print(this.GetCellTileData(0, tiles[0]));
		currentTileCoords = new Vector2I(0, 0);
		selectedCharacter = null;
		loadCharacters();
		placeCursor();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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
			moveCharacter();
			selectedCharacter.move = true;
			lastTile = path.Last();
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
	 
		if (reachDestination && path.Count != 0) {
			Vector2I next_tile = path.Dequeue();
			selectedCharacter.targetPosition = this.MapToLocal(next_tile);			
		} 

		Vector2 lastCoordinates = this.MapToLocal(lastTile);

		bool reachLast = 	Math.Abs(selectedCharacter.GlobalPosition.X - lastCoordinates.X) < 2.0f
									&& Math.Abs(selectedCharacter.GlobalPosition.Y - lastCoordinates.Y) < 2.0f;
	 
		if (reachLast) {
			selectedCharacter.move = false;
		}
	}

	private void selectCharacter() {
		Character character = loadedCharacters.FirstOrDefault<Character>(character => character.GlobalPosition == MapToLocal(currentTileCoords), null);
		selectedCharacter = character;
	}
	
	private void printQueue() {
		foreach (var element in path.ToList())
		{
			GD.Print(element);
		}
	}



	private void placeCursor() {

		if (selectedCharacter == null) {
			this.EraseCell(
				2,
				previousTileCoords
			);

			this.SetCell(
				2,
				currentTileCoords,
				7,
				new Vector2I(1, 0)
			);
		} else {
			this.SetCell(
				2,
				currentTileCoords,
				7,
				new Vector2I(1, 0)
			);

			path.Enqueue(currentTileCoords);
		}

	}

	private Character instantiateCharacter(Vector2I tileCoords, string path) {
		PackedScene characterScene = GD.Load<PackedScene>(path);
		var instance = characterScene.Instantiate<Character>();
		instance.GlobalPosition = this.MapToLocal(tileCoords);	

		container.AddChild(instance);
		loadedCharacters.Append<Character>(instance);

		return instance;
	}
	
	public void loadCharacters() {
		for (int i = 0; i < characters.Length; i++) {
			instantiateCharacter(
				characters[i].tileCoord,
				characters[i].characterPath
			);
		}
	}

}
