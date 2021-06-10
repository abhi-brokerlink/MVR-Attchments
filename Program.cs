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
            CloseActivities();
        }
          
        static void CloseActivities()
        {
            List<int> ActivitiesCloseList = new List<int>();

            string ActClosePath = @"C:\Users\Abhishek\Documents\PR388\"; //Final Check
            string ActCloseFilePath = ActClosePath + "ActivitiesToClose.csv";
            string ErrorFilePath = @"C:\Users\Abhishek\Documents\PR388\ActCloseErrorLog_";
            string ErrorList = "";
            var Actreader = new StreamReader(File.OpenRead(ActCloseFilePath));

            while (!Actreader.EndOfStream)
            {
                var line = Actreader.ReadLine();
                var values = line.Split(';');

                ActivitiesCloseList.Add(int.Parse(values[0]));
            }

            int ActCount = ActivitiesCloseList.Count; 
            EpicIntegrator.ActivityService acs = new EpicIntegrator.ActivityService();

            var ActCloseStart = DateTime.Now;
            Console.WriteLine("Act-Close Started: " + ActCloseStart);
            int ActCloseCounter = 0;
            foreach (int act in ActivitiesCloseList)
            {
                try
                {
                    ActCloseCounter++;
                    Console.WriteLine(ActCloseCounter + " of " + ActCount + " - " + act);
                    acs.CloseActivity(act);

                    
                    
                }
                catch (Exception e)
                {
                    string err = "#Failed| " + act + "| "+e;
                    ErrorList = ErrorList + err + System.Environment.NewLine; 
                    Console.WriteLine(err);
                }
            }
            var ActCloseEnd = DateTime.Now;
            
            if (ErrorList.Length > 1)
            {
                string ErrorPathFull = ErrorFilePath + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                string[] ErrorList2 = ErrorList.Split('\n');
                File.WriteAllLines(ErrorPathFull, ErrorList2);
            }

            Console.WriteLine("Act-Close Ended: " + ActCloseEnd);







            Console.ReadKey();
        }
    }
}

