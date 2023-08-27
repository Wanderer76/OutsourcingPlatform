namespace OutsourcePlatformApp.Hubs;

public class NotificationManager
{
    private List<NotificationConnection> Users { get; } = new();


    public void ConnectUser(string userToken, string connectionId)
    {
        var userAlreadyExists = GetConnectedUserByToken(userToken);
        if (userAlreadyExists != null)
        {
            userAlreadyExists.ConnectionId = connectionId;
            return;
        }
        var user = new NotificationConnection
        {
            Token = userToken,
            ConnectionId = connectionId
        };
        Users.Add(user);
    }


    public bool DisconnectUser(string connectionId)
    {
        var userExists = GetConnectedUserById(connectionId);
        if (userExists == null)
            return false;
        if (!userExists.ConnectionId.Equals(connectionId))
            return false;

        Users.Remove(userExists);
        return true;
    }


    private NotificationConnection? GetConnectedUserById(string connectionId)
    {
        return Users
            .FirstOrDefault(x => x.ConnectionId == connectionId);
    }


    public NotificationConnection? GetConnectedUserByToken(string token)
    {
        return Users.FirstOrDefault(connection => connection.Token == token);
    }
}