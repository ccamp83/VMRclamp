/* ------- INITIALIZE VARIABLES ------- */

	/* experiment design */
	var groups = ["jointUnrevealed", "jointRevealed"];
	var system = ["1DF","2DF","3DF","4DF"];
	var systemNum = [0,1,2,3]; 															// needed?
	var motionPathNum = [0,1,2,3]; 														// needed?
	var motionPath = ["1MP","2MP","3MP","4MP"];
	var allTrials = arraysConcat(system, motionPath);
	var direction = ["down","left","up","right"];
	var repetitions = 3;
	var trialsSeq = shuffle(arrayRepeat(allTrials, repetitions));
	var subjVideoIndex = 0; // video system is always TLA 							// needed?
	var subjImageIndex = 99;															// needed?
	var subjVideoCond = "empty";														// needed?
	var subjImageCond = "empty";														// needed?
	var group = "empty";																// needed?
	var currentSystem = "empty";
	var currentMotionPath = "empty";
	var Xpos = 0, Ypos = 0;
	var Xarray = [], Yarray = [];
	var timeArray = [];

	var pageNum = 0;
	var trialN = 0;
	var rotationAngle = 0;
	var rotationIndex = 0;
	var startTrialTime = 0;
	var maxTrials = trialsSeq.length;
	var Nsec = 5;
	var totalProgress = 0;

	/* Unity game instance */
	var gameInstance = UnityLoader.instantiate("gameContainer", "https://visuomotorrotation.s3.amazonaws.com/clamp/Unity+Build/Build/WebGL.json", {onProgress: UnityProgress});

	/* videos and images arrays */
	
	var videoDemo = ["https://res.cloudinary.com/ccamp83/video/upload/v1581790984/Intuitive%20Physics/SquaredBackground/EXP3/VIDEO/3DF.mp4",
					 "https://res.cloudinary.com/ccamp83/video/upload/v1581790984/Intuitive%20Physics/SquaredBackground/EXP3/VIDEO/3DFjr.mp4"];

	var videosRoot = ["https://res.cloudinary.com/ccamp83/video/upload/v1581790984/Intuitive%20Physics/SquaredBackground/EXP3/VIDEO/"];
	var imagesRoot = ["https://res.cloudinary.com/ccamp83/image/upload/v1581790955/Intuitive%20Physics/SquaredBackground/EXP3/IMG/"];

	/* ------- FUNCTIONS DECLARATION ------- */

	function initConditions() {
		// randomly assign to either jointUnrevealed or jointRevealed group
		var index = Math.floor(Math.random()*groups.length);
		group = groups[index];

		/* populate variable fields */
		for(var i = 1; i < maxTrials; i++) {
			$('#DVFields').append('<input 	name="response' + i + '" id="responseKey' + i + '" type="hidden" value="0">');
			$('#DVFields').append('<input 	name="RT' + i + '" id="RTKey' + i + '" type="hidden" value="0">');
			$('#IVFields').append('<input 	name="system' + i + '" id="system' + i + '" type="hidden" value="0">');
			$('#IVFields').append('<input 	name="motionPath' + i + '" id="motionPath' + i + '" type="hidden" value="0">');
			$('#IVFields').append('<input 	name="direction' + i + '" id="direction' + i + '" type="hidden" value="0">');
			$('#mousePosFields').append('<input 	name="mousePosX' + i + '" id="mpX' + i + '" type="hidden" value="0">');
			$('#mousePosFields').append('<input 	name="mousePosY' + i + '" id="mpY' + i + '" type="hidden" value="0">');
			$('#mousePosFields').append('<input 	name="time' + i + '" id="time' + i + '" type="hidden" value="0">');
		}

	}

	// get a new rotation
	function setRotation() {
		rotationIndex = Math.floor(Math.random()*4);
		rotationAngle = 90*rotationIndex;
		$('.rotate').rotate(rotationAngle);
	}

	function initIntroMedia() {
		/* cache videos sources */
		var intro_mp4 = document.getElementById("intro_mp4");
		intro_mp4.src = videoDemo[0];

		var intro_N_mp4 = document.getElementById("intro_N_mp4");
		intro_N_mp4.src = videoDemo[0];

		/* load intro video */
		//setRotation();
	  	var intro_video = document.getElementById("intro_video");
	  	intro_video.load();

	  	intro_video.onended = function( ){
	  		setTimeout(function() {
	  			intro_video.load();

	  			$('#advanceButton_intro1').show();
	  			
	  		}, 1000);
	  	}

	  	/* cache intro image source */
	  	var intro_where_ball_go = document.getElementById("intro_where_ball_go");
	  	var intro_image_N = document.getElementById("intro_image_N");

	  	var randomIntroImgIndex = Math.floor(Math.random()*motionPathNum.length);
	  	intro_where_ball_go.src = imagesRoot+"3DF"+(motionPathNum[randomIntroImgIndex]+1).toString()+"MP.png";
	  	intro_image_N.src = imagesRoot+"3DF"+(motionPathNum[randomIntroImgIndex]+1).toString()+"MP.png";
	}

	function acceptAndContinue() {
		$('#consentForm').hide();
		setTimeout(function(){
			pageNum++;
			$('#instructions_'+pageNum).show();
		}, 500);
	}

	function continueInstructions() {

		// hide current page
        $('#instructions_'+pageNum).hide();
  	    // increase page counter
	    pageNum++;
		// show next page
		$('#instructions_'+pageNum).show();

		switch(pageNum) {

			case 2:
				$('#advanceButton_intro2').hide();
				setTimeout(function() {
					$('#advanceButton_intro2').show();
				}, 3000);
				break;

			case 3:
				/* fetch gameContainer and append */
				var game = document.getElementById("gameContainer3DF");
				if(game.style.display == "none")
					game.style.display = "block";
				document.getElementById("Unity_intro1").prepend(game);
				break;

			case 4:
				resetUnity(gameInstance);
				/* fetch gameContainer and append */
				var game = document.getElementById("gameContainer3DF");
				if(game.style.display == "none")
					game.style.display = "block";
				document.getElementById("Unity_intro2").prepend(game);

				/* let play second intro game for Nsec seconds */
				/* then hide game and show video */
				setTimeout(function() {
	  				$('#Unity_intro2').hide();
					$('#intro_video_N').show();
	  			}, Nsec*1000);

				/* load and play video */
				var intro_video_N = document.getElementById("intro_video_N");
				intro_video_N.load();

				/* at the end of video */
				intro_video_N.onended = function(){ 
					/* hide video field */
					intro_video_N.style.display = "none";
					/* show image field */
					intro_image_N.style.display = "block";
					/* show intro instructions */
					$('#intro_instructions').show();
					$('#intro_final').show();
	  		    }
			    break;
		}
	}

	/* start experiment */
	function startExperiment() {

		// hide instructions
		$('#instructions_'+pageNum).hide();

		// load first trial after 500 milliseconds
		setTimeout(
			function(){

				/* show trial box */
				$('#box').show();
				/* show trial counter */
				$('#instrucBox').show();
				
				loadTrial();
			}
			,500);
	}
	
	/* load parameters either at random or from a csv file */
	function loadTrial() {

		if(trialN < maxTrials) {

			/* initialize system and motion path specs */
			currentSystem = trialsSeq[trialN].substring(0,3);
			currentMotionPath = trialsSeq[trialN].substring(3,7);

			/* initialize mouse position arrays */
			Xarray = [];
			Yarray = [];

			/* initialize time */
			timeArray = [];

			/* start recording time */
			startTrialTime = new Date();

			/* update trial counter */
			$('#trialNum').text('Trial ' + (trialN+1) +
							' of ' + maxTrials);

			manageStimuli();

		} else {
			/* If no trials left, show end of experiment information: */
			$('#done').show();
			$('#box').hide();
			$('#instrucBox').hide();
			$('#submitButton').show();
		}
	}

	/* this loads videos and images */
	function manageStimuli() { 

		$('#Unity').show();
		/* fetch gameContainer and append */
		var gameContainerName = "gameContainer"+currentSystem;
		eval("resetUnity(gameInstance" + currentSystem + ");");
		var game = document.getElementById(gameContainerName);
		game.style.display = "block";
		document.getElementById("Unity").prepend(game);

		/* listen to mouse position */
		game.addEventListener('mousemove', onMousemove, false);

		/* let play Unity game for Nsec seconds */
		/* then hide game and show video */
		setTimeout(function() {
			resetUnity(gameInstance);
			$('#Unity').hide();
			game.style.display = "none";
			$("#first_stimulus").show();
			}, Nsec*1000);

		/*---- select and play video ----*/
		var mp4 = document.getElementById("mp4");
		mp4.src = videosRoot+currentSystem+".mp4";
		/* load video */
		//setRotation();
	  	var first_stimulus = document.getElementById("first_stimulus");
	  	first_stimulus.load();

	  	/*---- select and load image after video ----*/
	  	var image = document.getElementById("second_stimulus");

		/* even listener triggering stuff at the end of the video */
		first_stimulus.onended = function(){ 
			/* hide video field */
			first_stimulus.style.display = "none";
			/* update image source */
		  	image.src = imagesRoot+trialsSeq[trialN]+".png";
			/* show image field */
			image.style.display = "block";
			/* show answers instructions */
			$('#answersInstr').show();

			handleResponse();

	  	};
	}; 

	function endTrial(_response) {
		/* hide image */
		$("#second_stimulus").hide();
		// hide answers instructions
		$('#answersInstr').hide();

		/* get RT */
		var endTrialTime = new Date();
		var RT = endTrialTime - startTrialTime;

		/* update hidden input */
		recordResponse(_response, RT);

		trialN += 1;

		/* brief pause to clear screen */
		setTimeout(function(){
			loadTrial();
		}, 500);
		
	}

	function handleResponse() {
		
		/* listen to keypress */
		$(document).bind("keydown.cc", function(event) {

			/* store keypress */
			var responseA = [65, 97];
			var responseW = [87, 119];

			/* move on to the next trial only if keypress is vvalid (either A or W) */
			if (responseA.includes(event.which) || responseW.includes(event.which)) {
				
				/* stop listening to the keyboard */
				$(document).unbind("keydown.cc");

				endTrial(String.fromCharCode(event.which));
			}
		});
	}

	function recordResponse(_response, _RT) {
		$("#responseKey"+trialN).val(_response);
		$("#RTKey"+trialN).val(_RT);
		// add IVs
		$("#system"+trialN).val(currentSystem);
		$('#motionPath'+trialN).val(currentMotionPath);
		$('#direction'+trialN).val(direction[rotationIndex]);
		$("#mpX"+trialN).val(JSON.stringify(Xarray));
		$("#mpY"+trialN).val(JSON.stringify(Yarray));
		$('#time'+trialN).val(JSON.stringify(timeArray));
	}

	/* repeat each element of an array N times */
	function arrayRepeat(_array, times) {

		var numOfItems = _array.length;
		var output = [];

		for(var i = 0; i < numOfItems; i++) {

			for(var j = 0; j < times; j++) {
				output.push(_array[i]);
			}
			
		}

		return output;
	}

	/* concatenate each element of two arrays */
	function arraysConcat(_array1, _array2) {

		var numOfItems1 = _array1.length;
		var numOfItems2 = _array2.length;
		var output = [];

		for(var i = 0; i < numOfItems1; i++) {

			for(var j = 0; j < numOfItems2; j++) {
				output.push(_array1[i].concat(_array2[j]));
			}
			
		}

		return output;
	}

	/* Fisher-Yates shuffle */
	function shuffle(o){
		for(var j, x, i = o.length; i; j = Math.floor(Math.random() * i), x = o[--i], o[i] = o[j], o[j] = x);
		return o;
	}

	/* Mouse position within a div */
	function onMousemove(e){
	    var m_posx = 0, m_posy = 0, e_posx = 0, e_posy = 0,
	        obj = this;
	    //get mouse position on document crossbrowser
	    if (!e){e = window.event;}
	    if (e.pageX || e.pageY){
	        m_posx = e.pageX;
	        m_posy = e.pageY;
	    } else if (e.clientX || e.clientY){
	        m_posx = e.clientX + document.body.scrollLeft
	                 + document.documentElement.scrollLeft;
	        m_posy = e.clientY + document.body.scrollTop
	                 + document.documentElement.scrollTop;
	    }
	    //get parent element position in document
	    if (obj.offsetParent){
	        do{ 
	            e_posx += obj.offsetLeft;
	            e_posy += obj.offsetTop;
	        } while (obj = obj.offsetParent);
	    }
	    // mouse position minus elm position is mouseposition relative to element:
	    var Xrel = m_posx-e_posx;
	    var Yrel = 360-(m_posy-e_posy);
	    // scale mousepos to game coordinates
	    var Xscaled = Xrel/360*20;
	    var Yscaled = Yrel/360*20;
	    // mouseposition rounded to the 2nd decimal
	    Xpos = Math.round(Xscaled * 100) / 100;
	    Ypos = Math.round(Yscaled * 100) / 100;
	    Xarray.push(Xpos);
	    Yarray.push(Ypos);
	    // print mousepos(only for debug)
	    //dbg.innerHTML = ' X Position: ' + Xpos 
	    //              + ' Y Position: ' + Ypos;
	    // record time
	    currentTime = new Date();
	    timeArray.push(currentTime - startTrialTime);
	}

	// terminate a Unity game (not used)
	function quitUnity(_gameInstance){
		_gameInstance.SendMessage('Controller', 'quitGame');
		delete _gameInstance;
	}

	// reset Unity game
	function resetUnity(_gameInstance){
		_gameInstance.SendMessage('Controller', 'resetGame');
		delete _gameInstance;
	}

	/* ------- MAIN ------- */

	// start the experiment by clicking to the startExperiment link
	$('#startExperiment').click(startExperiment);