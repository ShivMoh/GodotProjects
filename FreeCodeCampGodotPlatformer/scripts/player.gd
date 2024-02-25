extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0



# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")
var health = 10
var eagle = preload("res://objects/eagle_ammo.tscn")
var gem = preload("res://objects/gem.tscn")


#@onready var animated_sprite = $AnimatedSprite2D
@onready var animation_player = $AnimationPlayer

#func _ready():
	#animated_sprite.play("idle")

func _physics_process(delta):
	

	# Add the gravity.
	if not is_on_floor():
		velocity.y += gravity * delta
		# animation_player.play("fall")

	# Handle Jump.
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		velocity.y = JUMP_VELOCITY
		animation_player.play("jump")
	
	

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("ui_left", "ui_right")
	
	if direction == -1:
		$AnimatedSprite2D.flip_h = true
	elif direction == 1:
		$AnimatedSprite2D.flip_h = false
	if direction:
		velocity.x = direction * SPEED
		#animated_sprite.play("run")
		if velocity.y == 0:
			animation_player.play("run")
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		# animated_sprite.play("idle")
		if velocity.y == 0:
			animation_player.play("idle")
		if velocity.y > 0:
			animation_player.play("fall")
	
	if Input.is_action_just_pressed("ui_text_backspace"):
		fire_eagle()
			
	if Game.playerHealth <= 0:
		queue_free()
		get_tree().change_scene_to_file("res://scenes/main.tscn")

	move_and_slide()
	
func fire_eagle():
	print("I am running")
	var eagleTemp = eagle.instantiate()
	var rng = RandomNumberGenerator.new()
	var randint = rng.randi_range(25, 1000)
	eagleTemp.position = self.position
	add_child(eagleTemp)
	


func create_gem():
	var gemTemp = gem.instantiate()
	var rng = RandomNumberGenerator.new()
	var randint = rng.randi_range(25, 1000)
	gemTemp.position = Vector2(randint, 455)
	$eagles.add_child(gemTemp)


