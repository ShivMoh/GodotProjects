using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;

public partial class PlayableCharacterMoverUtil : TileMap
{
	PlayableCharacter selectedCharacter;

	[Export]
	Node2D container;

	Vector2I currentTileCoords;
	Vector2I previousTileCoords;

	List<Vector2I> path = new List<Vector2I>();

	bool  move = false;
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

	List<PlayableCharacter> loadedCharacters = new List<PlayableCharacter>();

	private Vector2I lastTile;

	private CombatUtil combatUtil;
	private PlayableCharacterUtil playableUtil;
	
	public override void _Ready()
	{
		currentTileCoords = new Vector2I(0, 0);
		selectedCharacter = null;
		combatUtil = new CombatUtil();
		playableUtil = new PlayableCharacterUtil();
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
			playableUtil.moveCharacter(this, selectedCharacter, ref path, currentTileCoords, ref characterMoveIndex, lastTile);
		}

		if (Input.IsActionJustPressed("select")) {
			selectCharacter();
		}

		if (selectedCharacter != null) {
			GD.Print("helllooo");
			if (selectedCharacter.move) {
				bool finished = playableUtil.moveCharacter(this, selectedCharacter, ref path, currentTileCoords, ref characterMoveIndex, lastTile);
				if (finished) {
					playableUtil.clearPath(this, ref path, currentTileCoords);
					path.Clear();	
					GD.Print("FINISHED");
				} 
			}
		}
	}

	private void selectCharacter() {
		var character = loadedCharacters.FirstOrDefault<PlayableCharacter>(character => playableUtil.compareValues(character.GlobalPosition, MapToLocal(currentTileCoords), 2.0f), null);
		if (character is not null) {
			if (selectedCharacter == null) {
				selectedCharacter = character;
			} else {
				selectedCharacter = null;
				playableUtil.clearPath(this, ref path, currentTileCoords);
			}
		} 
	}

	private void placeCursor() {
		if (selectedCharacter == null) {
			TileUtil.eraseCursor(this, previousTileCoords);
			TileUtil.drawCursor(this, currentTileCoords);
		} else {
					
			if (selectedCharacter.isMoving()) {
				currentTileCoords = previousTileCoords;
				return;
			}

			createPath();
		}

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
					TileUtil.eraseCursor(this, previousTileCoords);
					TileUtil.drawCursor(this, currentTileCoords);
					return;
				}
			}

			path.Add(removedElement);		
			
		} 

		if (path.Contains(currentTileCoords)) { 
			currentTileCoords = previousTileCoords;
			return;
		}

		TileUtil.setTiles(this, previousTileCoords, currentTileCoords);
		path.Add(currentTileCoords);

	}

	
	private void loadCharacters() {
		for (int i = 0; i < characters.Length; i++) {
			PlayableCharacter character = Character.instantiate(
				this.MapToLocal(characters[i].tileCoord),
				characters[i].characterPath
			) as PlayableCharacter;
			container.AddChild(character);
			loadedCharacters.Add(character);
		}
	}

}
