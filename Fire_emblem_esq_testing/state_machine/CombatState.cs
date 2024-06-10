using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CombatState : State {

	private EnemyCharacter targetedEnemy;
	private TileUtility tileUtilitiy;

	private int selectIndex = 0;
	public override void enter()
	{

		GD.Print("I have entered the combat state");


		this.tileUtilitiy = new TileUtility(MapEntities.map);
		targetedEnemy = MapEntities.detectedEnemies.First();
	}

	public override void physicsUpdate(double _delta)
	{

		if (Input.IsActionJustPressed("up") || Input.IsActionJustPressed("right")) {

			selectIndex = Mathf.Min(MapEntities.detectedEnemies.Count() - 1, selectIndex + 1);

		}

		if (Input.IsActionJustPressed("down") || Input.IsActionJustPressed("left")) {
			selectIndex = Mathf.Max(0, selectIndex - 1);

		}

		this.selectTarget();
	}

	private void selectTarget() {
		this.tileUtilitiy.eraseCursor(
			MapEntities.map.LocalToMap(targetedEnemy.GlobalPosition)
		);

		targetedEnemy = MapEntities.detectedEnemies.ElementAt(selectIndex);
		
		this.tileUtilitiy.highLight(
			MapEntities.map.LocalToMap(targetedEnemy.GlobalPosition),
			new Vector2I(0, 1)
		);
	}	


}
