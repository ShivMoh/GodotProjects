extends Node
class_name State

var state_name : String = ""
signal state_change(state_name : String, current_state : State, direction : Vector2)
var player : CharacterBody2D
var direction : Vector2
func enter():
	pass

func exit():
	pass

func physics_update(delta: float):
	pass

func update(delta: float):
	pass
