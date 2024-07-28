using Godot;
using System.Linq;

public partial class OpenWorldFinalState : State {

    public override void enter()
    {
        GD.Print("On open world final state");
        MapEntities.selectedCharacter = MapEntities.playableCharacters.First();
        MapEntities.playableCharacters.Clear();
        MapEntities.characters.Remove(MapEntities.selectedCharacter);
    }

    public override void physicsUpdate(double _delta)
    {
        GD.Print("Switching states");
        this.switchStateMachine = true;
    }
}