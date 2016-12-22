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
            DateTime start = new DateTime(2016, 7, 1);
            DateTime end = new DateTime(2016, 10, 1);
            TimeSpan span = new TimeSpan(1, 0, 0, 0);
            int maxDegreeOfParallelism = 5;
            var target = new TranslationFromTrasen.Main(maxDegreeOfParallelism);

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

            //==住院==

            #region 住院

            if (true)
            {
                for (var time = start; time < end; time += span)
                {
                    var tempStart = time;
                    var tempEnd = time + span;

                    //target.GetPatienstAndInPatients(tempStart, tempEnd, true, false, true);
                    //target.GetInPatientDrugRecords(tempStart, tempEnd, false, true);
                }
            }

            #endregion

            //==门诊==

            #region 门诊

            if (true)
            {
                for (var time = start; time < end; time += span)
                {
                    var tempStart = time;
                    var tempEnd = time + span;

                    target.GetPatientsAndOutPatients(tempStart, tempEnd, true);
                    //target.GetOutPatientPrescriptions(tempStart, tempEnd, true);
                    //target.GetOutPatientDrugRecords(tempStart, tempEnd, true);

                    Console.WriteLine("Finish {0} To {1}.", tempStart, tempEnd);
                }
            }

            #endregion

            //==结束==

            #region "结束"

            if (false)
            {
                Console.ReadLine();
            }

            #endregion
        }
    }
}