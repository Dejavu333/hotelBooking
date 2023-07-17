using MongoDB.Bson;
using MongoDB.Driver;

namespace bookingSite.Models
{
	//?IDataStoreAccess-----------------------------------------------------IDataStoreAccess-----------------------------------------------------IDataStoreAccess//
	public interface IDataStoreAccess
	{
		public dynamic link();
		public bool isOpen();
		public void open();
		public void close();

	}//IDataStoreAccess interface ends

	
	//?IConfig-----------------------------------------------------IConfig-----------------------------------------------------IConfig//
	public interface IConfig
	{
		public Object resourceLocator();
	}//IConfig interface ends

	
	//?MongodbDataStoreAccess-----------------------------------------------------MongodbDataStoreAcces-----------------------------------------------------MongodbDataStoreAcces//
	public sealed class MongodbDataStoreAccess: IDataStoreAccess
	{
		//!properties
		private readonly string _configRepres;
		private dynamic _link;
		                                         
		//!constructors
		public MongodbDataStoreAccess(string  p_configPath= "Models\\DataStoreAcces\\MongodbConfig.txt")
		{

			//p_configPath = System.IO.Directory.GetCurrentDirectory()+"/Models/DataStoreAccess/MongodbConfig.txt";	

			//read text file at p_configPath
			StreamReader reader = new StreamReader(p_configPath);
			Task<string> configRepres = reader.ReadToEndAsync();
			this._configRepres = configRepres.Result;
			reader.Close();

			this._link = null;
		}

		//!methods
		public dynamic link()
		{
			if (this.isOpen())
			{
				return this._link;
			}
			else
			{
				this.open();
				return this._link;
			}
		}
		public bool isOpen()
		{
			if (this._link == null) return false;
		
			else return this._link.GetDatabase("bookingSite").RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
			
		}
		public void open()
		{
			this._link = new MongoClient(this._configRepres);
		}
		public void close()
		{
			_link.close();		
		}
	}//MongodbDataStoreAccess class ends

	
	//?MysqlDataStoreAcces-----------------------------------------------------MysqlDataStoreAcces-----------------------------------------------------MysqlDataStoreAcces//
	public sealed class MysqlDataStoreAccess: IDataStoreAccess
	{
		//!properties
		private readonly string _config;
		private dynamic _link;

		//!constructors
		public MysqlDataStoreAccess(string p_config)
		{
			this._config = p_config;
			this._link = this.link();
		}

		//!methods
		public dynamic link()
		{
			if (this.isAlive())
			{
				return this._link;
			}
			else
			{
				this._link = new MongoClient(this._config);
				return this._link;
			}
		}

		public bool isAlive()
		{
			throw new NotImplementedException();
		}

		public bool isOpen()
		{
			throw new NotImplementedException();
		}

		public void open()
		{
			throw new NotImplementedException();
		}

		public void close()
		{
			throw new NotImplementedException();
		}
	}//MysqlDataStoreAccess class ends

}//bookingSite.Models namespace ends
