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

    public void findClosestTarget() {}

    public void findWeakestTarget() {}

    public void findStrongestTarget() {}
    public void findLowestHealthTarget() {}

    // the area where a given multi attack will have the most effect
    public void findLargestCharacterPool() {}
    
    // like a target u can attack from a distance
    // or without getting hurt back
    public void findSafestToAttackTarget() {}
}