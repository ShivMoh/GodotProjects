using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public partial class state_machine : Node
{

	[Export]
	public state initialState;

	public state currentState;

	public List<state> states = new List<state>();
	public override void _Ready()
	{
		base._Ready();
		foreach(state child in this.GetChildren()) 
		{

			if (child is state)
			{

				states.Add(child);

				child.Transitioned += OnChildTransition;
			}
		}


		if (initialState is not null)
		{
			currentState = initialState;
			currentState.Enter();
		}
		
	}

	public override void _Process(double delta)
	{
		this.currentState.Update(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		this.currentState.PhysicsUpdate(delta);
	}

	public void OnChildTransition(state state, string newStateName)
	{

		GD.Print(state, newStateName, currentState);
		
		if (state != currentState) 
		{
			return;
		}

		state newState = states.FirstOrDefault<state>((state state) => state.Name == newStateName);
		
		if (newState is null) 
		{
			return;
		}

		if (currentState is not null) 
		{
			currentState.Exit();
		}

		currentState = newState;
		currentState.Enter();
	}

}
