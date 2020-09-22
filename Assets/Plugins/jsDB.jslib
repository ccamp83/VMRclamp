mergeInto(LibraryManager.library,
{
    testFunction: function()
    {
        console.log("test function");
    },

    ReadData: function (tableName, itemId)
    {
        console.log("test test");
        
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "subjID": Pointer_stringify(itemId)
            }
        };

        console.log(params);

        var awsConfig =
        {
            region: "us-east-2",
            endpoint: "https://dynamodb.us-east-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };

        AWS.config.update(awsConfig);

        var docClient = new AWS.DynamoDB.DocumentClient();

        var returnStr = "Error";

        docClient.get(params, function(err, data)
            {
                if (err)
                {
                    returnStr = "Error:" + JSON.stringify(err);
                    console.log(returnStr);
                    SendMessage('DynamoInterface', 'StringCallback', returnStr);
                }
                else
                {
                    returnStr = "Data Found:" + JSON.stringify(data.Item.NickName);
                    console.log(data.Item.NickName);
                    SendMessage('DynamoInterface', 'StringCallback', data.Item.NickName);
                }
            }
        );
    },

    WriteData: function (tableName, subjID, subjName, x, y, movementTime, movementSpeed, phase, targetPos, adaptType, rotation, trialN)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            {
                "subjID": Pointer_stringify(subjID),
                "subjName": Pointer_stringify(subjName),
                "x": Pointer_stringify(x),
                "y": Pointer_stringify(y),
                "movementTime": Pointer_stringify(movementTime),
                "movementSpeed": Pointer_stringify(movementSpeed),
                "phase": Pointer_stringify(phase),
                "targetPos": Pointer_stringify(targetPos),
                "adaptType": Pointer_stringify(adaptType),
                "rotation": Pointer_stringify(rotation),
                "trialN": Pointer_stringify(trialN)
            }
        };

        var awsConfig =
        {
            region: "us-east-2",
            endpoint: "https://dynamodb.us-east-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };

        AWS.config.update(awsConfig);
        
        var docClient = new AWS.DynamoDB.DocumentClient();
        
        var returnStr = "Error";
        
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                SendMessage('DynamoInterface', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Inserted:" + JSON.stringify(data, undefined, 2);
                SendMessage('DynamoInterface', 'StringCallback', returnStr);
            }
        });
    },

    DeleteData: function (tableName, itemId)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "subjID": Pointer_stringify(itemId)
            }
        };

        var awsConfig =
        {
            region: "us-east-2",
            endpoint: "https://dynamodb.us-east-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };

        AWS.config.update(awsConfig);
        
        var docClient = new AWS.DynamoDB.DocumentClient();
        
        var returnStr = "Error";
        
        docClient.delete(params, function(err, data)
        {
            if (err)
            {
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                SendMessage('DynamoInterface', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Deleted:" + JSON.stringify(data, undefined, 2);
                SendMessage('DynamoInterface', 'StringCallback', returnStr);
            }
        });
    },

    UpdateData: function (tableName, itemId, nickname)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "subjID": Pointer_stringify(itemId)
            },

            UpdateExpression: "set NickName = :newName",
            
            ExpressionAttributeValues:
            {
                ":newName": Pointer_stringify(nickname)
            },
            
            ReturnValues:"UPDATED_NEW"
        };

        var awsConfig =
        {
            region: "us-east-2",
            endpoint: "https://dynamodb.us-east-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };

        AWS.config.update(awsConfig);

        var docClient = new AWS.DynamoDB.DocumentClient();

        var returnStr = "error";

        docClient.update(params, function(err, data)
        {
            if (err)
            {
                returnStr = "Error: " + JSON.stringify(err , undefined, 2);
                SendMessage('DynamoInterface', 'StringCallback', returnStr);
            }
            else if (data)
            {
                returnStr = "Updated: " + JSON.stringify( data , undefined, 2);
                SendMessage('DynamoInterface', 'StringCallback', returnStr);
            }
        });
    },

});