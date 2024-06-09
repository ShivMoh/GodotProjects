using System.Collections.Generic;
using System.Linq;
using Godot;


public partial class ExploreState : State
{
	PlayableCharacter selectedCharacter;

	[Export]
	TileMap tilemap;

	Vector2I currentTileCoords;
	Vector2I previousTileCoords;

	List<Vector2I> path = new List<Vector2I>();

	bool  move = false;
	int characterMoveIndex = 0;

	CharacterMeta[] characters = {
		new CharacterMeta(
			tileCoord: new Vector2I(5, 5),
			characterPath : "res://mobs/scenes/playable_character.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(0, 7),
			characterPath : "res://mobs/scenes/playable_character.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(5, 0),
			characterPath : "res://mobs/scenes/playable_character.tscn"
		),
	};

	CharacterMeta[] enemyCharacters = {
		new CharacterMeta(
			tileCoord: new Vector2I(7, 2),
			characterPath : "res://mobs/scenes/enemy_character.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(6, 3),
			characterPath : "res://mobs/scenes/enemy_character.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(3, 7),
			characterPath : "res://mobs/scenes/enemy_character.tscn"
		),
		new CharacterMeta(
			tileCoord: new Vector2I(3, 9),
			characterPath : "res://mobs/scenes/enemy_character.tscn"
		),
	};

	List<PlayableCharacter> loadedCharacters = new List<PlayableCharacter>();
	List<EnemyCharacter> loadedEnemyCharacters = new List<EnemyCharacter>();

	private Vector2I lastTile;

	private CombatUtility combatUtility;
	private CharacterUtility characterUtility;
	
	private TileUtility tileUtility;
	
	public override void enter()
	{
		currentTileCoords = new Vector2I(0, 0);
		previousTileCoords = currentTileCoords;
		selectedCharacter = null;
		loadCharacters();
		loadEnemies();

		combatUtility = new CombatUtility(tilemap, loadedEnemyCharacters, selectedCharacter);
		characterUtility = new CharacterUtility(tilemap, selectedCharacter, loadedCharacters);
		tileUtility = new TileUtility(tilemap);
		
		placeCursor();

	}

	public override void physicsUpdate(double delta)
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
			characterUtility.moveCharacter(ref path, currentTileCoords, lastTile);
		}

		if (Input.IsActionJustPressed("select")) {
			selectedCharacter = characterUtility.selectCharacter(currentTileCoords, ref path);
			combatUtility.setSelectedCharacter(selectedCharacter);
			tileUtility.drawCursor(currentTileCoords);
		}

		if (selectedCharacter != null) {
			if (selectedCharacter.move) {
				bool finished = characterUtility.moveCharacter(ref path, currentTileCoords, lastTile);

				if (finished) {
					List<EnemyCharacter> detectedEnemies = combatUtility.detectEnemy(tileUtility);
					tileUtility.drawCursor(currentTileCoords);
					
					if (detectedEnemies.Count() != 0) {
						EmitSignal(SignalName.StateChange, this, 1);
					}
				}
			}
		}
		
	}



	private void placeCursor() {
		if (selectedCharacter == null) {
			tileUtility.eraseCursor(previousTileCoords);
			tileUtility.drawCursor(currentTileCoords);
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
					tileUtility.eraseCursor(previousTileCoords);
					tileUtility.drawCursor(currentTileCoords);
					return;
				}
			}

			path.Add(removedElement);		
			
		} 

		if (path.Contains(currentTileCoords)) { 
			currentTileCoords = previousTileCoords;
			return;
		}

		tileUtility.setTiles(previousTileCoords, currentTileCoords);
		path.Add(currentTileCoords);

	}

	
	private void loadCharacters() {
		for (int i = 0; i < characters.Length; i++) {
			PlayableCharacter character = Character.instantiate(
				tilemap.MapToLocal(characters[i].tileCoord),
				characters[i].characterPath
			) as PlayableCharacter;
			tilemap.GetNode("playableCharacters").AddChild(character);
			loadedCharacters.Add(character);
		}
	}

	private void loadEnemies() {
		for (int i = 0; i < enemyCharacters.Length; i++) {
			EnemyCharacter character = Character.instantiate(
				tilemap.MapToLocal(enemyCharacters[i].tileCoord),
				enemyCharacters[i].characterPath
			) as EnemyCharacter;

			tilemap.GetNode("enemyCharacters").AddChild(character);
			loadedEnemyCharacters.Add(character);
		}
	}

}
