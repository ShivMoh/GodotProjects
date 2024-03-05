extends Node
class_name StateMachine

const SPEED = 300.0
const JUMP_VELOCITY = -400.0

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

var current_state: State
var states: Dictionary = {}

@export var initial_state : State

var player: Player

func _ready():
	player = self.owner
	for child in get_children():
		if child is State:
			states[child.name.to_lower()] = child
			child.player = player
			child.Transitioned.connect(on_child_transition)
	if initial_state:
		initial_state.Enter()
		current_state = initial_state
		
func _process(delta):
	if current_state:
		current_state.Update(delta)
		
func _physics_process(delta):
	if current_state:
		current_state.Physics_Update(delta)
		
func on_child_transition(state, new_state_name, _args = []):
	if state != current_state:
		return
	
	var new_state : State = states.get(new_state_name.to_lower())
	
	if !new_state:
		return
		
	if current_state:
		current_state.Exit()
		
	new_state.Enter(_args)
	
	current_state = new_state
	
func basic_movement(delta):
	# Add the gravity.
	if not player.is_on_floor():
		player.velocity.y += gravity * delta

	# Handle Jump.
	if Input.is_action_just_pressed("ui_accept") and player.is_on_floor():
		player.velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("ui_left", "ui_right")
	if direction:
		player.velocity.x = direction * SPEED
	else:
		player.velocity.x = move_toward(player.velocity.x, 0, SPEED)

	player.move_and_slide()



