using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Godot;

public partial class FinalState : State {

	public override void enter()
	{
		(MapEntities.selectedCharacter.GetNode("CollisionShape2D") as CollisionShape2D).Disabled = false;
		MapEntities.selectedCharacter.usedTurn = true;
		MapEntities.selectedCharacter = null;
		MapEntities.count++;
		MapEntities.detectedEnemies.Clear();
		MapEntities.targetedCharacters.Clear();
		MapEntities.chosenAttack = null;

	}

	public override void physicsUpdate(double _delta)
	{
		if (this.previousStateName == typeof(EnemyAttackState).ToString()) {
			if (    MapEntities.count >= MapEntities.enemyCharacters.Count() || 
					MapEntities.playableCharacters.Count() == 0
			) {
				GD.Print("Changine state");
				MapEntities.count = 0;
				foreach (Character character in MapEntities.enemyCharacters)
				{	
					character.usedTurn = false;	
				}

				EmitSignal(SignalName.StateChange, this, typeof(ExploreState).ToString());
			} else {
				EmitSignal(SignalName.StateChange, this, typeof(EnemyTargetSelectionState).ToString());
			}
		}

		if (this.previousStateName == nameof(AttackState) ||
			this.previousStateName == nameof(DecisionState)
		) {
			if ( MapEntities.count >= MapEntities.playableCharacters.Count()) {
				MapEntities.count = 0;
				foreach (Character character in MapEntities.playableCharacters)
				{	
					character.usedTurn = false;	
				}
				EmitSignal(SignalName.StateChange, this, typeof(EnemyTargetSelectionState).ToString());
			} else {
				EmitSignal(SignalName.StateChange, this, typeof(ExploreState).ToString());
			
			}
		}
	}

}
