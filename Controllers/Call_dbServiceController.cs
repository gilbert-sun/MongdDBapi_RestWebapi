using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    public class Call_dbServiceController
    {
        private readonly RobotPickModelService _robotPickModelService;
        private readonly RobotLogModelServices _robotLogModelServices;
        
        
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
		
       
        //LogErrKind == LEkind
        public enum LEkind
        {
            RobotArm = 0,
            VisionSys = 1,
            ConveySys=2,
            ControSys=3,
        }
        
        public enum STATUS
        {
            Error = 0,
            Good = 1
        }
        
        public enum SINK
        {
            SinkA = 0,
            SinkB = 1,
            SinkC = 2,
            SinkD = 3,            
        }
        
        public Call_dbServiceController()
        {
            _robotPickModelService = new RobotPickModelService();
            _robotLogModelServices = new RobotLogModelServices();
        }

        
        //-------PET type(p,s,o,ot,ch,...) mongo db read:r/write:w-------------------------------------------------
        public MongoPickDBmodel wPET_PData(int conf=50)
        {
            return _robotPickModelService.Dumpdata(nameof(Btype.P),conf,"Good");
        }
        
        public MongoPickDBmodel wPET_OilData(string sinktype,int conf=50)
        {
            return _robotPickModelService.Dumpdata(nameof(Btype.OIL),conf,sinktype);
        }
        
        public MongoPickDBmodel wPET_SoyData(string sinktype,int conf=50)
        {
            return _robotPickModelService.Dumpdata(nameof(Btype.SOY),conf,sinktype);
        }
        
        public MongoPickDBmodel wPET_TrayData(string sinktype,int conf=50)
        {
            return _robotPickModelService.Dumpdata(nameof(Btype.TRAY),conf,sinktype);
        }
        
        public MongoPickDBmodel wPET_CHData(string sinktype,int conf=50)
        {
            return _robotPickModelService.Dumpdata(nameof(Btype.CH),conf,sinktype);
        }
        
        public MongoPickDBmodel wPET_ColorData(string sinktype,int conf=50)
        {
            return _robotPickModelService.Dumpdata(nameof(Btype.COLOR),conf,sinktype);
        }
        
        public MongoPickDBmodel wPET_OtherData(string sinktype,int conf=50)
        {
            return _robotPickModelService.Dumpdata(nameof(Btype.OTHER),conf,sinktype);
        }
        
        public void rPET_Data(long  timetag)
        {
            Console.WriteLine("\n---- Data response -----: Call_dbServiceController.cs ::--rPET_Data {0} !!\n", _robotPickModelService.Get(timetag));
        }
        
        public void rPET_DataMM(long  timetag)
        {
            Console.WriteLine("\n---- Data response -----: Call_dbServiceController.cs ::--rPET_Data {0} !!\n", _robotPickModelService.GetMM(timetag));
        }
        
        //-------Err log mongo db read:r/write:w-------------------------------------------------
                
        public MongoLogDBmodel wPET_RobotArmLog(string content,string status)
        {
            return _robotLogModelServices.Dumplog( content ,status);
        }

        public MongoLogDBmodel wPET_VisionLog(string content,string status)
        {
            return _robotLogModelServices.Dumplog( content ,status,nameof(LEkind.VisionSys));
        }

        public MongoLogDBmodel wPET_ConveySysLog(string content,string status)
        {
            return _robotLogModelServices.Dumplog( content ,status,nameof(LEkind.ConveySys));
        }
        
        public MongoLogDBmodel wPET_ControSysLog(string content,string status)
        {
            return _robotLogModelServices.Dumplog( content ,status,nameof(LEkind.ControSys));
        }
        
        public void rPET_Log(long timetag)
        {
            Console.WriteLine("\n---- Err Log -----: Call_dbServiceController.cs ::--rPET_Log {0} !!\n", _robotLogModelServices.Get(timetag));
        }

        public void main( )
        {
            // write/read mongoDB ==> pet type(s,o,ot,ch,p.t)
            var w1 = wPET_PData();
            rPET_Data(w1.Datetimetag);
            var w2 = wPET_OtherData(nameof(SINK.SinkA),58);
            rPET_Data(w2.Datetimetag);
            var w3 = wPET_ColorData(nameof(SINK.SinkB),63);
            rPET_Data(w3.Datetimetag);
            var w4 = wPET_ColorData(nameof(SINK.SinkC),68);
            rPET_Data(w4.Datetimetag);
            var w5 = wPET_OilData(nameof(SINK.SinkD),73);
            rPET_Data(w5.Datetimetag);
            var w6 = wPET_SoyData(nameof(SINK.SinkA),78);
            rPET_Data(w6.Datetimetag);
            var w7 = wPET_TrayData(nameof(SINK.SinkB),83);
            rPET_Data(w7.Datetimetag);

            // write/read mongoDB ==> error log(vision,robotController,converyer, robotArm)
            var e1 = wPET_VisionLog("This is Vision camera error 11 !",nameof(STATUS.Error));
            rPET_Log(e1.Datetimetag);
            var e2 = wPET_ControSysLog("This is Robot Controller error 11 !",nameof(STATUS.Error));
            rPET_Log(e2.Datetimetag);
            var e3 = wPET_ConveySysLog("This is Conveyer error 11 !",nameof(STATUS.Error));
            rPET_Log(e3.Datetimetag);
            var e4 = wPET_RobotArmLog("This is Robot Arm error 11 !",nameof(STATUS.Error));
            rPET_Log(e4.Datetimetag);
            
        }

        public void ServiceGetMM(long timetag)
        {

            Console.WriteLine("\n----  Call_dbServiceController.cs ::--Get last 1min PET amount {0} !!\n", _robotPickModelService.GetMM(timetag));

        }
        
        public List<MongoPickDBmodel> ServiceGetMM1(long timetag)
        {

            Console.WriteLine("\n----  Call_dbServiceController.cs ::--Get last 1min PET amount {0} !!\n", _robotPickModelService.GetMMc(timetag));
            return _robotPickModelService.GetMM(timetag);

        }
        
        public void ServiceGetHH(long timetag)
        {

            Console.WriteLine("\n----  Call_dbServiceController.cs ::--Get last 1hr PET amount {0} !!\n", _robotPickModelService.GetHH(timetag));

        }
        
        public List<MongoPickDBmodel> ServiceGetHH1(long timetag)
        {

            Console.WriteLine("\n----  Call_dbServiceController.cs ::--Get last 1hr PET amount {0} !!\n", _robotPickModelService.GetHHc(timetag));
            return _robotPickModelService.GetHH(timetag);
        }
        
        public void ServiceGetDD(long timetag)
        {
            Console.WriteLine("\n----  Call_dbServiceController.cs ::--Get last 8hr PET amount  {0} !!\n", _robotPickModelService.GetDD(timetag));

        }
        
        public List<MongoPickDBmodel> ServiceGetDD1(long timetag)
        {
            Console.WriteLine("\n----  Call_dbServiceController.cs ::--Get last 8hr PET amount  {0} !!\n", _robotPickModelService.GetDDc(timetag));
            return _robotPickModelService.GetDD(timetag);

        }
        
    }
}