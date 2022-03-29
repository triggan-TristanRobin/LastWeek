using Google.Apis.Drive.v3.Data;

namespace LastWeek.Exporter.Interfaces
{
    public interface IAccountConnector
	{
		User User { get; set; }
		bool LoggedIn { get; }
		bool Login();
    }
}
