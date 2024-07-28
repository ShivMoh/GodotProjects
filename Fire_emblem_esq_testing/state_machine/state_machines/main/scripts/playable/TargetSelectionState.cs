using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TargetSelectionState : State {

	// private EnemyCharacter selectedEnemy;
	private TileUtility tileUtilitiy;

	private CombatUtility combatUtility;
	private Character selectedEnemy;

	int numberOfSelectedEnemies;
	
	private int selectIndex = 0;

	private Vector2I currentTileCoords;

	private Vector2I previousTileCoords;

	private int cursorRadius;

	private List<Vector2I> radiusCoords;
	public override void enter()
	{
		GD.Print("I am on target selection");
		this.tileUtilitiy = new TileUtility(MapEntities.map);
		this.combatUtility = new CombatUtility(
			MapEntities.map,
			MapEntities.characters,
			MapEntities.selectedCharacter
		);
		MapEntities.detectedEnemies = combatUtility.detectEnemiesForAttack(tileUtilitiy, MapEntities.chosenAttack);
		selectedEnemy = MapEntities.detectedEnemies.First();
		this.highLightEnemies();
		this.numberOfSelectedEnemies = 0;
		this.currentTileCoords = MapEntities.map.LocalToMap(MapEntities.selectedCharacter.GlobalPosition);
		this.cursorRadius = (int) MapEntities.chosenAttack.attackTargetMeta.radius;

		if (this.cursorRadius > 1) {
			this.radiusCoords = new List<Vector2I>();
		}
		this.placeCursor();

		foreach (Character character in MapEntities.playableCharacters)
		{
			GD.Print("Playable character healths target selection state", character.characterStat.health);
		}
	}

	public override void physicsUpdate(double _delta)
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

	
		if (Input.IsActionJustPressed("select")) {
		
			if (this.cursorRadius == 1) {

				this.detectEnemyPresence();

				if (this.selectedEnemy != null) {
						if (!MapEntities.targetedCharacters.Contains(this.selectedEnemy) && this.selectedEnemy != null) { 
						MapEntities.targetedCharacters.Add(this.selectedEnemy);
						
						this.numberOfSelectedEnemies += 1;
					}

				} else {
					
				}

				if (this.numberOfSelectedEnemies == MapEntities.chosenAttack.attackTargetMeta.targetableCount ||
					this.numberOfSelectedEnemies == MapEntities.detectedEnemies.Count()
				) {
					EmitSignal(SignalName.StateChange, this, typeof(AttackState).ToString());
				}

			} else {
				this.detectEnemiesInRadius();
				EmitSignal(SignalName.StateChange, this, typeof(AttackState).ToString());

			}
		}


		if (Input.IsActionJustPressed("cancel")) {
			// MapEntities.map.ClearLayer(1);
			EmitSignal(SignalName.StateChange, this, nameof(AttackSelectionState));
		}

		// this.highLightTarget();	
		this.highLightTargetable();
		this.highLightSelected();	
	}

	private void detectEnemiesInRadius() {
		foreach (Character character in MapEntities.detectedEnemies)
		{
			if(this.radiusCoords.Contains(MapEntities.map.LocalToMap(character.GlobalPosition))) {
				MapEntities.targetedCharacters.Add(character);
			}
		}
	}

	private void detectEnemyPresence() {

		if (cursorRadius == 1) {
			this.selectedEnemy = MapEntities.detectedEnemies.FirstOrDefault(
									enemy => MapEntities.map.LocalToMap(enemy.GlobalPosition) == this.currentTileCoords, 
									null
								);
		} else {
				this.selectedEnemy = MapEntities.detectedEnemies.FirstOrDefault(
									enemy => MapEntities.map.LocalToMap(enemy.GlobalPosition) == this.currentTileCoords, 
									null
								);
		}
	}

	private void placeCursor() {
		if (cursorRadius == 1) {
			this.tileUtilitiy.eraseCursor(previousTileCoords);
			this.tileUtilitiy.drawCursor(currentTileCoords);

		} else {

			int k = (cursorRadius - 1) / 2;
			int numberOfIterations = 0;
	
			// this.tileUtilitiy.highLight(currentTileCoords, new Vector2I(2, 1));
			
		
			this.radiusCoords.Clear();
			while(numberOfIterations < k) {

				Vector2I currentCoord = new Vector2I(currentTileCoords.X - (numberOfIterations + 1), currentTileCoords.Y - (numberOfIterations + 1));
				Vector2I previousCoord = new Vector2I(previousTileCoords.X - (numberOfIterations + 1), previousTileCoords.Y - (numberOfIterations + 1));

				int loopCount = 2 * (numberOfIterations + 1) + 1 - 1;
				
				// top
				for(int i = 0; i < loopCount; i++) {
					currentCoord.X++;
					previousCoord.X++;


					this.tileUtilitiy.eraseCursor(previousCoord);
				}

				// right
				for(int i = 0; i < loopCount; i++) {
					currentCoord.Y++;
					previousCoord.Y++;

					
					this.tileUtilitiy.eraseCursor(previousCoord);
				}

				// bottom
				for(int i = 0; i < loopCount; i++) {
					currentCoord.X--;
					previousCoord.X--;

					
					this.tileUtilitiy.eraseCursor(previousCoord);
				}

				// left
				for(int i = 0; i < loopCount; i++) {
					currentCoord.Y--;
					previousCoord.Y--;

					
					this.tileUtilitiy.eraseCursor(previousCoord);
				}

				numberOfIterations++;

			}
			
			numberOfIterations = 0;

			this.tileUtilitiy.eraseCursor(previousTileCoords);
			this.tileUtilitiy.drawCursor(currentTileCoords);
	
			while(numberOfIterations < k) {

				Vector2I currentCoord = new Vector2I(currentTileCoords.X - (numberOfIterations + 1), currentTileCoords.Y - (numberOfIterations + 1));
				Vector2I previousCoord = new Vector2I(previousTileCoords.X - (numberOfIterations + 1), previousTileCoords.Y - (numberOfIterations + 1));

				int loopCount = 2 * (numberOfIterations + 1) + 1 - 1;
			
				// top
				for(int i = 0; i < loopCount; i++) {
					currentCoord.X++;
					previousCoord.X++;
					this.radiusCoords.Add(currentCoord);

					this.tileUtilitiy.drawCursor(currentCoord);
				}

				// right
				for(int i = 0; i < loopCount; i++) {
					currentCoord.Y++;
					previousCoord.Y++;
					this.radiusCoords.Add(currentCoord);
					
					this.tileUtilitiy.drawCursor(currentCoord);
				}

				// bottom
				for(int i = 0; i < loopCount; i++) {
					currentCoord.X--;
					previousCoord.X--;
					this.radiusCoords.Add(currentCoord);

					this.tileUtilitiy.drawCursor(currentCoord);
				}

				// left
				for(int i = 0; i < loopCount; i++) {
					currentCoord.Y--;
					previousCoord.Y--;
					this.radiusCoords.Add(currentCoord);

					this.tileUtilitiy.drawCursor(currentCoord);
				}

				numberOfIterations++;

			}

			
		}
	}

	private void highLightSelected() {
		foreach (Character character in MapEntities.targetedCharacters)
		{
		
			Vector2I localPosition = MapEntities.map.LocalToMap(character.GlobalPosition);

			if (currentTileCoords == localPosition) {
				this.placeCursor();
			} else {
				this.tileUtilitiy.highLight(
					localPosition,
					new Vector2I(0, 0)
				);
			}

		}
	}

	private void highLightTargetable() {
		foreach (Character character in MapEntities.detectedEnemies)
		{
			Vector2I localPosition = MapEntities.map.LocalToMap(character.GlobalPosition);

			if (currentTileCoords == localPosition) {
				this.placeCursor();
			} else {
				this.tileUtilitiy.highLight(
					localPosition,
					new Vector2I(0, 0)
				);

			}
		}
	}


	private void highLightTarget() {
		tileUtilitiy.highLight(
			MapEntities.map.LocalToMap(selectedEnemy.GlobalPosition), 
			new Vector2I(2, 1)
		);

		selectedEnemy = MapEntities.detectedEnemies.ElementAt(selectIndex);
		
		this.tileUtilitiy.highLight(
			MapEntities.map.LocalToMap(selectedEnemy.GlobalPosition),
			new Vector2I(0, 1)
		);
	}	

	private void highLightEnemies() {
		foreach (Character enemy in MapEntities.detectedEnemies) 
		{
			tileUtilitiy.highLight(MapEntities.map.LocalToMap(enemy.GlobalPosition), new Vector2I(2, 1));
		}
	}

}
