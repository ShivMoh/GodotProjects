using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class AttackState : State {

	private CombatUtility combatUtility;
	public override void enter()
	{
		GD.Print("I am on attacking state");
		// this.tileUtility = new TileUtility(MapEntities.map);
		this.combatUtility = new CombatUtility(MapEntities.map, MapEntities.characters, MapEntities.selectedCharacter);
		GD.Print("Stats", MapEntities.characters.Count(), MapEntities.playableCharacters.Count(), MapEntities.enemyCharacters.Count());

	}

	public override void physicsUpdate(double _delta)
	{
		List<bool> removalList = new List<bool>();


		foreach (Character character in MapEntities.targetedCharacters)
		{			
			bool remove = combatUtility.attackCharacter(MapEntities.selectedCharacter, character, MapEntities.chosenAttack);			
			removalList.Add(remove);
		}
		
		foreach (bool item in removalList)
		{
			if (item == true) {
				Character removedCharacter = MapEntities.targetedCharacters.ElementAt(removalList.IndexOf(item));
				MapEntities.targetedCharacters.Remove(removedCharacter);
				removedCharacter.QueueFree();
				GD.Print("Stats", MapEntities.characters.Count(), MapEntities.playableCharacters.Count(), MapEntities.enemyCharacters.Count());
			}
		}

		EmitSignal(SignalName.StateChange, this, nameof(FinalState));
	}
}   
