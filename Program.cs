using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EpicIntegrator
{
    class Program
    {

        static void Main(string[] args)
        {
            CloseAttachments();
        }
          
        static void CloseAttachments()
        {
            List<int> AttachmentCloseList = new List<int>();

            string AttClosePath = @"C:\Users\AbhishekChhibber\OneDrive - BrokerLink\Documents\MVR Attchments\"; //Final Check
            string AttCloseFilePath = AttClosePath + "AttachmentsToClose.csv";
            string ErrorFilePath = @"C:\Users\AbhishekChhibber\OneDrive - BrokerLink\Documents\MVR Attchments\AttCloseErrorLog_";
            string ErrorList = "";
            var Attreader = new StreamReader(File.OpenRead(AttCloseFilePath));

            while (!Attreader.EndOfStream)
            {
                var line = Attreader.ReadLine();
                var values = line.Split(';');

                AttachmentCloseList.Add(int.Parse(values[0]));
            }

            int AttCount = AttachmentCloseList.Count; 
            EpicIntegrator.AttachmentService ats = new EpicIntegrator.AttachmentService();

            var AttCloseStart = DateTime.Now;
            Console.WriteLine("Att-Close Started: " + AttCloseStart);
            int AttCloseCounter = 0;
            foreach (int att in AttachmentCloseList)
            {
                try
                {
                    AttCloseCounter++;
                    Console.WriteLine(AttCloseCounter + " of " + AttCount + " - " + att);
                    ats.CloseAttachment(att);

                    
                    
                }
                catch (Exception e)
                {
                    string err = "#Failed| " + att + "| "+e;
                    ErrorList = ErrorList + err + System.Environment.NewLine; 
                    Console.WriteLine(err);
                }
            }
            var AttCloseEnd = DateTime.Now;
            
            if (ErrorList.Length > 1)
            {
                string ErrorPathFull = ErrorFilePath + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                string[] ErrorList2 = ErrorList.Split('\n');
                File.WriteAllLines(ErrorPathFull, ErrorList2);
            }

            Console.WriteLine("Act-Close Ended: " + AttCloseEnd);







            Console.ReadKey();
        }
    }
}

