using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using MongoDB.Bson.Serialization;
using System.Collections.ObjectModel;

namespace Anthology.SimulationManager.HistoryManager
{
    /// <summary>
    /// Concrete implementation of HistoryLogger using local instance of MongoDB.
    /// </summary>
    public class MongoHM : HistoryLogger
    {
        /// <summary>
        /// Client URL to use for connecting to MongoDB server.
        /// </summary>
        private MongoClient DbClient { get; set; } = new MongoClient("mongodb://localhost:27017/");

        /// <summary>
        /// Name of collection to save sim states.
        /// </summary>
        private const string SAVE_STATE_COLLECTION_NAME = "save_states";

        /// <summary>
        /// MongoDB collection containing sim states.
        /// </summary>
        private IMongoCollection<SimState> SimStates { get; set; }

        /// <summary>
        /// MongoDB database containing event log and sim states.
        /// </summary>
        private IMongoDatabase Database { get; set; }
        
        /// <summary>
        /// MongoDB collection containing event log entries.
        /// </summary>
        private IMongoCollection<EventLog> LastUsedLog { get; set; }

        /// <summary>
        /// Creates and initializes database and collections.
        /// </summary>
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

        /// <summary>
        /// Adds an NPC to be recorded by log.
        /// </summary>
        /// <param name="npc">The NPC to log.</param>
        public override void AddNpcToLog(NPC npc)
        {
            ELog.NpcChanges.Add(npc.Name, npc);
        }

        /// <summary>
        /// Push NPC state to the log given destination.
        /// </summary>
        /// <param name="destination">Where to store npc state data.</param>
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

        /// <summary>
        /// Saves the current sim state using given name.
        /// </summary>
        /// <param name="stateName">The name applied to the current sim state.</param>
        public override void SaveState(string stateName)
        {
            SimStates.InsertOne(new SimState(stateName));
        }

        /// <summary>
        /// Searches for a sim state in the sim state collection and returns it.
        /// </summary>
        /// <param name="stateName">The name of the sim state to find.</param>
        /// <returns>The sim state with the given name.</returns>
        /// <exception cref="NullReferenceException">Thrown if sim state not found.</exception>
        public override SimState LoadState(string stateName)
        {
            SimState state = SimStates.Find(simState => simState.SimName.Equals(stateName) ).ToList().First();
            if(state == null)
                throw new NullReferenceException("Not state with name: " + stateName);
            return state;
        }

        /// <summary>
        /// Deletes a sim state from the sim state collection.
        /// </summary>
        /// <param name="stateName">The name of the sim state to delete.</param>
        public override void DeleteState(string stateName)
        {
            SimStates.DeleteOne(state => state.SimName.Equals(stateName));
        }

        /// <summary>
        /// Clears all sim states from the sim state collection.
        /// </summary>
        public override void ClearStates()
        {
            Database.DropCollection(SAVE_STATE_COLLECTION_NAME);
        }

        /// <summary>
        /// Clears an event log with given name.
        /// </summary>
        /// <param name="log">The name of the log to delete.</param>
        public override void ClearLog(string log)
        {
            Database.GetCollection<EventLog>(log).DeleteMany(Builders<EventLog>.Filter.Empty);
        }

        ///<summary>
        /// Exports logs to a .json file
        ///</summary>
        /// <param name="collectionName">Name of collection to export</param>
        public override void ExportCollection()
        {
            var collection = Database.GetCollection<BsonDocument>("NPC History");
            var documents = collection.AsQueryable<BsonDocument>();
            var json = documents.ToJson();

            File.WriteAllText(@"C:\Users\aguia\Desktop\export.json", json);

            //Console.WriteLine(json.ToString());


            //var collection = Database.GetCollection<BsonDocument>("NPC History");
            //var documents = collection.AsQueryable();



            //File.WriteAllText(@"C:\Users\aguia\Desktop\export.json", text);

            //foreach(BsonDocument doc in documents)
            //{
            //var jsonWriter = new JsonWriterSettings { OutputMode = JsonOutputMode.CanonicalExtendedJson };
            //JObject json = JObject.Parse(doc.ToJson<MongoDB.Bson.BsonDocument>(jsonWriter));
            //Console.WriteLine(json.ToString());
            //}

            //var collAsDotNetObj = BsonTypeMapper.MapToDotNetValue(collection);
        }

        /// <summary>
        /// Determines if database is connected.
        /// </summary>
        /// <returns>True if database is connected.</returns>
        public bool IsConnected()
        {
            return Database != null;
        }
    }
}
