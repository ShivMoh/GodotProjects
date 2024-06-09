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

	CharacterMeta[] enemyCharacters = {
		new CharacterMeta(
			tileCoord: new Vector2I(7, 2),
			characterPath : "res://enemy.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(6, 3),
			characterPath : "res://enemy.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(3, 7),
			characterPath : "res://enemy.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(3, 9),
			characterPath : "res://enemy.tscn"
		),
	};

	List<PlayableCharacter> loadedCharacters = new List<PlayableCharacter>();
	List<EnemyCharacter> loadedEnemyCharacters = new List<EnemyCharacter>();

	private Vector2I lastTile;

	private CombatUtil combatUtil;
	private PlayableCharacterUtil playableUtil;
	
	public override void _Ready()
	{
		currentTileCoords = new Vector2I(0, 0);
		selectedCharacter = null;
		playableUtil = new PlayableCharacterUtil();
		loadCharacters();
		loadEnemies();
		placeCursor();

		combatUtil = new CombatUtil(this, loadedEnemyCharacters, selectedCharacter);
		
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
			selectedCharacter = playableUtil.selectCharacter(this, loadedCharacters, selectedCharacter, currentTileCoords, ref path);
			combatUtil.setSelectedCharacter(selectedCharacter);
		}

		if (selectedCharacter != null) {
			if (selectedCharacter.move) {
				bool finished = playableUtil.moveCharacter(this, selectedCharacter, ref path, currentTileCoords, ref characterMoveIndex, lastTile);

				if (finished) {
					List<EnemyCharacter> detectedEnemies = combatUtil.detectEnemy();

					foreach (EnemyCharacter enemy in detectedEnemies)
					{
						GD.Print(enemy);
					}
				}
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

	private void loadEnemies() {
		for (int i = 0; i < enemyCharacters.Length; i++) {
			GD.Print("Enemeies are loading");
			EnemyCharacter character = Character.instantiate(
				this.MapToLocal(enemyCharacters[i].tileCoord),
				enemyCharacters[i].characterPath
			) as EnemyCharacter;
			container.AddChild(character);
			loadedEnemyCharacters.Add(character);
		}
	}

}
