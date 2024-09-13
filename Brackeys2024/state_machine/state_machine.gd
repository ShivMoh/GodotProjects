extends Node
class_name PlayerStateMachine

var current : State
var states = [] as Array[State]

@export var player : CharacterBody2D

func _ready():
	set_up_states() 
	for state in states:
		state.state_change.connect(_on_state_change)
		state.player = self.player
	current = states[0]
	current.enter()

func _physics_process(delta):
	current.physics_update(delta)
	
func _process(delta):
	current.update(delta)
		
func _on_state_change(state_name : String, current_state : State, direction : Vector2 = Vector2.ZERO):
	
	var new_state : State
	
	for state in states:
		if state_name == state.get_script().resource_path.get_file():	
			new_state = state
			break
			
	if not new_state:
		return
	new_state.direction = direction	
	new_state.enter()
	current = new_state
	
	 			
func set_up_states():
	states.push_back(WalkState.new())
	states.push_back(JumpState.new())
	states.push_back(FloatState.new())
	states.push_back(WindState.new())	
		
