extends CharacterBody2D

@export var moveDistance: int = 150
@export var unitMovement: int = 100


var direction: int = 1
var counter: int = 0
var playerDetected : bool = false

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

func _ready():
	if !$BoarAnimatedSprite.flip_h:
		direction = -1
	else:
		direction = 1
	$BoarAnimatedSprite.play("walk")
	
func _physics_process(delta):
	if not is_on_floor():
		velocity.y += gravity * delta
	
	if is_on_floor():
		if !playerDetected:
			if direction == 1:
				velocity.x = unitMovement * direction
				counter+=1
			else:
				velocity.x = unitMovement * direction
				counter+=1
			if counter == (moveDistance - 1):
				$BoarAnimatedSprite.flip_h = !$BoarAnimatedSprite.flip_h
				direction *= -1
				counter = 0
				velocity.x = 0
		else:
			velocity.x += unitMovement * direction
		
	move_and_slide()
		
func _on_player_detection_area_body_entered(body):
	if body.name == "Player":
		playerDetected = true
		$BoarAnimatedSprite.play("run")
		unitMovement = 3
		if body.position.x > self.position.x:
			direction = 1
			$BoarAnimatedSprite.flip_h = true
		else:
			direction = -1
			$BoarAnimatedSprite.flip_h = false
		
func _on_player_kill_box_body_entered(body):
	if body.collision_layer == 1:
		var playerAnimatedSprite : AnimatedSprite2D = body.get_children()[1]
		body.death = true
		playerAnimatedSprite.play("death")
		await playerAnimatedSprite.animation_finished
		body.queue_free()
