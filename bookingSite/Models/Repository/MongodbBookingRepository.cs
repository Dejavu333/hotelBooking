using SharpCompress.Common;

namespace bookingSite.Models
{
    //?MongodbBookingRepository-----------------------------------------------------MongodbBookingRepository-----------------------------------------------------MongodbBookingRepository//
    public sealed class MongodbBookingRepository : AbstractBookingRepository
    {
        //!properties
        //inherited from AbstractHotelRepository

        //!constructors
        public MongodbBookingRepository(IDataStoreAccess p_dataStoreAcces) : base(p_dataStoreAcces)
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



    }//MongodbBookingRepository class ends

}//bookingSite.Models namespace ends
