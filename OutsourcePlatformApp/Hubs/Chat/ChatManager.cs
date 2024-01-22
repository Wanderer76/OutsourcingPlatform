using Shared;

namespace OutsourcePlatformApp.Hubs.Chat;

public class ChatManager
{
    private List<ChatConnection> Users { get; } = new();

    public ChatConnection? GetConnectedUserById(string connectionId)
    {
        return Users
            .FirstOrDefault(x => x.ConnectionId == connectionId);
    }
    
    public ChatConnection? GetConnectedUserByToken(string token)
    {
        var username = JwtUtil.GetClaimsFromToken(token)["username"];
        return Users.FirstOrDefault(connection => connection.Username == username);
    }

    public void ConnectUser(string userToken, string connectionId)
    {
        var userAlreadyExists = GetConnectedUserByToken(userToken);
        if (userAlreadyExists != null)
        {
            userAlreadyExists.ConnectionId = connectionId;
            return;
        }

        var user = new ChatConnection
        {
            ConnectionId = connectionId,
            Token = userToken,
            Username = JwtUtil.GetClaimsFromToken(userToken)["username"]
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
}