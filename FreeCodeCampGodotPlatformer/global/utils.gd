extends Node

# for releases
# should probably look into more
# const SAVE_PATH = "users://savegame.bin"
const SAVE_PATH = "res://savegame.bin"

func saveGame():
	var file = FileAccess.open(SAVE_PATH, FileAccess.WRITE)
	var data: Dictionary = {
		"playerHealth" = Game.playerHealth,
		"gold" = Game.gold
	}
	
	var jsonString = JSON.stringify(data)
	file.store_line(jsonString)
	
func loadGame():
	var file = FileAccess.open(SAVE_PATH, FileAccess.READ)
	if FileAccess.file_exists(SAVE_PATH):
		if not file.eof_reached():
			var current_line = JSON.parse_string(file.get_line())
			if current_line:
				Game.playerHealth = current_line["playerHealth"]
				Game.gold = current_line["gold"]
	
