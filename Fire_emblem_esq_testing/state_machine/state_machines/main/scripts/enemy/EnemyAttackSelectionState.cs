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

		MapEntities.targetedCharacters.AddRange(this.attackSelectionUtility.getTargets());

		EmitSignal(SignalName.StateChange, this, "EnemyMoveState");
	}

}
