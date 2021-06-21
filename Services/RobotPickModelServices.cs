using WebApplication3.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class RobotPickModelService
    {
        private readonly IMongoCollection<MongoPickDBmodel> collection1S;

        
        //BottleType == Btype
        public enum Btype
        {
            P = 0,
            COLOR = 1,
            SOY=2,
            OIL=3,
            TRAY=4,
            CH=5,
            OTHER=6,
        }
		
        //BottleKind == Bkind
        public enum Bkind
        {
            PET = 0,
            PP = 1,
            PS=2,
            PLA=3,
            PC=4,
            PVC=5,
        }
        
        public enum PastDay
        {
            MM = 0,
            HH = 1,
            DD = 2,
        }

        public RobotPickModelService()
        {
            var client = new MongoClient("mongodb://localhost:27017");//settings.ConnectionString);
            var database = client.GetDatabase("mongoDBrobot1");//settings.DatabaseName);
            collection1S = database.GetCollection<MongoPickDBmodel>("robot1db");//settings.DeltaRobotCollectionName);
            Console.WriteLine("\n--------Hi---------: DeltaRobotModelService.cs : !!\n");//" 1:{0}, 2:{1}, 3:{2}", settings.DatabaseName, settings.ConnectionString,settings.DeltaRobotCollectionName);
            // Console.WriteLine("\n--------Hi1---------: DeltaRobotModelService.cs :  1:{0}, 2:{1}, 3:{2}", settings.DatabaseName, settings.ConnectionString,settings.DeltaRobotCollectionName);
        }
        
        //C-R-U-D DB
        //C:Create PET DB data
        public MongoPickDBmodel Create(MongoPickDBmodel mongoPickDBmodel)
        {
            collection1S.InsertOne(mongoPickDBmodel);
            return mongoPickDBmodel;
        }
        
        //C:Create PET DB data II
        public MongoPickDBmodel Dumpdata(string btName, int conf,string sinktype ,string btType=nameof(Bkind.PET))
        {
            MongoPickDBmodel mongodbmodel = new MongoPickDBmodel
            {
                BottleName = btName,
                Category = btType,
                Confidence = conf,
                Sink = sinktype,
                Datetimetag = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Timestamp = DateTime.Now.ToLocalTime(),
            };
            collection1S.InsertOne(mongodbmodel);
            return mongodbmodel;
        }
        
        //R
        public List<MongoPickDBmodel> Get()
        {
            return collection1S.Find(model1 => true).ToList();
        }
        //R:Read I
        public MongoPickDBmodel Get(long timetag)
        {
            Console.WriteLine("\n--------: DeltaRobotModelService.cs ::--GET--ID:{0} !!\n",timetag );
            var model1 = Builders<MongoPickDBmodel>.Filter.Lte("Datetimetag", timetag-1000);

            if (model1 != null)
            {
                return collection1S.Find(x => ( (x.Datetimetag > (timetag-1000)))).FirstOrDefault();            
                // var result = _collection1s.Find(model1).FirstOrDefault();
                // return _collection1s.Find(bmodel => bmodel.Datetimetag == timetag).FirstOrDefault();
                // return _collection1s.Find(model1 => model1.Id == id).FirstOrDefault();
            }
            
            return null;
        }
        
        //R:Read II ==> DD:HH:MM
        public List<MongoPickDBmodel> GetMM(long timetag)
        {
            //----Calculate past pet total pick amount
            //1). fMM:timetag < now - 1 minute (60 * 1000) 
            
            var ff1 = Builders<MongoPickDBmodel>.Filter.Gt(x => x.Datetimetag, timetag -60000);
            var ff2 = Builders<MongoPickDBmodel>.Filter.Lte(x => x.Datetimetag, timetag );
            
            // Console.WriteLine("\n--------: DeltaRobotModelService.cs ::--GET--PastMM :{0}:{1}:{2}!!\n",timetag,collection1S.Find(ff1 & ff2 ).CountDocuments(),collection1S.Find(ff2 ).CountDocuments());
            return collection1S.Find(ff1 &ff2 ).ToList();
        }
        
        public long GetMMc(long timetag)
        {
            //----Calculate past pet total pick amount
            //1). fMM:timetag < now - 1 minute (60 * 1000) 
            
            var ff1 = Builders<MongoPickDBmodel>.Filter.Gt(x => x.Datetimetag, timetag -60000);
            var ff2 = Builders<MongoPickDBmodel>.Filter.Lte(x => x.Datetimetag, timetag );
            
            // Console.WriteLine("\n--------: DeltaRobotModelService.cs ::--GET--PastMM :{0}:{1}:{2}!!\n",timetag,collection1S.Find(ff1 & ff2 ).CountDocuments(),collection1S.Find(ff2 ).CountDocuments());
            return collection1S.Find(ff1 &ff2 ).CountDocuments();
        }
        
        public List<MongoPickDBmodel> GetHH(long timetag)
        {
            //----Calculate past pet total pick amount
            //2). fHH:timetag < now - 1 hour (60 * 60 * 1000)
            
            var ff1 = Builders<MongoPickDBmodel>.Filter.Gt(x => x.Datetimetag, timetag  - 3600000);
            var ff2 = Builders<MongoPickDBmodel>.Filter.Lte(x => x.Datetimetag, timetag );
            
            Console.WriteLine("\n--------: DeltaRobotModelService.cs ::--GET--PastHH !!\n");
            return collection1S.Find(ff1&ff2).ToList();
        }
        
        public long GetHHc(long timetag)
        {
            //----Calculate past pet total pick amount
            //2). fHH:timetag < now - 1 hour (60 * 60 * 1000)
            
            var ff1 = Builders<MongoPickDBmodel>.Filter.Gt(x => x.Datetimetag, timetag - 3600000);
            var ff2 = Builders<MongoPickDBmodel>.Filter.Lte(x => x.Datetimetag, timetag );
            
            Console.WriteLine("\n--------: DeltaRobotModelService.cs ::--GET--PastHH !!\n");
            return collection1S.Find(ff1&ff2).CountDocuments();
        }
        public List<MongoPickDBmodel> GetDD(long timetag)
        {
            //----Calculate past pet total pick amount
            //3). fDD:timetag < now - 8 hour (8 * 60 * 60 * 1000)
            
            var ff1 = Builders<MongoPickDBmodel>.Filter.Gt(x => x.Datetimetag, timetag -28800000);
            var ff2 = Builders<MongoPickDBmodel>.Filter.Lte(x => x.Datetimetag, timetag);
            
            Console.WriteLine("\n--------: DeltaRobotModelService.cs ::--GET--PastDD !!\n");
            return collection1S.Find(ff1&ff2).ToList();
        }
        
        public long GetDDc(long timetag)
        {
            //----Calculate past pet total pick amount
            //3). fDD:timetag < now - 8 hour (8 * 60 * 60 * 1000)
            
            var ff1 = Builders<MongoPickDBmodel>.Filter.Gt(x => x.Datetimetag, timetag -28800000);
            var ff2 = Builders<MongoPickDBmodel>.Filter.Lte(x => x.Datetimetag, timetag);
            
            Console.WriteLine("\n--------: DeltaRobotModelService.cs ::--GET--PastDD !!\n");
            return collection1S.Find(ff1&ff2).CountDocuments();
        }
        
        //U
        public void Update(string id, MongoPickDBmodel moedl1In) =>
            collection1S.ReplaceOne(model1 => model1.Id == id, moedl1In);
        //D
        public void Remove(MongoPickDBmodel moedl1In) =>
            collection1S.DeleteOne(model1 => model1.Id == moedl1In.Id); 
        //D
        public void Remove(long timetag) =>
            collection1S.DeleteOne(model1 => model1.Datetimetag == timetag); 

        
    } // End of Class::DeltaRobotModelService
    

}