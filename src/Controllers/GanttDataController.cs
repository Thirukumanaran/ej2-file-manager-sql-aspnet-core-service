using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace EJ2APIServices.Controllers
{
    public class GanttDataController : Controller
    {
        public static List<GanttDataSource> DataList = null;

        // GET: api/GanttData  
        [Route("api/GanttData")]
        [HttpGet]
        [EnableCors("AllowAllOrigins")]
        public object Get()
        {
            DataList = GanttDataSource.remoteData().ToList();
            return new { result = DataList, count = DataList.Count() };
        }

        public class GanttDataSource
        {
            public int TaskId { get; set; }
            public string TaskName { get; set; }
            public DateTime StartDate { get; set; }
            public int? Duration { get; set; }
            public int Progress { get; set; }
            public string Predecessor { get; set; }
            public List<GanttDataSource> SubTasks { get; set; }
            public static List<GanttDataSource> remoteData()
            {
                List<GanttDataSource> GanttDataSourceCollection = new List<GanttDataSource>();

                GanttDataSource Record1 = new GanttDataSource()
                {
                    TaskId = 1,
                    TaskName = "Germination",
                    StartDate = new DateTime(2019, 03, 01),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record1Child1 = new GanttDataSource()
                {
                    TaskId = 2,
                    TaskName = "Dry seed (caryopsis)",
                    StartDate = new DateTime(2019, 03, 01),
                    Duration = 0,
                };
                GanttDataSource Record1Child2 = new GanttDataSource()
                {
                    TaskId = 3,
                    TaskName = "Seed imbibition complete",
                    StartDate = new DateTime(2019, 03, 01),
                    Duration = 3,
                    Predecessor = "2"
                };
                GanttDataSource Record1Child3 = new GanttDataSource()
                {
                    TaskId = 4,
                    TaskName = "Radicle emerged from caryopsis",
                    StartDate = new DateTime(2019, 03, 04),
                    Duration = 2,
                    Predecessor = "3"
                };
                GanttDataSource Record1Child4 = new GanttDataSource()
                {
                    TaskId = 5,
                    TaskName = "Coleoptile emerged from caryopsis",
                    StartDate = new DateTime(2019, 03, 06),
                    Duration = 2,
                    Predecessor = "4"
                };
                GanttDataSource Record1Child5 = new GanttDataSource()
                {
                    TaskId = 6,
                    TaskName = "Emergence: coleoptile penetrates soil surface (cracking stage)",
                    StartDate = new DateTime(2019, 03, 08),
                    Duration = 2,
                    Predecessor = "5"
                };
                Record1.SubTasks.Add(Record1Child1);
                Record1.SubTasks.Add(Record1Child2);
                Record1.SubTasks.Add(Record1Child3);
                Record1.SubTasks.Add(Record1Child4);
                Record1.SubTasks.Add(Record1Child5);


                GanttDataSource Record2 = new GanttDataSource()
                {
                    TaskId = 7,
                    TaskName = "Leaf development",
                    StartDate = new DateTime(2019, 03, 10),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record2Child1 = new GanttDataSource()
                {
                    TaskId = 8,
                    TaskName = "First leaf through coleoptile",
                    StartDate = new DateTime(2019, 03, 10),
                    Duration = 1,
                    Predecessor = "6"
                };
                GanttDataSource Record2Child2 = new GanttDataSource()
                {
                    TaskId = 9,
                    TaskName = "First leaf unfolded",
                    StartDate = new DateTime(2019, 03, 11),
                    Duration = 1,
                    Predecessor = "8"
                };
                GanttDataSource Record2Child3 = new GanttDataSource()
                {
                    TaskId = 10,
                    TaskName = "3 leaves unfolded",
                    StartDate = new DateTime(2019, 03, 12),
                    Duration = 2,
                    Predecessor = "9"
                };
                GanttDataSource Record2Child4 = new GanttDataSource()
                {
                    TaskId = 11,
                    TaskName = "9 or more leaves unfolded",
                    StartDate = new DateTime(2019, 03, 14),
                    Duration = 5,
                    Predecessor = "10"
                };
                Record2.SubTasks.Add(Record2Child1);
                Record2.SubTasks.Add(Record2Child2);
                Record2.SubTasks.Add(Record2Child3);
                Record2.SubTasks.Add(Record2Child4);

                GanttDataSource Record3 = new GanttDataSource()
                {
                    TaskId = 12,
                    TaskName = "Tillering",
                    StartDate = new DateTime(2019, 03, 18),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record3Child1 = new GanttDataSource()
                {
                    TaskId = 13,
                    TaskName = "Beginning of tillering: first tiller detectable",
                    StartDate = new DateTime(2019, 03, 18),
                    Duration = 0,
                    Predecessor = "11"
                };
                GanttDataSource Record3Child2 = new GanttDataSource()
                {
                    TaskId = 14,
                    TaskName = "2 tillers detectable",
                    StartDate = new DateTime(2019, 03, 19),
                    Duration = 3,
                    Predecessor = "13"
                };
                GanttDataSource Record3Child3 = new GanttDataSource()
                {
                    TaskId = 15,
                    TaskName = "3 tillers detectable",
                    StartDate = new DateTime(2019, 03, 22),
                    Duration = 3,
                    Predecessor = "14"
                };
                GanttDataSource Record3Child4 = new GanttDataSource()
                {
                    TaskId = 16,
                    TaskName = "Maximum no. of tillers detectable",
                    StartDate = new DateTime(2019, 03, 25),
                    Duration = 6,
                    Predecessor = "15"
                };
                GanttDataSource Record3Child5 = new GanttDataSource()
                {
                    TaskId = 17,
                    TaskName = "End of tillering",
                    StartDate = new DateTime(2019, 03, 30),
                    Duration = 0,
                    Predecessor = "16"
                };
                Record3.SubTasks.Add(Record3Child1);
                Record3.SubTasks.Add(Record3Child2);
                Record3.SubTasks.Add(Record3Child3);
                Record3.SubTasks.Add(Record3Child4);
                Record3.SubTasks.Add(Record3Child5);

                GanttDataSource Record4 = new GanttDataSource()
                {
                    TaskId = 18,
                    TaskName = "Stem elongation",
                    StartDate = new DateTime(2019, 03, 30),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record4Child1 = new GanttDataSource()
                {
                    TaskId = 19,
                    TaskName = "Beginning of stem elongation: pseudostem and tillers erect, first internode begins to elongate, top of inflorescence at least 1 cm above tillering node",
                    StartDate = new DateTime(2019, 03, 30),
                    Duration = 0,
                    Predecessor = "17"
                };
                GanttDataSource Record4Child2 = new GanttDataSource()
                {
                    TaskId = 20,
                    TaskName = "First node at least 1 cm above tillering node",
                    StartDate = new DateTime(2019, 03, 31),
                    Duration = 1,
                    Predecessor = "19"
                };
                GanttDataSource Record4Child3 = new GanttDataSource()
                {
                    TaskId = 21,
                    TaskName = "Node 3 at least 2 cm above node 2",
                    StartDate = new DateTime(2019, 04, 01),
                    Duration = 2,
                    Predecessor = "20"
                };
                GanttDataSource Record4Child4 = new GanttDataSource()
                {
                    TaskId = 22,
                    TaskName = "Flag leaf just visible, still rolled",
                    StartDate = new DateTime(2019, 04, 03),
                    Duration = 4,
                    Predecessor = "21"
                };
                GanttDataSource Record4Child5 = new GanttDataSource()
                {
                    TaskId = 23,
                    TaskName = "Flag leaf stage: flag leaf fully unrolled, ligule just visible",
                    StartDate = new DateTime(2019, 04, 07),
                    Duration = 2,
                    Predecessor = "22"
                };
                Record4.SubTasks.Add(Record4Child1);
                Record4.SubTasks.Add(Record4Child2);
                Record4.SubTasks.Add(Record4Child3);
                Record4.SubTasks.Add(Record4Child4);
                Record4.SubTasks.Add(Record4Child5);

                GanttDataSource Record5 = new GanttDataSource()
                {
                    TaskId = 24,
                    TaskName = "Booting",
                    StartDate = new DateTime(2019, 04, 09),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record5Child1 = new GanttDataSource()
                {
                    TaskId = 25,
                    TaskName = "Early boot stage: flag leaf sheath extending",
                    StartDate = new DateTime(2019, 04, 09),
                    Duration = 2,
                    Predecessor = "23"
                };
                GanttDataSource Record5Child2 = new GanttDataSource()
                {
                    TaskId = 26,
                    TaskName = "Mid boot stage: flag leaf sheath just visibly swollen",
                    StartDate = new DateTime(2019, 04, 11),
                    Duration = 2,
                    Predecessor = "25"
                };
                GanttDataSource Record5Child3 = new GanttDataSource()
                {
                    TaskId = 27,
                    TaskName = "Late boot stage: flag leaf sheath swollen",
                    StartDate = new DateTime(2019, 04, 13),
                    Duration = 2,
                    Predecessor = "26"
                };
                GanttDataSource Record5Child4 = new GanttDataSource()
                {
                    TaskId = 28,
                    TaskName = "Flag leaf sheath opening",
                    StartDate = new DateTime(2019, 04, 15),
                    Duration = 2,
                    Predecessor = "27"
                };
                GanttDataSource Record5Child5 = new GanttDataSource()
                {
                    TaskId = 29,
                    TaskName = "First awns visible (in awned forms only)",
                    StartDate = new DateTime(2019, 04, 17),
                    Duration = 2,
                    Predecessor = "28"
                };
                Record5.SubTasks.Add(Record5Child1);
                Record5.SubTasks.Add(Record5Child2);
                Record5.SubTasks.Add(Record5Child3);
                Record5.SubTasks.Add(Record5Child4);
                Record5.SubTasks.Add(Record5Child5);


                GanttDataSource Record6 = new GanttDataSource()
                {
                    TaskId = 30,
                    TaskName = "Inflorescence emergence, heading",
                    StartDate = new DateTime(2019, 04, 18),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record6Child1 = new GanttDataSource()
                {
                    TaskId = 31,
                    TaskName = "Beginning of heading: tip of inflorescence emerged from sheath, first spikelet just visible",
                    StartDate = new DateTime(2019, 04, 18),
                    Duration = 0,
                    Predecessor = "29"
                };
                GanttDataSource Record6Child2 = new GanttDataSource()
                {
                    TaskId = 32,
                    TaskName = "20% of inflorescence emerged",
                    StartDate = new DateTime(2019, 04, 19),
                    Duration = 3,
                    Predecessor = "31"
                };
                GanttDataSource Record6Child3 = new GanttDataSource()
                {
                    TaskId = 33,
                    TaskName = "40% of inflorescence emerged",
                    StartDate = new DateTime(2019, 04, 22),
                    Duration = 2,
                    Predecessor = "32"
                };
                GanttDataSource Record6Child4 = new GanttDataSource()
                {
                    TaskId = 34,
                    TaskName = "Middle of heading: half of inflorescence emerged",
                    StartDate = new DateTime(2019, 04, 23),
                    Duration = 0,
                    Predecessor = "33"
                };
                GanttDataSource Record6Child5 = new GanttDataSource()
                {
                    TaskId = 35,
                    TaskName = "60% of inflorescence emerged",
                    StartDate = new DateTime(2019, 04, 24),
                    Duration = 2,
                    Predecessor = "34"
                };
                GanttDataSource Record6Child6 = new GanttDataSource()
                {
                    TaskId = 36,
                    TaskName = "80% of inflorescence emerged",
                    StartDate = new DateTime(2018, 04, 26),
                    Duration = 3,
                    Predecessor = "35"
                };
                GanttDataSource Record6Child7 = new GanttDataSource()
                {
                    TaskId = 37,
                    TaskName = "End of heading: inflorescence fully emerged",
                    StartDate = new DateTime(2018, 04, 28),
                    Duration = 0,
                    Predecessor = "36"
                };
                Record6.SubTasks.Add(Record6Child1);
                Record6.SubTasks.Add(Record6Child2);
                Record6.SubTasks.Add(Record6Child3);
                Record6.SubTasks.Add(Record6Child4);
                Record6.SubTasks.Add(Record6Child5);
                Record6.SubTasks.Add(Record6Child6);
                Record6.SubTasks.Add(Record6Child7);

                GanttDataSource Record7 = new GanttDataSource()
                {
                    TaskId = 38,
                    TaskName = "Flowering, anthesis",
                    StartDate = new DateTime(2019, 04, 28),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record7Child1 = new GanttDataSource()
                {
                    TaskId = 39,
                    TaskName = "Beginning of flowering: first anthers visible",
                    StartDate = new DateTime(2019, 04, 28),
                    Duration = 0,
                    Predecessor = "37"
                };
                GanttDataSource Record7Child2 = new GanttDataSource()
                {
                    TaskId = 40,
                    TaskName = "Full flowering: 50% of anthers mature",
                    StartDate = new DateTime(2019, 04, 29),
                    Duration = 5,
                    Predecessor = "39"
                };
                GanttDataSource Record7Child3 = new GanttDataSource()
                {
                    TaskId = 41,
                    TaskName = "spikelet have completed flowering",
                    StartDate = new DateTime(2019, 05, 04),
                    Duration = 5,
                    Predecessor = "40"
                };
                GanttDataSource Record7Child4 = new GanttDataSource()
                {
                    TaskId = 42,
                    TaskName = "End of flowering",
                    StartDate = new DateTime(2019, 05, 08),
                    Duration = 0,
                    Predecessor = "41"
                };
                Record7.SubTasks.Add(Record7Child1);
                Record7.SubTasks.Add(Record7Child2);
                Record7.SubTasks.Add(Record7Child3);
                Record7.SubTasks.Add(Record7Child4);

                GanttDataSource Record8 = new GanttDataSource()
                {
                    TaskId = 43,
                    TaskName = "Development of fruit",
                    StartDate = new DateTime(2019, 05, 08),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record8Child1 = new GanttDataSource()
                {
                    TaskId = 44,
                    TaskName = "Watery ripe: first grains have reached half their final size",
                    StartDate = new DateTime(2019, 05, 08),
                    Duration = 0,
                    Predecessor = "42"
                };
                GanttDataSource Record8Child2 = new GanttDataSource()
                {
                    TaskId = 45,
                    TaskName = "Early milk",
                    StartDate = new DateTime(2019, 05, 09),
                    Duration = 3,
                    Predecessor = "44"
                };
                GanttDataSource Record8Child3 = new GanttDataSource()
                {
                    TaskId = 46,
                    TaskName = "Medium milk: grain content milky, grains reached final size,still green",
                    StartDate = new DateTime(2019, 05, 12),
                    Duration = 3,
                    Predecessor = "45"
                };
                GanttDataSource Record8Child4 = new GanttDataSource()
                {
                    TaskId = 47,
                    TaskName = "Late milk",
                    StartDate = new DateTime(2019, 05, 15),
                    Duration = 2,
                    Predecessor = "46"
                };
                Record8.SubTasks.Add(Record8Child1);
                Record8.SubTasks.Add(Record8Child2);
                Record8.SubTasks.Add(Record8Child3);
                Record8.SubTasks.Add(Record8Child4);

                GanttDataSource Record9 = new GanttDataSource()
                {
                    TaskId = 48,
                    TaskName = "Ripening",
                    StartDate = new DateTime(2019, 05, 17),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record9Child1 = new GanttDataSource()
                {
                    TaskId = 49,
                    TaskName = "Early dough",
                    StartDate = new DateTime(2019, 05, 17),
                    Duration = 6,
                    Predecessor = "47"
                };
                GanttDataSource Record9Child2 = new GanttDataSource()
                {
                    TaskId = 50,
                    TaskName = "Soft dough: grain content soft but dry. Fingernail impression not held",
                    StartDate = new DateTime(2019, 05, 23),
                    Duration = 2,
                    Predecessor = "49"
                };
                GanttDataSource Record9Child3 = new GanttDataSource()
                {
                    TaskId = 51,
                    TaskName = "Hard dough: grain content solid. Fingernail impression held",
                    StartDate = new DateTime(2019, 05, 25),
                    Duration = 2,
                    Predecessor = "50"
                };
                GanttDataSource Record9Child4 = new GanttDataSource()
                {
                    TaskId = 52,
                    TaskName = "Fully ripe: grain hard, difficult to divide with thumbnail",
                    StartDate = new DateTime(2019, 05, 27),
                    Duration = 2,
                    Predecessor = "51"
                };
                Record9.SubTasks.Add(Record9Child1);
                Record9.SubTasks.Add(Record9Child2);
                Record9.SubTasks.Add(Record9Child3);
                Record9.SubTasks.Add(Record9Child4);
                GanttDataSource Record10 = new GanttDataSource()
                {
                    TaskId = 53,
                    TaskName = "Senescence",
                    StartDate = new DateTime(2019, 05, 29),
                    SubTasks = new List<GanttDataSource>(),
                };
                GanttDataSource Record10Child1 = new GanttDataSource()
                {
                    TaskId = 54,
                    TaskName = "Over-ripe: grain very hard, cannot be dented by thumbnail",
                    StartDate = new DateTime(2019, 05, 29),
                    Duration = 3,
                    Predecessor = "52"
                };
                GanttDataSource Record10Child2 = new GanttDataSource()
                {
                    TaskId = 55,
                    TaskName = "Grains loosening in day-time",
                    StartDate = new DateTime(2019, 06, 01),
                    Duration = 1,
                    Predecessor = "54"
                };
                GanttDataSource Record10Child3 = new GanttDataSource()
                {
                    TaskId = 56,
                    TaskName = "Plant dead and collapsing",
                    StartDate = new DateTime(2019, 06, 02),
                    Duration = 4,
                    Predecessor = "55"
                };
                GanttDataSource Record10Child4 = new GanttDataSource()
                {
                    TaskId = 57,
                    TaskName = "Harvested product",
                    StartDate = new DateTime(2019, 06, 06),
                    Duration = 2,
                    Predecessor = "56"
                };
                Record10.SubTasks.Add(Record10Child1);
                Record10.SubTasks.Add(Record10Child2);
                Record10.SubTasks.Add(Record10Child3);
                Record10.SubTasks.Add(Record10Child4);

                GanttDataSourceCollection.Add(Record1);
                GanttDataSourceCollection.Add(Record2);
                GanttDataSourceCollection.Add(Record3);
                GanttDataSourceCollection.Add(Record4);
                GanttDataSourceCollection.Add(Record5);
                GanttDataSourceCollection.Add(Record6);
                GanttDataSourceCollection.Add(Record7);
                GanttDataSourceCollection.Add(Record8);
                GanttDataSourceCollection.Add(Record9);
                GanttDataSourceCollection.Add(Record10);
                return GanttDataSourceCollection;
            }
           
        }
    }
}
