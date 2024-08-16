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

	public PathUtility pathUtility;
	public override void _Ready()
	{

		this.pathUtility = new PathUtility(tileMap);
		this.pathUtility.setUpAStarGrid(0);

		// path = new Stack<Vector2I>();

	}
	public override void _PhysicsProcess(double delta)
	{

			this.pathUtility.generatePath(
				this.Position,
				this.target.Position
				);

			this.pathUtility.showPath();
			this.wait = false;

	
	}


}
