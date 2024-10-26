extends CharacterBody2D
class_name Enemy

const SPEED = 300.0
const JUMP_VELOCITY = -400.0

@export var health : int = 100
var label : Label
# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

func _ready():
	label = $Label
	label.text = str(health)
var randint	= -1
func _physics_process(delta):
	# Add the gravity.
	if not is_on_floor():
		velocity.y += gravity * delta
		
	var direction : int = -1
	
	if randint == 0:
		direction = -1
	if randint == 1:
		direction = 1
	
	velocity.x = direction * SPEED
	
	
	update_label()
	move_and_slide()

func update_label():
	label.text = str(health)



func _on_timer_timeout():
	print("direction should be changing")
	var rng = RandomNumberGenerator.new()
	randint = rng.randi_range(0, 1)
