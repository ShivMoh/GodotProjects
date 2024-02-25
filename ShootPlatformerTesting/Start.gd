# Idle.gd
extends PlayerState
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

func _ready():
	print("We are on start")
	
# Upon entering the state, we set the Player node's velocity to zero.
func enter(_msg := {}) -> void:
	
	print("Entered this node")
	# We must declare all the properties we access through `owner` in the `Player.gd` script.
#	player.velocity = Vector2.ZERO
	
	



func update(delta: float) -> void:

	# If you have platforms that break when standing on them, you need that check for 
	# the character to fall.
	# if not owner.is_on_floor():
	#	print("It changed state")
	#	state_machine.transition_to("Air")
	#	return

	if Input.is_action_just_pressed("ui_up"):
		# As we'll only have one air state for both jump and fall, we use the `msg` dictionary 
		# to tell the next state that we want to jump.
		state_machine.transition_to("Air", {do_jump = true})
	elif Input.is_action_pressed("ui_left") or Input.is_action_pressed("ui_right"):
		state_machine.transition_to("Run")
		
func physics_update(delta: float) -> void:
	if not player.is_on_floor():
		player.velocity.y += gravity * delta
	else:
		player.velocity.y = 0
		
	player.move_and_slide()
