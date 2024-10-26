extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0
@onready var animated_spire : AnimatedSprite2D = $AnimatedSprite2D
# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

var attacking : bool = false

var collision2D : CollisionShape2D 

func _ready():
	collision2D = $Area2D/CollisionShape2D
	collision2D.disabled = true
	animated_spire.play("idle")
	animated_spire.animation_looped.connect(change_animation)

func _physics_process(delta):
	# Add the gravity.
	collision2D.disabled = true
	if not is_on_floor():
		velocity.y += gravity * delta

	# Handle jump.
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("ui_left", "ui_right")
	
	if direction == 1 or direction == 0:
		animated_spire.flip_h = false
	else:
		animated_spire.flip_h = true
			
	if direction:
		velocity.x = direction * SPEED
		if velocity.x > 0:
			animated_spire.play("walk")
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
			
	if Input.is_action_just_pressed("attack"):
		
		attacking = true
		
	if attacking:
		if animated_spire.frame > 4:
			collision2D.disabled = false
		animated_spire.play("attack")
		

	move_and_slide()

func change_animation():
	if attacking:
		attacking = false
		animated_spire.play("idle")
	

func _on_area_2d_body_entered(body):
	
	if body is CharacterBody2D:

		var enemy : Enemy = body
		enemy.health -= 10
		
		if enemy.health <= 0:
			enemy.queue_free()
