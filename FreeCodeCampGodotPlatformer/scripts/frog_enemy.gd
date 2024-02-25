extends CharacterBody2D

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")
var player
var chase_player = false
const SPEED = 50

func _ready():
	$AnimatedSprite2D.play("idle")
	
func _physics_process(delta):
	velocity.y += gravity * delta

	player = $"../../player/player"
	var direction = (player.global_position - self.position).normalized()
		
	if chase_player:
		if direction.x > 0:
			$AnimatedSprite2D.flip_h = true
		elif direction.x < 0:
			$AnimatedSprite2D.flip_h = false
		
		if $AnimatedSprite2D.animation != "death":
			$AnimatedSprite2D.play("jump")
			velocity.x = direction.x * SPEED
	else:
		if $AnimatedSprite2D.animation != "death":
			$AnimatedSprite2D.play("idle")
		velocity.x = 0
		
	move_and_slide()
	
func _on_area_2d_body_entered(body):
	if body.name == "player":
		chase_player = true
		
func _on_area_2d_body_exited(body):
	if body.name == "player":
		chase_player = false

func _on_player_death_body_entered(body):
	if body.name == "player":
		frog_death()

func _on_player_collison_body_entered(body):
	if body.name == "player":
		Game.playerHealth -= 3
		frog_death()
	if body.name == "eagle_ammo":
		frog_death()
		
func frog_death():
	Game.gold += 2
	Utils.saveGame()
	
	chase_player = false
	$AnimatedSprite2D.play("death")
		
	await $AnimatedSprite2D.animation_finished
	self.queue_free()
	
