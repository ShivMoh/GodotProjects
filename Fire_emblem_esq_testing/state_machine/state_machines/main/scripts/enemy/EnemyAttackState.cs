using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnemyAttackState : State {

	private CombatUtility combatUtility;

	private List<bool> removalList;
	public override void enter()
	{
		// GD.Print("Am on enemy attack state");

		this.combatUtility = new CombatUtility(
			MapEntities.map,
			MapEntities.characters,
			MapEntities.selectedCharacter
		);

		this.removalList = new List<bool>();
	}

	public override void physicsUpdate(double _delta)
	{
		
		
		removalList.Clear();
		
		foreach (Character character in MapEntities.targetedCharacters)
		{
			GD.Print("Character health before attack", character.characterStat.health);
			bool remove = this.combatUtility.attackCharacter(MapEntities.selectedCharacter, character, MapEntities.chosenAttack);
			removalList.Add(remove);
		}

		foreach (bool item in this.removalList)
		{
			if (item == true) {
				Character removedCharacter = MapEntities.targetedCharacters.ElementAt(removalList.IndexOf(item));
				MapEntities.targetedCharacters.Remove(removedCharacter);
				removedCharacter.QueueFree();
			}
		}
				
		EmitSignal(SignalName.StateChange, this, typeof(FinalState).ToString());

	}
}
