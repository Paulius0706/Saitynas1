using TimeT.Auth.Model;

namespace TimeT.Auth
{
    public static class CostumeAuth
    {

        public static bool Atuh(string policy, string token, string id, string secondId = "")
        {
            if (token == "") return false;
            switch (policy)
            {
                case PolicyNames.ResourceOwner:
                    return token == id || token == secondId;
                    break;
            }
            return false;
        }
    }
}
