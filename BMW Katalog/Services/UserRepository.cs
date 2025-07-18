using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BMW_Katalog.Model;

public static class UserRepository
{
    private static readonly string FilePath = "Users.xml";

    public static List<User> LoadUsers()
    {
        if (!File.Exists(FilePath))
        {
            var defaultUsers = new List<User>
            {
                new User("admin", "admin123", UserRole.Admin),
                new User("fikilauda", "starwars123", UserRole.Visitor)
            };
            SaveUsers(defaultUsers);
            return defaultUsers;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
        using (FileStream fs = new FileStream(FilePath, FileMode.Open))
        {
            return (List<User>)serializer.Deserialize(fs);
        }
    }

    public static void SaveUsers(List<User> users)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
        using (FileStream fs = new FileStream(FilePath, FileMode.Create))
        {
            serializer.Serialize(fs, users);
        }
    }
}
