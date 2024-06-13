using System.Collections.Generic;
using Godot;

public partial class EnemySelectionUtility {
    private List<PlayableCharacter> playableCharacters;

    private EnemyCharacter enemyCharacter;

    private TileMap tileMap;

    private List<AttackMeta> availableAttacks;

    public EnemySelectionUtility(
        List<PlayableCharacter> playableCharacters,
        EnemyCharacter enemyCharacter,
        TileMap tileMap
    ) {
        this.playableCharacters = playableCharacters;
        this.enemyCharacter = enemyCharacter;
        this.tileMap = tileMap;
    }

	public Character findTargetWithinRange() {

        Vector2I enemyTileLocation = tileMap.LocalToMap(enemyCharacter.GlobalPosition);

		PlayableCharacter targetCandidate = null;
		foreach (PlayableCharacter playableCharacter in MapEntities.playableCharacters)
		{
            int greatestRange = this.getGreatestRange(playableCharacter);
            Vector2I playableTileLocation = tileMap.LocalToMap(playableCharacter.GlobalPosition);

            int numberOfStepsTo = Mathf.Abs(playableTileLocation.Y - enemyTileLocation.Y) + 
            Mathf.Abs(playableTileLocation.X - enemyTileLocation.X) - (greatestRange + 1);

            if (numberOfStepsTo <= enemyCharacter.moveSteps) {
                if (targetCandidate != null) {
                    if (enemyCharacter.characterStat.strenth > enemyCharacter.characterStat.magic) {
                        if (targetCandidate.characterStat.physicalDefence > playableCharacter.characterStat.physicalDefence) {
                            targetCandidate = playableCharacter;
                        }
                    } else {
                        if (targetCandidate.characterStat.magic > playableCharacter.characterStat.magic) {
                            targetCandidate = playableCharacter;
                        }
                    }
                       
                }
            }    
			// float distanceBetween = Mathf.Sqrt(
			// 	Mathf.Pow(playableCharacter.GlobalPosition.X - enemyCharacter.GlobalPosition.X, 2) +                 
			// 	Mathf.Pow(playableCharacter.GlobalPosition.Y - enemyCharacter.GlobalPosition.Y, 2) 
			// );

		}
        return targetCandidate;
	}

    private int getGreatestRange(PlayableCharacter playableCharacter) {
        int greatestRange = 0;
        foreach (AttackMeta attack in playableCharacter.attacks)
        {
            if (attack.attackTargetMeta.range > greatestRange && !attack.attackTargetMeta.areaOfEffect) {
                greatestRange = attack.attackTargetMeta.range;
            }
        }
        return greatestRange;
    }
    public void findWeakestTarget() {}

    public void findStrongestTarget() {}
    public void findLowestHealthTarget() {}

    // the area where a given multi attack will have the most effect
    public void findLargestCharacterPool() {}
    
    // like a target u can attack from a distance
    // or without getting hurt back
    public void findSafestToAttackTarget() {}
}