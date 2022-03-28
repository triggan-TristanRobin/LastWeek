using Google.Apis.Drive.v3.Data;

namespace ReviewExporter.Interfaces
{
    public interface IAccountConnector
	{
		User User { get; set; }
		bool LoggedIn { get; }
		bool Login();
    }
}
