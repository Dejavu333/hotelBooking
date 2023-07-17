namespace bookingSite.Models
{
    //?MysqlHotelRepository-----------------------------------------------------MysqlHotelRepository-----------------------------------------------------MysqlHotelRepository//
    public sealed class MysqlHotelRepository : AbstractHotelRepository
    {
        //!properties
        //inherited from AbstractHotelRepository

        //!constructors
        public MysqlHotelRepository(IDataStoreAccess p_dataStoreAcces) : base(p_dataStoreAcces)
        {
            //inherited from AbstractHotelRepository
        }

        //!methods
        public override void Create(dynamic p_model)
        {
            throw new NotImplementedException();
        }
        public override object Read(object p
            )
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

        public override List<Hotel> readByDetails(string[] p_details)
        {
            throw new NotImplementedException();
        }

		public override List<Hotel> readByName(string p_name)
		{
			throw new NotImplementedException();
		}

        public override List<Booking> readBookingsByEamil(string email)
        {
            throw new NotImplementedException();
        }
    }//MysqlHotelRepository class ends

}//bookingSite.Models namespace ends
