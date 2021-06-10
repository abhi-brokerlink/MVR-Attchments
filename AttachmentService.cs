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
    public class AttachmentService
    {
        static string AuthenticationKey;
        static string DataBase;
        static string ConnectionString = ConfigurationManager.ConnectionStrings["CBLReporting"].ConnectionString;
        CBLServiceReference.MessageHeader oMessageHeader;


        public AttachmentService()
        {
            AuthenticationKey = ConfigurationManager.AppSettings["AppliedSDKKey"];
            DataBase = ConfigurationManager.AppSettings["AppliedSDKDatabase"];
            oMessageHeader = new CBLServiceReference.MessageHeader();
            oMessageHeader.AuthenticationKey = AuthenticationKey;
            oMessageHeader.DatabaseName = DataBase;

        }


        public void CloseAttachment(int AttachmentID)
        {
            CBLServiceReference.EpicSDK_2017_02Client EpicSDKClient = new CBLServiceReference.EpicSDK_2017_02Client();
            CBLServiceReference.AttachmentGetResult oAttResult = new CBLServiceReference.AttachmentGetResult();
            CBLServiceReference.AttachmentFilter oAttFilter = new CBLServiceReference.AttachmentFilter();
            CBLServiceReference.Attachment att = new CBLServiceReference.Attachment();
            CBLServiceReference.AttachmentSorting oAttSorting = new CBLServiceReference.AttachmentSorting();
            oAttSorting.SortOrder = CBLServiceReference.SortOrder.Ascending;

            oAttFilter.AttachmentID = AttachmentID;
            oAttResult = EpicSDKClient.Get_Attachment(oMessageHeader, oAttFilter, oAttSorting, 0);
            att = oAttResult.Attachments[0];

            
            Console.WriteLine(att.AttachmentID + " - " + att.Description);
            Console.WriteLine("*-*-*-*");

            EpicSDKClient.Delete_Attachment(oMessageHeader, AttachmentID);


            Console.WriteLine("*-*-*");



        }





    }

    public class AttachmentVars
    {
        public int UniqAttachment { get; set; }


    }

}
