using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class tile_map : TileMap
{


	[Export]
	player_test character;

	Vector2I current_tile_coords;

	Queue<Vector2I> path = new Queue<Vector2I>();

	bool move = false;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("hello world");
		// var tiles = this.GetUsedCells(0);
		// GD.Print(this.GetCellTileData(0, tiles[0]));
		current_tile_coords = new Vector2I(0, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{	
		if (Input.IsActionJustPressed("ui_right")) {
			current_tile_coords.X += 1;
			this.placeCursor();
		}

		if (Input.IsActionJustPressed("ui_left")) {
			current_tile_coords.X -= 1;
			this.placeCursor();
		}

		if (Input.IsActionJustPressed("ui_up")) {
			current_tile_coords.Y -= 1;
			this.placeCursor();

		}

		if (Input.IsActionJustPressed("ui_down")) {
			current_tile_coords.Y += 1;
			this.placeCursor();

		}

		if (Input.IsActionJustPressed("ui_text_delete")) {
			GD.Print("hello?");
			moveChacater();
			move = true;
		}

		if (move) {
			moveChacater();
		}

	}

	// private void updateCharacterPosition(Vector2I toCoords) {
	// 	character.Position = target_position;
	// }

	private void moveChacater() {

		if (Math.Abs(character.GlobalPosition.X - character.target_position.X) < 2.0f
			&& Math.Abs(character.GlobalPosition.Y - character.target_position.Y) < 2.0f
		) {
			Vector2I next_tile = path.Dequeue();
			character.target_position = this.MapToLocal(next_tile);
			GD.Print("is this working");
		} 

	

		if (path.Count <= 0) {
			move = false;
		}
	}

	private void placeCursor() {
		this.SetCell(
			0,
			current_tile_coords,
			7,
			new Vector2I(1, 0)
		);

		path.Enqueue(current_tile_coords);
	}
	

}
