using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationFromTrasen
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start = new DateTime(2016, 8, 30);
            DateTime end = new DateTime(2016, 10, 1);

            if (false)
            {
                var target = new TranslationFromTrasen.Main();

                //已完成从2016-07-01到2016-10-01
                target.GetPatientAndInPatient(start, end, true, false, true);
            }

            if (false)
            {
                for (var time = start; time < end; time = time.AddDays(1))
                {
                    var tempStart = time;
                    var tempEmd = time.AddDays(1);

                    Console.WriteLine(tempStart.ToLongDateString() + " - " + tempEmd.ToLongDateString() + "Startted");

                    var target = new TranslationFromTrasen.Main();

                    //已完成从2016-07-01到2016-10-01（后界开区间）
                    //target.GetDrugRecord(tempStart, tempEmd, false, false);

                    //已完成从2016-07-01到2016-10-01（后界开区间）
                    //target.GetDrugFee(tempStart, tempEmd, false, false);

                    Console.WriteLine(tempStart.ToLongDateString() + " - " + tempEmd.ToLongDateString() + "Finished");
                }
            }        

            if (false)
            {
                //var target = new TranslationFromTrasen.Main();

                //最近更新：2016-11-01
                //target.GetDoctor(true);

                //最近更新：2016-11-01
                //target.GetDepartment(true);

                //target.GetDrugUsage();

                //target.GetAntiBioticLevel();

                //target.GetUnit();
            }

            if (true)
            {
                var target = new TranslationFromTrasen.Main();

                for (var time = start; time < end; time = time.AddDays(1))
                {
                    var tempStart = time;
                    var tempEnd = time.AddDays(1);

                    //target.GetPatientsAndOutPatients(tempStart, tempEnd, true);
                    //target.GetOutPatientPrescriptions(tempStart, tempEnd, false);
                    target.GetOutPatientDrugRecords(tempStart, tempEnd, false);
                }                    
            }

            Console.ReadLine();
        }
    }
}