using MongoDB.Driver;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Anthology.SimulationManager.HistoryManager
{
    public class MongoHM : HistoryLogger
    {
        private MongoClient DbClient { get; set; } = new MongoClient("mongodb://localhost:27017/");

        private const string SAVE_STATE_COLLECTION_NAME = "save_states";

        private IMongoCollection<SimState> SimStates { get; set; }

        private IMongoDatabase Database { get; set; }
        
        private IMongoCollection<EventLog> LastUsedLog { get; set; }

        public MongoHM()
        {
            Database = DbClient.GetDatabase("SimManager");
            LastUsedLog = Database.GetCollection<EventLog>("EventLog");
            if (LastUsedLog == null)
            {
                Database.CreateCollection("EventLog");
                LastUsedLog = Database.GetCollection<EventLog>("EventLog");
            }

            SimStates = Database.GetCollection<SimState>(SAVE_STATE_COLLECTION_NAME);
            if(SimStates == null)
            {
                Database.CreateCollection(SAVE_STATE_COLLECTION_NAME);
                SimStates = Database.GetCollection<SimState>(SAVE_STATE_COLLECTION_NAME);
            }
        }

        public override void AddNpcToLog(NPC npc)
        {
            ELog.NpcChanges.Add(npc.Name, npc);
        }

        public override void LogNpcStates(string? destination)
        {
            IMongoCollection<EventLog> logCollection;
            if (destination != null)
            {
                logCollection = Database.GetCollection<EventLog>(destination);
                if (logCollection == null)
                {
                    Database.CreateCollection(destination);
                    logCollection = Database.GetCollection<EventLog>(destination);
                }
            }
            else
            {
                logCollection = LastUsedLog;
            }
            LastUsedLog = logCollection;
            if (ELog.NpcChanges.Count > 0)
            {
                logCollection.InsertOne(ELog);
                ELog = new();
            }
        }

        public override void SaveState(string stateName)
        {
            SimStates.InsertOne(new SimState(stateName));
        }

        public override SimState LoadState(string stateName)
        {
            SimState state = SimStates.Find(simState => simState.SimName.Equals(stateName) ).ToList().First();
            if(state == null)
                throw new NullReferenceException("Not state with name: " + stateName);
            return state;
        }

        public override void DeleteState(string stateName)
        {
            SimStates.DeleteOne(state => state.SimName.Equals(stateName));
        }

        public override void ClearStates()
        {
            Database.DropCollection(SAVE_STATE_COLLECTION_NAME);
        }

        public override void ClearLog(string log)
        {
            Database.GetCollection<EventLog>(log).DeleteMany(Builders<EventLog>.Filter.Empty);
        }
    }
}
