extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0

const bullet = preload("res://bullet.tscn")


# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

@onready var animationSprite = $AnimatedSprite2D

func _physics_process(delta):
	# Add the gravity.
	if not is_on_floor():
		velocity.y += gravity * delta

	# Handle Jump.
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("ui_left", "ui_right")
	if direction:
		velocity.x = direction * SPEED
		
		if(direction == -1):
			animationSprite.flip_h = true
		else:
			animationSprite.flip_h = false
		animationSprite.play("walk")
		
		
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		animationSprite.play("idle")
	
	if Input.is_action_pressed("ui_text_delete"):
		spawn_bullet()

		
	move_and_slide()


func spawn_bullet():
	print("bullet has been spawned")
	var bullet_instance = bullet.instantiate()
	bullet_instance.position = self.position + Vector2(0, 0)
	get_parent().add_child(bullet_instance)
	
	var end_point : Vector2 = bullet_instance.position + Vector2(200, 0)
	var final_value = fire_bullet(bullet_instance, end_point)

func fire_bullet(bullet: Area2D, end_point : Vector2):
	var fire_tween = get_tree().create_tween()
	var vanish_tween = get_tree().create_tween()
	fire_tween.tween_property(bullet, "position", end_point, 0.5)
	vanish_tween.tween_property(bullet, "modulate:a", 0, 0.7)
	vanish_tween.tween_callback(bullet.queue_free)


	
	
	
	
