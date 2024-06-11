using System.Linq;
using Godot;

public partial class AttackState : State {

	private CombatUtility combatUtility;
	public override void enter()
	{
		GD.Print("I am on attacking state");
		// this.tileUtility = new TileUtility(MapEntities.map);
		this.combatUtility = new CombatUtility(MapEntities.map, MapEntities.enemyCharacters, MapEntities.selectedCharacter);
	}

	public override void physicsUpdate(double _delta)
	{
		if (MapEntities.chosenAttack.attackType == AttackType.CLOSE) {
			combatUtility.attackCharacter(MapEntities.selectedCharacter, MapEntities.targetedCharacters.First(), MapEntities.chosenAttack);
		}

		EmitSignal(SignalName.StateChange, this, "ExploreState");
	}
}   
