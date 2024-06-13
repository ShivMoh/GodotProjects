using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TargetSelectionState : State {

	// private EnemyCharacter selectedEnemy;
	private TileUtility tileUtilitiy;

	private CombatUtility combatUtility;
	private EnemyCharacter selectedEnemy;
	
	private int selectIndex = 0;
	public override void enter()
	{
		GD.Print("I am on target selection");
		this.tileUtilitiy = new TileUtility(MapEntities.map);
		this.combatUtility = new CombatUtility(
			MapEntities.map,
			MapEntities.enemyCharacters,
			MapEntities.selectedCharacter
		);
		MapEntities.detectedEnemies = combatUtility.detectEnemy(tileUtilitiy);
		selectedEnemy = MapEntities.detectedEnemies.First();
		this.highLightEnemies();
	}

	public override void physicsUpdate(double _delta)
	{

		if (Input.IsActionJustPressed("down") || Input.IsActionJustPressed("left")) {
			selectIndex = selectIndex + 1 > MapEntities.detectedEnemies.Count() - 1 ? 0 : selectIndex + 1;
		}

		if (Input.IsActionJustPressed("up") || Input.IsActionJustPressed("right")) {
			selectIndex = selectIndex - 1 < 0 ? MapEntities.detectedEnemies.Count() - 1 : selectIndex - 1; 
		}

		if (Input.IsActionJustPressed("select")) {
			MapEntities.targetedCharacters.Add(this.selectedEnemy);

			if (MapEntities.chosenAttack.attackTargetMeta.range >= 1) {
				EmitSignal(SignalName.StateChange, this, "AttackState");
			}
		}

		this.highLightTarget();		
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
		foreach (EnemyCharacter enemy in MapEntities.detectedEnemies) 
		{
			tileUtilitiy.highLight(MapEntities.map.LocalToMap(enemy.GlobalPosition), new Vector2I(2, 1));
		}
	}

}
