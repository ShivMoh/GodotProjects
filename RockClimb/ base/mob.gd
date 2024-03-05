extends CharacterBody2D
class_name Mob

@export var health : int = 6

func get_health() -> int:
    return self.health

func set_health(new_health : int):
    self.health = new_health
    

