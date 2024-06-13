using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EnemyTargetSelectionState : State {

	private Trait trait;

	private EnemySelectionUtility enemySelectionUtility;
	public override void enter()
	{
		GD.Print("I am on enemy target selection state");
		MapEntities.selectedCharacter = MapEntities.enemyCharacters.MaxBy(character => character.characterStat.speed);
		this.trait = MapEntities.selectedCharacter.characterStat.trait;
		this.enemySelectionUtility = new EnemySelectionUtility(MapEntities.selectedCharacter as EnemyCharacter, MapEntities.map);
	}

	public override void physicsUpdate(double _delta)
	{
		 switch (trait)
		{
			case Trait.NONE:
				GD.Print("NONE");
				break;
			case Trait.BRAZEN:
				GD.Print("BRAZEN");
				break;    
			case Trait.COWARD:
				GD.Print("COWARD");
				break;    
			case Trait.CUNNING:
				GD.Print("CUNNING");
				List<List<Character>> targets = enemySelectionUtility.findTargetsWithinRange(MapEntities.playableCharacters);
				foreach (List<Character> characterList in targets)
				{
					foreach (Character character in characterList)
					{
						GD.Print(character);
					}
				}

				// Character weakestCharacter = enemySelectionUtility.findWeakestDefenceTarget()
				break;    
			default:
				break;
		}
	}


}
