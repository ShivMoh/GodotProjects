using Godot;
using System.Linq;

public partial class OpenWorldFinalState : State {

    public override void enter()
    {
        GD.Print("On open world final state");
        MapEntities.selectedCharacter = MapEntities.playableCharacters.First();
        MapEntities.playableCharacters.Clear();
        MapEntities.characters.Remove(MapEntities.selectedCharacter);
        MapEntities.playableCharacters.Clear();
    }

    public override void physicsUpdate(double _delta)
    {

        if (MapManager.determineBounds(MapEntities.selectedCharacter.Position) is not Direction.NONE and Direction direction ) {
            GD.Print("FSJDFJDFJS");
			MapManager.loadMap(
				MapList.mapNodes.ElementAt(1).path
			);

            foreach(Character character in MapEntities.characters) {
                character.QueueFree();
            }

            MapEntities.characters.Clear();
            MapEntities.enemyCharacters.Clear();
            MapEntities.selectedCharacter = null;

            switch (direction)
            {
                case Direction.LEFT:
                    MapManager.loadMap(
                      MapList.mapNodes.ElementAt(1).path
                    );
                    break;
                default:
                    break;
            }
               
            EmitSignal(SignalName.StateChange, this, nameof(OpenWorldInitialState));
            
        } else {
            GD.Print("Switching states");
            this.switchStateMachine = true;
        }
    }
}