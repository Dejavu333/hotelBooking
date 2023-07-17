using SharpCompress.Common;

namespace bookingSite.Models
{
    //?MongodbGuestRepository-----------------------------------------------------MongodbGuestRepository-----------------------------------------------------MongodbGuestRepository//
    public sealed class MongodbGuestRepository : AbstractGuestRepository
    {
        //!properties
        //inherited from AbstractHotelRepository

        //!constructors
        public MongodbGuestRepository(IDataStoreAccess p_dataStoreAcces) : base(p_dataStoreAcces)
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



    }//MongodbGuestRepository class ends

}//bookingSite.Models namespace ends
