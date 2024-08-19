class_name ChefStationActions extends Resource


@export var itemIn : String
@export var itemOut : String

@export var actionTime : float

func _station_action(chef : Chef, option : String):
	
	if option == "itemIn":
		chef.currentHeldItem = itemOut
