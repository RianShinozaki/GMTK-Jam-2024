extends Node
#Put in unique types here later
var potLocation : ChefStation
var alcoholLocation : ChefStation
var seasoningLocation : ChefStation
var vegetableLocation : ChefStation
var meatLocation : ChefStation
var stoveLocation : ChefStation
var chopLocation : ChefStation
var serveLocation : ChefStation

var chefRemove : Node
var chefCanRemove : bool

var ladderLocations : Array[Ladder] = [null,null,null]

var panicLevel : int
