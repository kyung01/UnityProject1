using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class DataLevel
{
    public string name;
    public int  countMaps,
                countCompleted;
    public class DataStage { public List<MapData> data ; public int countAll, countCompleted;
        public DataStage()
        {
            data = new List<MapData>();
            countAll = 0; countCompleted = 0;
        }
        public void update()
        {
            countAll = data.Count;
            countCompleted = data.Select(s => (s.isGameOver()) ? 1 : 0).Sum();
            //countCompleted += 5;
        }
    }

    public List<DataStage> collections;

    static int count = 1;
    public DataLevel()
    {
        count++;
        name = "map_" + Random.Range(0, 10000);
        collections = new List<DataStage>();
        for (int i = 0; i < count; i++)
        {
            var l = new DataStage();
            int countMapsRandom = Random.Range(2,5);
            for(int j =0; j < countMapsRandom;j++){
                l.data.Add(MapGenerator.getBoard_RandomContent(3 + Random.Range(0,2),3+ Random.Range(0,2))) ;
            }
            l.update();
            collections.Add(l);
        }
        countMaps =( from l in collections select l.data.Count).ToArray<int>().Sum();
        countCompleted = (from l in collections select l.countCompleted).ToArray<int>().Sum();

    }
}