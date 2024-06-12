using System.Collections.Generic;
using System.Linq;
using Godot;


public partial class ExploreState : State
{

	
	Vector2I currentTileCoords;
	Vector2I previousTileCoords;

	List<Vector2I> path = new List<Vector2I>();

	bool  move = false;
	int characterMoveIndex = 0;

	private Vector2I lastTile;

	private CombatUtility combatUtility;
	private CharacterUtility characterUtility;
	
	private TileUtility tileUtility;

	private string currentTurn;
	
	private int turnIndex = 0;
	public override void enter()
	{
	
		currentTileCoords = MapEntities.cursorCoords;
		previousTileCoords = currentTileCoords;
		
		combatUtility = new CombatUtility(MapEntities.map, MapEntities.enemyCharacters, MapEntities.selectedCharacter);
		characterUtility = new CharacterUtility(MapEntities.map, MapEntities.selectedCharacter, MapEntities.playableCharacters);
		tileUtility = new TileUtility(MapEntities.map);

		MapEntities.map.ClearLayer(1);
		this.handleTurn();
		GD.Print("IT IS ", currentTurn, " turn");
		placeCursor();

	}

	private void handleTurn() {
		if (MapEntities.entities.Count <= 0) {
			MapEntities.entities.Add("PLAYER");
			MapEntities.entities.Add("ENEMY");
			currentTurn = MapEntities.entities.First();
		}

		if (currentTurn == MapEntities.entities.ElementAt(0)) {
			if (MapEntities.count == MapEntities.playableCharacterCount) {
				switchTurns();
			}
		}

	
	}

	private void switchTurns() {
		turnIndex = turnIndex + 1 == MapEntities.entities.Count ? 0 : turnIndex + 1;
		currentTurn = MapEntities.entities.ElementAt(turnIndex);
	}

	public override void physicsUpdate(double delta)
	{	

		if (currentTurn == MapEntities.entities.ElementAt(1)) {
			EmitSignal(SignalName.StateChange, this, "EnemyMoveState");
			
		}
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
			MapEntities.selectedCharacter = characterUtility.selectCharacter(currentTileCoords, ref path) as PlayableCharacter;
			combatUtility.setSelectedCharacter(MapEntities.selectedCharacter);
			tileUtility.drawCursor(currentTileCoords);
		}

		if (MapEntities.selectedCharacter != null) {
			if (MapEntities.selectedCharacter.move) {
				bool finished = characterUtility.moveCharacter(ref path, currentTileCoords, lastTile);

				if (finished) {
					// MapEntities.detectedEnemies = combatUtility.detectEnemy(tileUtility);
					tileUtility.drawCursor(currentTileCoords); 
					
					if (MapEntities.selectedCharacter.usedTurn == false) {
						EmitSignal(SignalName.StateChange, this, "DecisionState");
					} else {
						MapEntities.selectedCharacter = null;
					}
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

		MapEntities.cursorCoords = currentTileCoords;

	}

	private void createPath() {
		
		if (MapEntities.selectedCharacter.usedTurn == true) {
			MapEntities.selectedCharacter = null;
			return;
		}

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
					MapEntities.selectedCharacter.moveSteps = Mathf.Min(MapEntities.selectedCharacter.moveSteps + 1, MapEntities.selectedCharacter.getCharacterStats().speed);
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
		MapEntities.selectedCharacter.moveSteps -= 1;

		if (MapEntities.selectedCharacter.moveSteps < 0) {
			path.Remove(currentTileCoords);
			MapEntities.selectedCharacter.moveSteps += 1;
			tileUtility.eraseCursor(currentTileCoords);
			tileUtility.drawCursor(previousTileCoords);
			currentTileCoords = previousTileCoords;
		}

	}

	


}
