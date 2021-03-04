mergeInto(LibraryManager.library,
{
    ReadData: function (tableName, itemId)
    {
        console.log("testing ReadData");
        
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "testID": Pointer_stringify(itemId)
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

    WriteData: function (tableName, testID, var1, var2)
    {
        console.log("testing WriteData");

        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            {
                "testID": Pointer_stringify(testID),
                "var1": Pointer_stringify(var1),
                "var2": Pointer_stringify(var2)
            }
        };

        var awsConfig =
        {
            region: "us-east-2",
            endpoint: "https://dynamodb.us-east-2.amazonaws.com",
            accessKeyId: "AKIA3OG3TLYSJQKT62OR",
            secretAccessKey: "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"
        };

        AWS.config.update(awsConfig);
        
        var docClient = new AWS.DynamoDB.DocumentClient();
        
        var returnStr = "Error";
        
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                //SendMessage('DynamoInterface', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Inserted:" + JSON.stringify(data, undefined, 2);
                //SendMessage('DynamoInterface', 'StringCallback', returnStr);
            }

            console.log(returnStr);
        });
    },

    DeleteData: function (tableName, testID)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "testID": Pointer_stringify(testID)
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

    UpdateData: function (tableName, testID, nickname)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "testID": Pointer_stringify(testID)
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