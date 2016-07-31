using System;

[Serializable]
public class TokenPayload
{
    public string client_id;
    public string client_secret;
    public string refresh_token;
    public string grant_type;

    public TokenPayload(string client_id, string client_secret, string refresh_token, string grant_type)
    {
        this.client_id = client_id;
        this.client_secret = client_secret;
        this.refresh_token = refresh_token;
        this.grant_type = grant_type;
    }
}
