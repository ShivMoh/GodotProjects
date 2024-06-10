using System.Collections.Generic;
using System.Linq;
using Godot;


public partial class ExploreState : State
{

	[Export]
	TileMap tilemap;

	Vector2I currentTileCoords;
	Vector2I previousTileCoords;

	List<Vector2I> path = new List<Vector2I>();

	bool  move = false;
	int characterMoveIndex = 0;

	CharacterMeta[] playableCharactersMeta = {
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

	CharacterMeta[] enemyCharactersMeta = {
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

	private Vector2I lastTile;

	private CombatUtility combatUtility;
	private CharacterUtility characterUtility;
	
	private TileUtility tileUtility;
	
	public override void enter()
	{
		MapEntities.map = tilemap;

		currentTileCoords = new Vector2I(0, 0);
		previousTileCoords = currentTileCoords;
		MapEntities.selectedCharacter = null;
		loadCharacters();
		loadEnemies();

		combatUtility = new CombatUtility(MapEntities.map, MapEntities.enemyCharacters, MapEntities.selectedCharacter);
		characterUtility = new CharacterUtility(MapEntities.map, MapEntities.selectedCharacter, MapEntities.playableCharacters);
		tileUtility = new TileUtility(MapEntities.map);

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
			MapEntities.selectedCharacter.move = true;
			lastTile = path.Last();
			characterUtility.moveCharacter(ref path, currentTileCoords, lastTile);
		}

		if (Input.IsActionJustPressed("select")) {
			MapEntities.selectedCharacter = characterUtility.selectCharacter(currentTileCoords, ref path);
			combatUtility.setSelectedCharacter(MapEntities.selectedCharacter);
			tileUtility.drawCursor(currentTileCoords);
		}

		if (MapEntities.selectedCharacter != null) {
			if (MapEntities.selectedCharacter.move) {
				bool finished = characterUtility.moveCharacter(ref path, currentTileCoords, lastTile);

				if (finished) {
					MapEntities.detectedEnemies = combatUtility.detectEnemy(tileUtility);
					tileUtility.drawCursor(currentTileCoords);
					
					// EmitSignal(SignalName.ShareCharacter, MapEntities.selectedCharacter);
					EmitSignal(SignalName.StateChange, this, 2);
					// if (detectedEnemies.Count() != 0) {
					// 	EmitSignal(SignalName.StateChange, this, 1);
					// }
				}
			}
		}
		
	}



	private void placeCursor() {
		if (MapEntities.selectedCharacter == null) {
			tileUtility.eraseCursor(previousTileCoords);
			tileUtility.drawCursor(currentTileCoords);
		} else {
					
			if (MapEntities.selectedCharacter.isMoving()) {
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
		for (int i = 0; i < playableCharactersMeta.Length; i++) {
			PlayableCharacter character = Character.instantiate(
				MapEntities.map.MapToLocal(playableCharactersMeta[i].tileCoord),
				playableCharactersMeta[i].characterPath
			) as PlayableCharacter;
			MapEntities.map.GetNode("playableCharacters").AddChild(character);
			MapEntities.playableCharacters.Add(character);
		}
	}

	private void loadEnemies() {
		for (int i = 0; i < enemyCharactersMeta.Length; i++) {
			EnemyCharacter character = Character.instantiate(
				MapEntities.map.MapToLocal(enemyCharactersMeta[i].tileCoord),
				enemyCharactersMeta[i].characterPath
			) as EnemyCharacter;

			MapEntities.map.GetNode("enemyCharacters").AddChild(character);
			MapEntities.enemyCharacters.Add(character);
		}
	}

}
