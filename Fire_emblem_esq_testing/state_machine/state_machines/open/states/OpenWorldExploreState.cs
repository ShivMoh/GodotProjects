using System.Linq;
using System.Numerics;
using Godot;

using Vector2 = Godot.Vector2;
public partial class OpenWorldExploreState : State {

    private float speed = 100.0f;
    public override void enter()
    {
        MapEntities.selectedCharacter = MapEntities.playableCharacters.First();
        GD.Print(this.Name);
    }

    public override void physicsUpdate(double _delta)
    {

        Vector2 direction = Input.GetVector("left", "right", "up", "down");

        MapEntities.selectedCharacter.Velocity = direction * speed;

        MapEntities.selectedCharacter.playAnimation(direction);
        
        MapEntities.selectedCharacter.MoveAndSlide();        

    }
}