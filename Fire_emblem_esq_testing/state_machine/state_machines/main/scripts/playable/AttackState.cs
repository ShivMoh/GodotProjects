using System.Linq;
using Godot;

public partial class AttackState : State {

	private CombatUtility combatUtility;
	public override void enter()
	{
		GD.Print("I am on attacking state");
		// this.tileUtility = new TileUtility(MapEntities.map);
		this.combatUtility = new CombatUtility(MapEntities.map, MapEntities.characters, MapEntities.selectedCharacter);
	}

	public override void physicsUpdate(double _delta)
	{

		foreach (Character character in MapEntities.targetedCharacters)
		{
			combatUtility.attackCharacter(MapEntities.selectedCharacter, character, MapEntities.chosenAttack);			
		}

		EmitSignal(SignalName.StateChange, this, typeof(FinalState).ToString());
	}
}   
