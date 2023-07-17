using bookingSite.Models;

namespace bookingSite.Utils
{
    //?ILog-----------------------------------------------------ILog-----------------------------------------------------ILog//
    public interface ILog
	{
        public void logQuery(string p_query);

    }//ILog interface ends

    //?LogAppendOnly-----------------------------------------------------LogAppendOnly-----------------------------------------------------LogAppendOnly//
    public sealed class LogAppendOnly : ILog
	{
		//!properties
		private readonly string _logPath;

        //!constructors
        //primary
        public LogAppendOnly(string p_logPath)
        {
            this._logPath = p_logPath;
            
            if (!File.Exists(_logPath)) //if doesn't exists creates one
            {
                File.Create(_logPath).Close();
            }
        }
        //secondary
        [ActivatorUtilitiesConstructor]
        public LogAppendOnly(IConfiguration config) : this( config.GetSection("initConfig")["logPath"] ) {}
       
        //!methods
        //saves every successful operation into a log file
        public void logQuery(string p_query)
		{
			using (StreamWriter writer = File.AppendText(_logPath))
			{
				writer.WriteLine(p_query);
			}
		}
    }//LogAppendOnly class ends
}
