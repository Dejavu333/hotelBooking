namespace bookingSite.Models
{
	//?IRepository-----------------------------------------------------IRepository-----------------------------------------------------IRepository//
    ////   public interface IRepository 
    ////   {
    ////       //access
	////	public IDataStoreAccess dataStoreAcces();
	////	//CRUD
	////	public void create(dynamic p_entity);
	////	public dynamic readOne(dynamic p_entity);
	////	public List<dynamic> readEvery();
	////	public void update(dynamic p_entity);
	////	public void delete(dynamic p_entity);
	////}//IRepository interface ends
	
	
    //?AbstractRepository-----------------------------------------------------AbstractRepository-----------------------------------------------------AbstractRepository//
    public abstract class AbstractRepository
    {
        //!properties
        protected readonly IDataStoreAccess _dataStoreAccess;
		
        //!constructors
        public AbstractRepository(IDataStoreAccess p_dataStoreAcces)
        {
            this._dataStoreAccess = p_dataStoreAcces;
        }

		//!methods
		public abstract void Create(Object p_model);
		public abstract Object Read(Object p_id);
		public abstract Object ReadEvery();
		public abstract void Update(Object p_model);
		public abstract void Delete(Object p_model);

	}//AbstractRepository class ends


    //?AbstractHotelRepository-----------------------------------------------------AbstractHotelRepository-----------------------------------------------------AbstractHotelRepository//
    public abstract class AbstractHotelRepository : AbstractRepository
    {
        //!properties
        //inherited from AbstractRepository class

        //!constructors
        public AbstractHotelRepository(IDataStoreAccess p_dataStoreAcces) : base(p_dataStoreAcces)
        {
            //inherited from AbstractRepository class
        }

        //!methods
        //inherited from AbstractRepository class +
        //////public abstract int CountFullRooms();
        //////public abstract int CountFreeRooms();
        public abstract List<Hotel> readByDetails(string[] p_details);
        public abstract List<Hotel> readByName(string p_name);
        public abstract List<Booking> readBookingsByEamil(string email);

    }//AbstractHotelRepository class ends


    //?AbstractRoomRepository-----------------------------------------------------AbstractRoomRepository-----------------------------------------------------AbstractRoomRepository//
    public abstract class AbstractRoomRepository : AbstractRepository
    {
        //!properties
        //inherited from AbstractRepository class

        //!constructors
        public AbstractRoomRepository(IDataStoreAccess p_dataStoreAcces) : base(p_dataStoreAcces)
        {
            //inherited from AbstractRepository class
        }


        //!methods
        //inherited from AbstractRepository +


        
    }//AbstractRoomRepository class ends

    
    //?AbstractGuestRepository-----------------------------------------------------AbstractGuestRepository-----------------------------------------------------AbstractGuestRepository//
    public abstract class AbstractGuestRepository : AbstractRepository
    {
        //!properties
        //inherited from AbstractRepository

        //!constructors
        public AbstractGuestRepository(IDataStoreAccess p_dataStoreAcces) : base(p_dataStoreAcces)
        {
            //inherited from AbstractRepository
        }


        //!methods
        //inherited from AbstractRepository +


        
    }//AbstractGuestRepository class ends

    
    //?AbstractBookingRepository-----------------------------------------------------AbstractBookingRepository-----------------------------------------------------AbstractBookingRepository//
    public abstract class AbstractBookingRepository : AbstractRepository
    {
        //!properties
        //inherited from AbstractRepository

        //!constructors
        public AbstractBookingRepository(IDataStoreAccess p_dataStoreAcces) : base(p_dataStoreAcces)
        {
            //inherited from AbstractRepository
        }

        //methods
        //inherited from AbstractRepository +

        
        
    }//AbstractBookingRepository class ends

}//bookingSite.Models namespace ends
