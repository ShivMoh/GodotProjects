using System.Linq;
using System.Numerics;
using Godot;

using Vector2 = Godot.Vector2;
public partial class OpenWorldExploreState : State {

	private float speed = 100.0f;
	public override void enter()
	{
		MapEntities.selectedCharacter = MapEntities.playableCharacters.First();
		MapEntities.selectedCharacter.open = true;
		GD.Print(MapEntities.selectedCharacter.YSortEnabled);

		//GD.Print(this.Name);
	}

	private bool changingMap = false;
	public override void physicsUpdate(double _delta)
	{

			Vector2 direction = Input.GetVector("left", "right", "up", "down");

			MapEntities.selectedCharacter.Velocity = direction * speed;

			MapEntities.selectedCharacter.playAnimation(direction);

			MapEntities.selectedCharacter.MoveAndSlide();        

			MapEntities.mapCamera.GlobalPosition = MapEntities.selectedCharacter.GlobalPosition;

			if (Input.IsActionJustPressed("select")) {
				EmitSignal(SignalName.StateChange, this, nameof(OpenWorldFinalState));
			}
	
		//GD.Print(MapManager.determineBounds(MapEntities.selectedCharacter.Position));

		if(Input.IsActionJustPressed("cancel")) {
		
		//   TileMap instance = MapManager.loadMap(MapList.mapNodes.First());
		//   GetParent().GetNode("MapManager").AddChild(instance);  
		
			EmitSignal(SignalName.StateChange, this, nameof(OpenWorldFinalState));
		
			// MapManager.loadMap(
			// 	MapList.mapNodes.ElementAt(1).path
			// );

	

		}
		
	}

}
