# PET robot 存取 MongoDB 之 CRUD 範例

ASP&#x3E;NET Core 3.0/3.1 存取 MongoDB 的 CRUD 範例專案。

### mongo db query cmd
![img.png](img.png)

### mongo db restful cmd
![img_1.png](img_1.png)


## 背景資訊

以簡單的 Robot-PET-MongoDBmodel 類別為資料單元做基本 CRUD 的存取示範。

* .NET MongoDB Driver 的版本相容資訊
  * 2.7 版 才支援 MongoDB 4.0。

### 專案的執行環境

* Visual Studio 2019
* .NET Core 3.0/3.1
* Mongo.Driver 2.7 or above
* MongoDB 4 (Docker)

## 安裝 MongoDB.Driver

安裝最新版，今時今日為版本 2.7.0。

* 至 [NuGet 套件管理員] 搜尋 "MongoDB.Driver" 並安裝。  

  或
* 至 [套件管理主控台] 執行
    ```
    Install-Package MongoDB.Driver
    ```
## 使用 Mongo DB Service, 以 Collection 做 Create/Read

### Create PET=='P' / PET='Other' / PET='Color' / ...
```
w1 = wPET_PData() / wPET_OtherData('confidence =' : 55) / wPET_ColorData('confidence =' : 60)
e4 = wPET_RobotArmLog("This is Robot Arm error 1 !");
```
### Read Data / Log
rPET_Data(w1.Datetimetag);
rPET_Log(e4.Datetimetag);
```
var filter = Builders<MongoDBmodel>.Filter.Eq(o => o.Id, id);
MongoDBmodel mongoDBmodel = collection.Find(filter).FirstOrDefault();
```
public void main( )
{
        /* write/read mongoDB ==> pet type(s,o,ot,ch,p.t)*/

        var w1 = wPET_PData();
        rPET_Data(w1.Datetimetag);

        var w2 = wPET_OtherData(55);
        rPET_Data(w2.Datetimetag);

        var w3 = wPET_ColorData(60);
        rPET_Data(w3.Datetimetag);

        var w4 = wPET_ColorData(75);
        rPET_Data(w4.Datetimetag);

        var w5 = wPET_OilData(70);
        rPET_Data(w5.Datetimetag);

        var w6 = wPET_SoyData(75);
        rPET_Data(w6.Datetimetag);

        var w7 = wPET_TrayData(80);
        rPET_Data(w7.Datetimetag);

        /* write/read mongoDB ==> error log(vision,robotController,converyer, robotArm) */

        var e1 = wPET_VisionLog("This is Vision camera error 1 !");
        rPET_Log(e1.Datetimetag);

        var e2 = wPET_ControSysLog("This is Robot Controller error 1 !");
        rPET_Log(e2.Datetimetag);

        var e3 = wPET_ConveySysLog("This is Conveyer error 1 !");
        rPET_Log(e3.Datetimetag);

        var e4 = wPET_RobotArmLog("This is Robot Arm error 1 !");
        rPET_Log(e4.Datetimetag);
}
### Log Result ==> http://localhost:5000/robot/log/
![img.png](img2.png)

### PET Data Result ==> http://localhost:5000/robot/arm/
![img.png](img3.png)


### Create 1 time PET Data ==> http://localhost:5000/robot/arm/
![img.png](img4.png)
```

## 建立 Model 
####(Here you don't read if you don't want to understand Mongo DB)

### DeltaRobot_model1.cs
```
public class MongoDBmodel
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("Content")]
        [JsonProperty("Content")]
        public string Content { get; set; }
        
        [BsonElement("Category")]
        public string Category { get; set; }
        
        [BsonElement("Datetimetag")]
        public long Datetimetag { get; set; }
        
        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }
}
```
* `ObjectId` 用來儲存 MongoDB 產生的專用識別碼。
* 利用 `BsonElement` 來指定欄位名稱。

## 取得 MongoDBmodel 的 Collection

### 建立 Client

建立起 `MongoClient`。  

初始化 MongoClient 的連線寫法有很多種，範例使用最基本的連線字串寫法，更多寫法可看 參考資料2。

1) 使用預設連線 (localhost:27017)
    ```
    MongoClient client = new MongoClient();
    ```
2) 連線字串
    ```
    MongoClient client = new MongoClient("mongodb://localhost:27017");
    ```
3) 密碼保護的連線字串
    ```
    MongoClient client= new MongoClient("mongodb://{username}:{password}@{host}:{port}/{Database}")
    如：  
    MongoClient client= new MongoClient("mongodb://user1:password1@127.0.0.1:27017/testdb");
    ```

### 拿到 Collection - IMongoCollection<T>

取得 Database
```
IMongoDatabase db = client.GetDatabase(DbName);
```
取得 Collection
```
IMongoCollection<MongoDBmodel> collection1S = db.GetCollection<MongoDBmodel>(CollectionName);
```

## 以 Collection 做 CRUD

### List
```
collection.Find(new BsonDocument()).ToList();
或
collection.AsQueryable().ToList();
```
### Get
id：MongoDB ObjectId
```
var filter = Builders<MongoDBmodel>.Filter.Eq(o => o.Id, id);
MongoDBmodel mongoDBmodel = collection.Find(filter).FirstOrDefault();
```
### Create
```
collection.InsertOne(mongoDBmodel);
```
### Update
```
var filter = Builders<MongoDBmodel>.Filter.Eq(o => o.Id, id);
ReplaceOneResult result = collection1S.ReplaceOne(filter, model1 => model1.Id == id, moedl1In);
```
### Delete
```
var filter = Builders<MongoDBmodel>.Filter.Eq(o => o.Id, id);
DeleteResult result = collection.DeleteOne(filter);
```

## 參考資料及圖片來源

1. [C# and .NET MongoDB Driver](https://docs.mongodb.com/ecosystem/drivers/csharp/)
2. [C# 搭配 MongoDB 的連線寫法](https://blog.yowko.com/mongodb-connectionstring/)
3. [Update all properties of object in MongoDb](https://stackoverflow.com/questions/30893012/update-all-properties-of-object-in-mongodb)
4. [Docker版MongoDB的安裝](https://www.jianshu.com/p/2181b2e27021)
