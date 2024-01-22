using Shared;

namespace OutsourcePlatformApp.Hubs;

public class NotificationManager
{
    private List<NotificationConnection> Users { get; } = new();


    public void ConnectUser(string userToken, string connectionId)
    {
        var username = JwtUtil.GetClaimsFromToken(userToken)["username"];
        var userAlreadyExists = GetConnectedUserByUsername(username);
        if (userAlreadyExists != null)
        {
            userAlreadyExists.ConnectionId = connectionId;
            return;
        }
        var user = new NotificationConnection
        {
            Token = userToken,
            ConnectionId = connectionId,
            Username = username
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
        var username = JwtUtil.GetClaimsFromToken(token)["username"];
        return Users.FirstOrDefault(connection => connection.Username == username);
    }
    public NotificationConnection? GetConnectedUserByUsername(string username)
    {
        return Users.FirstOrDefault(connection => connection.Username == username);
    }
}