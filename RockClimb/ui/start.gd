extends Node2D

func _ready():
	pass

func _on_play_pressed():
	get_tree().change_scene_to_file("res://levels/tile_map.tscn")
	
func _on_quit_pressed():
	get_tree().quit();


func _on_play_again_pressed():
	get_tree().change_scene_to_file("res://levels/tile_map.tscn")


func _on_back_to_main_pressed():
	get_tree().change_scene_to_file("res://ui/start.tscn")
