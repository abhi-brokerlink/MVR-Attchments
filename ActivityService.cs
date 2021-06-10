using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace EpicIntegrator
{
    public class ActivityService
    {
        static string AuthenticationKey;
        static string DataBase;
        static string ConnectionString = ConfigurationManager.ConnectionStrings["CBLReporting"].ConnectionString;
        CBLServiceReference.MessageHeader oMessageHeader;


        public ActivityService()
        {
            AuthenticationKey = ConfigurationManager.AppSettings["AppliedSDKKey"];
            DataBase = ConfigurationManager.AppSettings["AppliedSDKDatabase"];
            oMessageHeader = new CBLServiceReference.MessageHeader();
            oMessageHeader.AuthenticationKey = AuthenticationKey;
            oMessageHeader.DatabaseName = DataBase;
            
        }


        public void CloseActivity(int ActivityID)
        {
            CBLServiceReference.EpicSDK_2017_02Client EpicSDKClient = new CBLServiceReference.EpicSDK_2017_02Client();
            CBLServiceReference.ActivityGetResult oActResult = new CBLServiceReference.ActivityGetResult();
            CBLServiceReference.ActivityFilter oActFilter = new CBLServiceReference.ActivityFilter();
            CBLServiceReference.Activity act = new CBLServiceReference.Activity();
            CBLServiceReference.ActivityGetResult oActResult2 = new CBLServiceReference.ActivityGetResult();
            CBLServiceReference.Activity act2 = new CBLServiceReference.Activity();

            oActFilter.ActivityID = ActivityID;
            oActResult = EpicSDKClient.Get_Activity(oMessageHeader, oActFilter, 0);
            act = oActResult.Activities[0];

            //Console.WriteLine(act.Description);
            //Console.WriteLine(act.StatusOption.Value);
            //Console.WriteLine(act.CloseDetailValue.ClosedReason);
            //Console.WriteLine(act.CloseDetailValue.ClosedStatus);
            //Console.WriteLine(act.StatusOption.OptionName);


            
            if (act.DetailValue.AmountQualifier == "0")
            {
                act.DetailValue.AmountQualifier = "Dollars";
            }

            if (act.Priority == "")
            {
                act.Priority = "Normal";
            }


            int TaskCounts = act.Tasks.Count;
            for (int t=0; t<TaskCounts; t++)
            {
                if (act.Tasks[t].Status != "Completed")
                {
                    act.Tasks[t].Status = "Completed";
                    act.Tasks[t].Flag = CBLServiceReference.Flags5.Update;
                    
                }
            }

            EpicSDKClient.Update_Activity(oMessageHeader, act);

            oActResult2 = EpicSDKClient.Get_Activity(oMessageHeader, oActFilter, 0);
            act2 = oActResult2.Activities[0];

            


            act2.StatusOption.Value = 1;
            act2.StatusOption.OptionName = "Closed";
            act2.CloseDetailValue.ClosedStatus = "Unsuccessful";
            act2.CloseDetailValue.ClosedReason = "Administrative";
            EpicSDKClient.Update_Activity(oMessageHeader, act2);
            Console.WriteLine("*-*-*");



        }





    }

    public class ActivityVars
    {
        public int UniqActivity { get; set; }


    }

}
