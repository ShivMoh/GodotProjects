extends CharacterBody2D


const SPEED = 1.0
const JUMP_VELOCITY = -400.0

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")
@onready var animated_sprite = $AnimatedSprite2D
 
var move : bool = false
func _physics_process(delta):

	move = false
	if(Input.is_action_pressed("ui_left")):
		position.x += -SPEED
		animated_sprite.play("left")
		move = true
		
	if(Input.is_action_pressed("ui_right")):
		position.x += SPEED
		animated_sprite.play("right")
		move = true
	
	if(Input.is_action_pressed("ui_up")):
		position.y += -SPEED
		animated_sprite.play("back")
		move = true
	
	if(Input.is_action_pressed("ui_down")):
		position.y += SPEED
		animated_sprite.play("front")
		move = true

	if !move:
		animated_sprite.play("idle")	
	#move_and_slide()
