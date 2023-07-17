using bookingSite.Utils;
using MongoDB.Bson;
using SharpCompress.Common;

namespace bookingSite.Models
{
    //?MongodbRoomRepository-----------------------------------------------------MongodbRoomRepository-----------------------------------------------------MongodbRoomRepository//
    public sealed class MongodbRoomRepository : AbstractRoomRepository
    {
        //!properties
        //inherited from AbstractHotelRepository+
        private readonly ILog _log;

        //!constructors
        public MongodbRoomRepository(IDataStoreAccess p_dataStoreAcces, ILog p_log)
                                    : base(p_dataStoreAcces)
        {
			//inherited from AbstractHotelRepository+
			this._log = p_log;
		}

        //!methods
        public override void Create(dynamic p_model)
        {
			var database = this._dataStoreAccess.link().GetDatabase("bookingSite");

			//?1.
			// insert into rooms; using mongodb queries
			p_model._id = ObjectId.GenerateNewId();

			var json = ((Object)p_model).ToJson();

			string cmd = "{ insert: 'rooms', documents: [" + json + @"] }";
			database.RunCommand<BsonDocument>(cmd);

			this._log.logQuery(cmd); //logs every CUD operation
		}
        public override object Read(object p_id)
        {
            throw new NotImplementedException();
        }
        public override List<object> ReadEvery()
        {
            throw new NotImplementedException();
        }
        public override void Update(object p_model)
        {
            throw new NotImplementedException();
        }
        public override void Delete(object p_model)
        {
            throw new NotImplementedException();
        }



    }//MongodbRoomRepository class ends

}//bookingSite.Models namespace ends
