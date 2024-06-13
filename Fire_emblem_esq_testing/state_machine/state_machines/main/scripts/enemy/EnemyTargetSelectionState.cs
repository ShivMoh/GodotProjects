using Godot;

public partial class EnemyTargetSelectionState : State {


    public override void enter()
    {
        Trait trait = MapEntities.selectedCharacter.characterStat.trait;

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
                break;    
            default:
                break;
        }
    }

    
}
