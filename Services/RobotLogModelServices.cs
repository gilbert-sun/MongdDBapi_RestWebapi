using WebApplication3.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class RobotLogModelServices
    {
        private readonly IMongoCollection<MongoLogDBmodel> collection2S;


        //LogErrKind == LEkind
        public enum LEkind
        {
            RobotArm = 0,
            VisionSys = 1,
            ConveySys=2,
            ControSys=3,
        }
        
        public RobotLogModelServices()
        {
            var client = new MongoClient("mongodb://localhost:27017");//settings.ConnectionString);
            var database = client.GetDatabase("mongoDBrobot1");//settings.DatabaseName);
            collection2S = database.GetCollection<MongoLogDBmodel>("robot1logdb");//settings.DeltaRobotCollectionName);
            Console.WriteLine("\n--------Hi---------: DeltaRobotLogModelServices-Log-DbServices.cs : !!\n");//" 1:{0}, 2:{1}, 3:{2}", settings.DatabaseName, settings.ConnectionString,settings.DeltaRobotCollectionName);
        }
        
        //C-R-U-D DB
        //C:Create PET DB data
        public MongoLogDBmodel Create(MongoLogDBmodel mongoMongoLogDBmodel)
        {
            collection2S.InsertOne(mongoMongoLogDBmodel);
            return mongoMongoLogDBmodel;
        }
        
        //C:Create Err LOG DB data
        public MongoLogDBmodel Dumplog(string content, string status,string btType=nameof(LEkind.RobotArm))
        {
            MongoLogDBmodel mongoMongoLogDBmodel = new MongoLogDBmodel
            {
                Content = content,
                Category = btType, 
                Status = status,
                Datetimetag = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Timestamp = DateTime.Now.ToLocalTime(),
            };
            collection2S.InsertOne(mongoMongoLogDBmodel);
            return mongoMongoLogDBmodel;
        }  
        
        //R:Read
        public List<MongoLogDBmodel> Get()
        {
            return collection2S.Find( model1=> true).ToList(); 
        }
        
        //R:Read I
        public MongoLogDBmodel Get(long timetag)
        {
            Console.WriteLine("\n--------: DeltaRobotLogModelServices.cs ::--GET--ID !!\n");
            var model1 = Builders<MongoLogDBmodel>.Filter.Eq("Datetimetag", timetag);

            if (model1 == null)
            {
                return null;
            }
            else
            {
                return collection2S.Find(x => ( (x.Datetimetag > (timetag-1000)))).FirstOrDefault();
            }
        }
        
        //U
        public void Update(string id, MongoLogDBmodel moedl1In) =>
            collection2S.ReplaceOne(model1 => model1.Id == id, moedl1In);
        
        //D
        public void Remove(MongoLogDBmodel moedl1In) =>
            collection2S.DeleteOne(model1 => model1.Id == moedl1In.Id); 
        //D
        public void Remove(string id) =>
            collection2S.DeleteOne(model1 => model1.Id == id); 
    }
}