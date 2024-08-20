extends AnimatedSprite2D

func _on_trigger_shape_area_entered(area):
	ChefManager.chefRemove = get_parent()
	ChefManager.chefCanRemove = true
