using Godot;

public partial class EnemyAttackState : State {

	private CombatUtility combatUtility;
	public override void enter()
	{
		this.combatUtility = new CombatUtility(
			MapEntities.map,
			MapEntities.characters,
			MapEntities.selectedCharacter
		);
	}

	public override void physicsUpdate(double _delta)
	{
		foreach (Character character in MapEntities.targetedCharacters)
		{
			this.combatUtility.attackCharacter(MapEntities.selectedCharacter, character, MapEntities.chosenAttack);
		}
	}
}
