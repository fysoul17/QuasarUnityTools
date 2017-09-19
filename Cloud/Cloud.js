var dreamlo_URL = 'http://dreamlo.com/lb/';
var dreamlo_publicKey = '59af8f316b2b65e3a834faf6';
var dreamlo_privateKey = 'FI0awyPwWkuvF3XdHKprUw4DRuRVHnLUyMk8dBjZVT7A';

// Get Server Time.
handlers.GetServerTime = function (args) {
    return JSON.stringify(new Date());
};

handlers.RoomCreated = function (args) {
    server.WriteTitleEvent({
    	EventName : "room_created"
    });
};

handlers.RoomJoined = function (args) {
    server.WriteTitleEvent({
    	EventName : "room_joined"
    });
};

handlers.RoomLeft = function (args) {
    server.WriteTitleEvent({
    	EventName : "room_left"
    });
};

handlers.RoomClosed = function (args) {
    server.WriteTitleEvent({
    	EventName : "room_closed"
    });
};

handlers.RoomPropertyUpdated = function (args) {
    server.WriteTitleEvent({
    	EventName : "room_property_changed"
    });
};

handlers.RoomEventRaised = function (args) {
    server.WriteTitleEvent({
    	EventName : "room_event_raised"
    });
};

// Character selected character.
//
// Expected Params
// - args.ID		// Unique ID of seletable characters.
handlers.ChangeCharacter = function (args) 
{
	var dataToUpdate = {};
	dataToUpdate["SelectedCharacterId"] = args.ID;
	
	server.UpdateUserReadOnlyData({"PlayFabId" : currentPlayerId, "Data" : dataToUpdate, "Permission":"Public" });
	
	return server.GetUserReadOnlyData({"PlayFabId" : currentPlayerId, "Keys" : ["SelectedCharacterId"]});
};

// Update infomation on classic game start.
//
// Expected Params
// - args.Mode		// Entering price for the game.
handlers.ClassicGameStarted = function (args) 
{
	// Subtract gold first.
	var response = server.SubtractUserVirtualCurrency({ PlayFabId: currentPlayerId, VirtualCurrency: "GD", Amount: args.Mode });
	
	// Update statistics for leaderboard.
	UpdateStatistics("TotalGolds", response.Balance);
	
	return response.Balance;
};

// Update Video Ads Timer.
//
// Expected Params
// - args.AdsID			// ID of Ads.
// - args.Ticks			// Available time in ticks.	(long type stringyfied)
handlers.UpdateVideoAdsTimer = function (args) 
{
	var timerDataDictionary = {};

	var timeData = server.GetUserReadOnlyData({"PlayFabId" : currentPlayerId, "Keys" : ["VideoAdsTimer"]});
	if(!isObjectEmpty(timeData) && !isObjectEmpty(timeData.Data["VideoAdsTimer"]))
	{
		timerDataDictionary = JSON.parse(timeData.Data["VideoAdsTimer"].Value);
	}
	
    timerDataDictionary[args.AdsID] = args.Ticks;

	var dataToUpdate = {};
	dataToUpdate["VideoAdsTimer"] = JSON.stringify(timerDataDictionary);
	server.UpdateUserReadOnlyData({"PlayFabId" : currentPlayerId, "Data" : dataToUpdate, "Permission":"Public" });
};

// Update infomation on classic game end.
//
// Expected Params
// - args.Mode			// Entering price for the game.
// - args.Opponent  	// Opponent
handlers.ClassicGameWon = function (args) 
{
	var response = server.AddUserVirtualCurrency({ PlayFabId: currentPlayerId, VirtualCurrency: "GD", Amount: args.Mode });
	
	// Update statistics for leaderboard.
	var statisticsToUpdate = [];
	statisticsToUpdate.push({ "StatisticName": "TotalGolds", "Value": response.Balance });
	statisticsToUpdate.push({ "StatisticName": "ClassicWins", "Value": 1 });
	server.UpdatePlayerStatistics({ PlayFabId: currentPlayerId, Statistics: statisticsToUpdate });

	// Update statistics for opponent.
	UpdateStatisticsWithId(args.Opponent, "ClassicLoses", 1);

	return response.Balance;
};

