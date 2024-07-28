using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class StateMachineManager : Node {
	
	[Export]
	TileMap tilemap;

	List<StateMachine> stateMachines;

	StateMachine currentStateMachine;
	public override void _EnterTree()
	{
		stateMachines = new List<StateMachine>();

		stateMachines.Add(new OpenWorldStateMachine());
		stateMachines.Add(new CombatStateMachine());		
	
		this.currentStateMachine = stateMachines.First();
	
	}

	public override void _Ready()
	{
		TileMap instance = MapManager.loadMap(MapList.map1);
		GetParent().AddChild(instance);
		MapEntities.map = instance;

		if (GetParent().GetNode("Camera2D") is Camera2D camera) {
			GD.Print("Camera assigned");
			MapEntities.mapCamera = camera;

		}
		
		foreach (StateMachine stateMachine in this.stateMachines)
		{
			stateMachine.StateMachineChange += onChildTransition;
		}

		AddChild(currentStateMachine);

	}

	public void onChildTransition(string stateMachine, string stateMachineName) {

		if (currentStateMachine.GetType().Name == stateMachineName) return;

		StateMachine newStateMachine = this.stateMachines.FirstOrDefault( stateMachine => stateMachine.GetType().Name == stateMachineName, null);

		if (newStateMachine == null) return;

		currentStateMachine.QueueFree();

		currentStateMachine = newStateMachine;

		AddChild(currentStateMachine);

	}

	

}
