extends CharacterBody2D

const SPEED = 300.0
const JUMP_VELOCITY = -500.0
const ENEMY_COLLISION_LAYER = 4
const PLAYER_SWORD_COLLISION_POSITION_X = 15

@onready var playerAnimatedSprite = $PlayerAnimatedSprites
@onready var playerSwordCollision = $PlayerSwordRange/PlayerSwordCollision

var playerIsAttacking: bool = false
var playerIsFalling: bool = false
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

@export var death : bool = false
var power_up_time = 0
var force = 0
 
func _physics_process(delta):
	if not is_on_floor():
		velocity.y += gravity * delta
		
	if !death:
		if Input.is_action_just_pressed("ui_accept") and is_on_floor():
			playerAnimatedSprite.play("jump_start")
			velocity.y = JUMP_VELOCITY
			
		var direction = Input.get_axis("ui_left", "ui_right")
		if direction:
			if velocity.y >= 0:
				if direction == 1:
					playerAnimatedSprite.flip_h = false
					playerSwordCollision.position.x = PLAYER_SWORD_COLLISION_POSITION_X;
				else:
					playerAnimatedSprite.flip_h = true
					playerSwordCollision.position.x = -PLAYER_SWORD_COLLISION_POSITION_X;
			
			if velocity.y == 0:
				if !playerIsAttacking:
					playerAnimatedSprite.play("run")
				playerIsFalling = false
			
			velocity.x = direction * SPEED
		else:
			velocity.x = move_toward(velocity.x, 0, SPEED)
			if velocity.y == 0 and !playerIsAttacking:
				playerAnimatedSprite.play("idle")
			elif velocity.y > 0 and !playerIsAttacking:
				if !playerIsFalling:
					playerIsFalling = true
					playerAnimatedSprite.play("jump_end")
					
		if Input.is_action_pressed("Dash"):
			var dash_value = dash()
			velocity.x = dash_value
			velocity.y = -200
			
		if Input.is_action_just_released("Dash"):
			force = 0
			power_up_time = 0
			
		if Input.is_action_just_pressed("Fire"):
			playerIsAttacking = true
			$AnimationPlayer.play("new_animation")
			
			# playerSwordCollision.disabled = false
			# playerAnimatedSprite.connect("animation_finished", setFalse)
			# playerAnimatedSprite.connect("animation_changed", setFalse2)
	move_and_slide()
	
func setFalse():
	playerIsAttacking = false
	playerSwordCollision.disabled = true
	
func dash():
	power_up_time += 1
	
	if(power_up_time > 5):
		return get_force(-100)/2 * 5
	# F/m * t**2 = d
	# F/m * t = v
	return get_force(100)/2 * 5
	
	
func get_force(increment):
	force = max(min(force + increment, 500), 0)
	return force
	

	
func _on_player_sword_range_body_entered(body):

	if body.collision_layer == ENEMY_COLLISION_LAYER:
		if len((body.get_children()[3]).get_children()) != 0:
			var boarCollision: CollisionShape2D = (body.get_children()[3]).get_children()[0]
			body.collision_layer = 8
			boarCollision.queue_free()
	
			var boarAnimatedSprite : AnimatedSprite2D = (body.get_children()[0])
			boarAnimatedSprite.play("death")
			await boarAnimatedSprite.animation_finished
			body.queue_free()
		

		
