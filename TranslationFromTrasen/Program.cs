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
            DateTime start = new DateTime(2016, 9, 1);
            DateTime end = new DateTime(2016, 10, 1);

            for (var time = start; time < end; time = time.AddDays(1))
            {
                var tempStart = time;
                var tempEmd = time.AddDays(1);

                Console.WriteLine(tempStart.ToLongDateString() + " - " + tempEmd.ToLongDateString() + "Startted");

                var target = new TranslationFromTrasen.Main();

                //已完成从2016-07-01到2016-10-01（后界开区间）
                target.GetPatientAndInPatient(tempStart, tempEmd, true, false, true);

                //已完成从2016-07-01到2016-09-01（后界开区间）
                //target.GetDrugRecord(tempStart, tempEmd, false, false);

                Console.WriteLine(tempStart.ToLongDateString() + " - " + tempEmd.ToLongDateString() + "Finished");
            }
        }
    }
}