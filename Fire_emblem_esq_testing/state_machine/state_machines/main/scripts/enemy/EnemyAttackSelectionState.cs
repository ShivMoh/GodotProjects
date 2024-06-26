using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Godot;

public partial class EnemyAttackSelectionState : State {


	private AttackSelectionUtility attackSelectionUtility;
	public override void enter()
	{
		this.attackSelectionUtility = new AttackSelectionUtility(
			MapEntities.selectedCharacter as EnemyCharacter,
			MapEntities.targetCandidates,
			MapEntities.attackMetas,
			MapEntities.closeRangeTargets            
		);
	}

	public override void physicsUpdate(double _delta)
	{
		this.attackSelectionUtility.chooseCunningCharacterAttack();
		MapEntities.chosenAttack = this.attackSelectionUtility.getAttack();
		MapEntities.targetedCharacters.Clear();
		MapEntities.targetedCharacters.AddRange(this.attackSelectionUtility.getTargets());

		if (MapEntities.targetedCharacters.Count() == 0 || MapEntities.targetedCharacters.Contains(null)) {
			EmitSignal(SignalName.StateChange, this, typeof(EnemyTargetSelectionState).ToString());
		} else {
			EmitSignal(SignalName.StateChange, this, typeof(EnemyMoveState).ToString());
		}	
	}

}
