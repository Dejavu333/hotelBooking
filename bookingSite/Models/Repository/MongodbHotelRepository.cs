using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using bookingSite.Utils;

namespace bookingSite.Models
{
	//?MongodbHotelRepository-----------------------------------------------------MongodbHotelRepository-----------------------------------------------------MongodbHotelRepository//
	public sealed class MongodbHotelRepository : AbstractHotelRepository
	{
        //!properties
        //inherited from AbstractHotelRepository+
        private readonly ILog _log;

        //!constructors
        public MongodbHotelRepository(IDataStoreAccess p_dataStoreAcces, ILog p_log)
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
			// insert into hotels; using mongodb queries
			p_model._id = ObjectId.GenerateNewId();

			var json = ((Object)p_model).ToJson();
			
			string cmd = "{ insert: 'hotels', documents: [" + json + @"] }";
            database.RunCommand<BsonDocument>(cmd);

            this._log.logQuery(cmd); //logs every CUD operation

            //?2.
            // insert into hotels; using the driver helper functions
            // var collection = database.GetCollection<Hotel>("hotels");
            //collection.InsertOne((Hotel)p_model);
        }

        public override Object Read(object p_id)
		{
			var mongoClient = this._dataStoreAccess.link();	
			var database = mongoClient.GetDatabase("bookingSite");

            // select * from hotels where _name=p_name; using mongodb queries
			var c = database.RunCommand<BsonDocument>(@"{ 
															find: 'hotels', 
															filter: { _id:ObjectId('" + p_id + @"')}
                                                        }");
		    var bsonArr = c["cursor"]["firstBatch"];

            // process data
            List<Object> result = new List<Object>();
            foreach (BsonDocument doc in bsonArr)
            {
                Hotel h = BsonSerializer.Deserialize<Hotel>(doc);
                result.Add(h);
            }

            return result[0];
		}
		
		public override Object ReadEvery()
		{
			var mongoClient = this._dataStoreAccess.link();
			var database = mongoClient.GetDatabase("bookingSite");

			// select * from hotels where _name=p_name; using mongodb queries
			var c = database.RunCommand<BsonDocument>(@"{ 
															find: 'hotels', 
															filter: {}
                                                        }");
			var bsonArr = c["cursor"]["firstBatch"];
			
			// process data
			List<Hotel> result = new List<Hotel>();
			foreach (BsonDocument doc in bsonArr)
			{
				Hotel h = BsonSerializer.Deserialize<Hotel>(doc);
				result.Add(h);
			}

			return result;
		}
		
		public override void Update(object p_model)
		{
			var mongoClient = this._dataStoreAccess.link();
			var database = mongoClient.GetDatabase("bookingSite");
            
			var collection = database.GetCollection<Hotel>("hotels");
			var filter = Builders<Hotel>.Filter.Eq("_id", ((Hotel)p_model)._id);
			collection.ReplaceOne(filter, (Hotel)p_model);
		}

		public override void Delete(object p_model)
		{
			var mongoClient = this._dataStoreAccess.link();
			var database = mongoClient.GetDatabase("bookingSite");
            
			var collection = database.GetCollection<Hotel>("hotels");
			var filter = Builders<Hotel>.Filter.Eq("name", ((Hotel)p_model)._name);
			collection.DeleteOne(filter);
		}

		
		public override List<Hotel> readByName(string p_name)
		{
			var mongoClient = this._dataStoreAccess.link();
			var database = mongoClient.GetDatabase("bookingSite");

			// select * from hotels where _name=p_name; using mongodb queries
			var c = database.RunCommand<BsonDocument>(@"{ 
															find: 'hotels', 
															filter: { _name:'" + p_name + @"'}
                                                        }");
			var bsonArr = c["cursor"]["firstBatch"];

			// process data
			List<Hotel> result = new List<Hotel>();
			foreach (BsonDocument doc in bsonArr)
			{
				// var name = doc["name"].RawValue;			// rawValue removes braces
				// var address = doc["address"].RawValue;	// doc.Names , doc.Values						

				Hotel h = BsonSerializer.Deserialize<Hotel>(doc);
				result.Add(h);
			}

			return result;
		}

		public override List<Hotel> readByDetails(string[] p_details)
		{
			var mongoClient = this._dataStoreAccess.link();
			var database = mongoClient.GetDatabase("bookingSite");

			// select * from hotels where _country=p_country and _starCount=p_starCount; using mongodb queries
			string countryCmd = p_details[0];
			string checkInDateCmd = (p_details[1] ??= "2018-10-10");
			string checkOutDateCmd = (p_details[2] ??= "2018-10-10");
            string starCountCmd = p_details[3] == "0" ? "{$gt:0}" : p_details[3];
			//leaves earilier than we arrive OR arrives later than we leave, this constraint must be met for all elements in _bookings, THEN the room is available
			string timeFrameCmd = "{$lte:2}";
			
			
			var c = database.RunCommand<BsonDocument>(@"{ 
															find: 'hotels',
															filter: 
															{ 
																_country:{$regex:'^" +countryCmd+ @"', $options: 'i'}, 
																_starCount:" + starCountCmd + @",
																_rooms:
																{
																	$elemMatch:
																	{
																		$or:[{_bookings:null},{
																			_bookings:
																			{
																				$all:
																				[{
																					$elemMatch:
																					{
																						$or: 
																						[{
																							checkInDate: {$gt:ISODate('" + checkOutDateCmd+@"')}
																						},
																						{
																							checkOutDate: {$lt:ISODate('"+checkInDateCmd+@"')}
																						}]
																					}
																				}]
																			}
																		}]
																	}
																}
															}
														}");    //i case insensitive
			var bsonArr = c["cursor"]["firstBatch"];

			// process data
			List<Hotel> result = new List<Hotel>();
			foreach (BsonDocument doc in bsonArr)
			{
				Hotel h = BsonSerializer.Deserialize<Hotel>(doc);
				result.Add(h);
			}
			
			return result;
		}

        public override List<Booking> readBookingsByEamil(string email)
        {
            var mongoClient = this._dataStoreAccess.link();
            var database = mongoClient.GetDatabase("bookingSite");

            var c = database.RunCommand<BsonDocument>(@"{ 
															find: 'hotels',
															filter: 
															{ 		
																_rooms:
																{
																	$elemMatch:
																	{
																		_bookings:
																		{
																			$elemMatch:
																			{
																				email:'" + email + @"'
																			}
																		}
																	}
																}
															}
														}");    //i case insensitive
            var bsonArr = c["cursor"]["firstBatch"];

            // process data
            List<Hotel> result = new List<Hotel>();
            foreach (BsonDocument doc in bsonArr)
            {
                Hotel h = BsonSerializer.Deserialize<Hotel>(doc);
                result.Add(h);
            }
            List<Booking> bookings = new List<Booking>();
			foreach (Hotel hotel in result)
			{
				foreach (Room room in hotel._rooms)
				{
					if (room._bookings != null)
					{
						foreach (Booking booking in room._bookings)
						{
							if (booking.email == email)
							{
								bookings.Add(booking);
							}
						}
					}
				}
			}
			//bookings = result.SelectMany(hotel => hotel._rooms).SelectMany(room => room._bookings).Where(booking => booking.email == email).ToList();

			return bookings;
        }
        
    }//MongodbHotelRepository class ends

}//bookingSite.Models namespace ends