// Update leaderboard for country ranks.
//
// Expected Params
// - args.CountryCode 	// Country code.
handlers.UpdateClassicGameCountryLeaderboard = function (args) {
    var score = 1;

    // Get country score.
    var response = http.request(dreamlo_URL + dreamlo_publicKey + '/pipe-get/' + args.CountryCode, 'get', '', 'text/plain', null, true);

    // code|1|0||9/6/2017 6:29:51 AM|0
    var currentData = response.split("|");
    if (currentData[1] != null) {
        score = parseInt(currentData[1]);
        score += 1;
    }

    var updateResponse = http.request(dreamlo_URL + dreamlo_privateKey + '/add/' + args.CountryCode + "/" + score, 'get', '', 'text/plain', null, true);
};

// Fetch leaderboard for country ranks.
handlers.GetClassicGameCountryLeaderboard = function (args) {
    return http.request(dreamlo_URL + dreamlo_publicKey + '/json', 'get', '', 'text/plain', null, true);
};

// Update game result (Every game in match).
//
// Expected Params
// - args.Result	// 0: Win, 1: Lose, 2: Draw.
// - args.Decision	// 0: Rock, 1: Paper, 2: Scissors.
handlers.UpdateEachGameResults = function(args) 
{
	UpdateEachGameStatistics(args);
	UpdateRecentEachGameHistory(args);
};

var resultKeys = [ "Wins", "Loses", "Draws" ];
var decisionKeys = [ "Rock", "Paper", "Scissors" ];
function UpdateEachGameStatistics(args) 
{
	var result = resultKeys[args.Result];
	var decision = decisionKeys[args.Decision];
	var key = decision + result;
	
	UpdateStatistics(key, 1);

	server.UpdatePlayerStatistics({ PlayFabId: currentPlayerId, Statistics: statisticsToUpdate });
}

function UpdateStatistics(key, amount) 
{
	var statisticsToUpdate = [];
	statisticsToUpdate.push({ "StatisticName": key, "Value": amount });
	server.UpdatePlayerStatistics({ PlayFabId: currentPlayerId, Statistics: statisticsToUpdate });
}

function UpdateStatisticsWithId(id, key, amount) 
{
	var statisticsToUpdate = [];
	statisticsToUpdate.push({ "StatisticName": key, "Value": amount });
	server.UpdatePlayerStatistics({ PlayFabId: id, Statistics: statisticsToUpdate });
}

function UpdateRecentEachGameHistory(args) 
{
	var resultHistoryDataList = [];
	
	var resultHistoryData = server.GetUserReadOnlyData({"PlayFabId" : currentPlayerId, "Keys" : ["ResultHistory"]});
	if(!isObjectEmpty(resultHistoryData) && !isObjectEmpty(resultHistoryData.Data["ResultHistory"]))
	{
		resultHistoryDataList = JSON.parse(resultHistoryData.Data["ResultHistory"].Value);
	}
	
	var currentResult = {};
	currentResult["Result"] = args.Result;
	currentResult["By"] = args.Decision;
	
	var numberOfResults = resultHistoryDataList.push(currentResult);
	if (numberOfResults > 10) 
	{
		resultHistoryDataList.splice(0, numberOfResults - 10);
	}
	
	var dataToUpdate = {};
	dataToUpdate["ResultHistory"] = JSON.stringify(resultHistoryDataList);
	
	server.UpdateUserReadOnlyData({"PlayFabId" : currentPlayerId, "Data" : dataToUpdate, "Permission":"Public" });
}

// Checks to see if an object has any properties
// Returns true for empty objects and false for non-empty objects
function isObjectEmpty(obj)
{
	if(typeof obj === 'undefined')
	{
		return true;
	}

	if(Object.getOwnPropertyNames(obj).length === 0)
	{
		return true;
	}
	else
	{
		return false;
	}
}