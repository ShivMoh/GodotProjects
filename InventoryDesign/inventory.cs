using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Numerics;

using Array = System.Array;

public partial class inventory : TileMap
{
	private Vector2I previousCursorPosition;
	private Vector2I cursorPosition;

	private int[,] inventoryState = {
		{0, 0, 0, 0, 0, 0, 0, 0}, 
		{0, 0, 3, 0, 0, 0, 0, 0}, 
		{0, 0, 0, 0, 3, 0, 0, 0}, 
		{0, 0, 0, 0, 0, 0, 0, 0}
	};
	public override void _Ready()
	{
		this.cursorPosition = Vector2I.One;
		this.moveCursor();	
		this.initializeItems();
	}

	public override void _Process(double delta)
	{

		this.previousCursorPosition = cursorPosition;

		if(Input.IsActionJustPressed("up")) {
			this.cursorPosition = new Vector2I(
				this.cursorPosition.X,
				Math.Min(Math.Max(this.cursorPosition.Y - 1, 1), 4)
			);
		}

		if(Input.IsActionJustPressed("down")) {
			this.cursorPosition = new Vector2I(
				this.cursorPosition.X,
				Math.Max(Math.Min(this.cursorPosition.Y + 1, 4), 1)
			);
		}

		if(Input.IsActionJustPressed("right")) {
			this.cursorPosition = new Vector2I(
				Math.Max(Math.Min(this.cursorPosition.X + 1, 16), 1),
				this.cursorPosition.Y
			);
		}

		if(Input.IsActionJustPressed("left")) {
			this.cursorPosition = new Vector2I(
				Math.Min(Math.Max(this.cursorPosition.X - 1, 1), 16),
				this.cursorPosition.Y				
			);
		}

		this.resetCell();
		this.moveCursor();
		
	}

	public void initializeItems() {
		for (int i = 1; i < this.inventoryState.GetLength(0) + 1; i++)
		{
			
			for (int j = 1; j < this.inventoryState.GetLength(1) + 1; j++)
			{	
				if(this.inventoryState[i - 1, j - 1] != 0) {
					this.setItem(new Vector2I(j, i), this.inventoryState[i - 1, j - 1]);
				}
			}
		}
	}

	public void resetCell() {
		this.EraseCell(
			1,
			this.previousCursorPosition
		);	
	}

	public void moveCursor() 
	{
		this.SetCell(
			1,
			this.cursorPosition,
			2,
			new Vector2I(0, 0)
		);
	}

	public void setItem(Vector2I location, int itemId) {
		this.SetCell(
			0, 
			location,
			itemId,
			new Vector2I(0, 0)		
		);
	}


} 
