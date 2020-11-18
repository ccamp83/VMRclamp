mergeInto(LibraryManager.library, {

	testJS: function (x, y, movementTime, movementSpeed, phase, targetPos, adaptType, rotation, trialN, mouseX, mouseY, time) {
		$('#IVFields').append('<input 	name="phase' + trialN + '" id="phase' + trialN + '" type="hidden" value="' + UTF8ToString(phase) + '">');
		$('#IVFields').append('<input 	name="targetPos' + trialN + '" id="targetPos' + trialN + '" type="hidden" value="' + targetPos + '">');
		$('#IVFields').append('<input 	name="adaptType' + trialN + '" id="adaptType' + trialN + '" type="hidden" value="' + UTF8ToString(adaptType) + '">');
		$('#IVFields').append('<input 	name="rotation' + trialN + '" id="rotation' + trialN + '" type="hidden" value="' + rotation + '">');

		$('#DVFields').append('<input 	name="endpointX' + trialN + '" id="endpointX' + trialN + '" type="hidden" value="' + x + '">');
		$('#DVFields').append('<input 	name="endpointY' + trialN + '" id="endpointY' + trialN + '" type="hidden" value="' + y + '">');
		$('#DVFields').append('<input 	name="movementTime' + trialN + '" id="movementTime' + trialN + '" type="hidden" value="' + movementTime + '">');
		$('#DVFields').append('<input 	name="movementSpeed' + trialN + '" id="movementSpeed' + trialN + '" type="hidden" value="' + movementSpeed + '">');
		$('#mousePosFields').append('<input 	name="mousePosX' + trialN + '" id="mpX' + trialN + '" type="hidden" value="' + mouseX + '">');
		$('#mousePosFields').append('<input 	name="mousePosY' + trialN + '" id="mpY' + trialN + '" type="hidden" value="' +  mouseY + '">');
		$('#mousePosFields').append('<input 	name="time' + trialN + '" id="time' + trialN + '" type="hidden" value="' +  time + '">');
		//gameInstance.SendMessage('trialManager', 'NextTrial');
	},

	goFullScreen: function(){
		gameInstance.SetFullscreen(1);
	},

	stopFullScreen: function(){
		gameInstance.SetFullscreen(0);
		$('#submitButton').show();
	},

});