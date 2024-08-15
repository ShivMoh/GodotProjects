using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Test_Seeker_AStar : CharacterBody2D
{

	[Export]
	CharacterBody2D target;

	[Export]
	TileMap tileMap;
	public const float Speed = 300.0f;

	Stack<Vector2I> path;

	bool wait = true;

	public override void _Ready()
	{

		
		path = new Stack<Vector2I>();

	}
	public override void _PhysicsProcess(double delta)
	{

		if (wait) {

			AStarGrid2D aStarGrid2D = new AStarGrid2D();
			aStarGrid2D.Region = tileMap.GetUsedRect();
			aStarGrid2D.CellSize = new Vector2I(64, 64);
			aStarGrid2D.DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan;
			aStarGrid2D.DefaultEstimateHeuristic = AStarGrid2D.Heuristic.Manhattan;
			aStarGrid2D.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;

			aStarGrid2D.Update();



			foreach(Vector2I cell in tileMap.GetUsedCells(0)) {
				bool solid = isSpotSolid(cell);
				if (solid) {
					GD.Print(solid);
					GD.Print(cell);
					tileMap.SetCell(0, cell, 1, new Vector2I(0, 1));
					aStarGrid2D.SetPointSolid(cell, true);

				}
			}


			foreach(Vector2I coord in aStarGrid2D.GetIdPath(tileMap.LocalToMap(this.Position), tileMap.LocalToMap(target.Position))) {

				tileMap.SetCell(0, coord, 1, new Vector2I(2, 1));
				path.Push(coord);
			}

			if (path.Count > 0) {
				wait = false;
			}


		} else {
			// GD.Print("should be moving");
			// MoveCharacter();
			MoveAndSlide();

		}
	}

	private void MoveCharacter() {

		if (path.Count > 0) {
			this.Position = path.Pop();
		} else {
			// GD.Print("Reached position", tileMap.LocalToMap(this.Position), tileMap.LocalToMap(target.Position));
		}
	}

	private bool isSpotSolid(Vector2I spot) {
		return (bool) tileMap.GetCellTileData(0, spot).GetCustomData("isSolid");
	}
}
