using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMW_Katalog.Model;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BMW_Katalog.Helpers
{
    class XMLHelper
    {
        private static string xmlPath = "D:\\BMW Katalog\\BMW-Katalog---WPF-Project\\BMW Katalog\\Cars.xml";

        public static void DodajAuto(Cars noviAuto)
        {
            ObservableCollection<Cars> lista;

            if (File.Exists(xmlPath))
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<Cars>));
                using (var reader = new StreamReader(xmlPath))
                {
                    lista = (ObservableCollection<Cars>)serializer.Deserialize(reader);
                }
            }
            else
            {
                lista = new ObservableCollection<Cars>();
            }

            lista.Add(noviAuto);

            var serializerOut = new XmlSerializer(typeof(ObservableCollection<Cars>));
            using (var writer = new StreamWriter(xmlPath))
            {
                serializerOut.Serialize(writer, lista);
            }
        }

        public static List<Cars> UcitajAute()
        {
            if (!File.Exists(xmlPath))
                return new List<Cars>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Cars>));
            using (FileStream fs = new FileStream(xmlPath, FileMode.Open))
            {
                return (List<Cars>)serializer.Deserialize(fs);
            }
        }

        public static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "Untitled"; // Povratite podrazumevano ime ako je prazno
            }

            // Uklonite nedozvoljene karaktere za nazive fajlova
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            string sanitizedName = new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());

            // Takođe zamenite razmake sa podvlakama ili crticama radi bolje kompatibilnosti putanje
            sanitizedName = sanitizedName.Replace(" ", "_");

            // Obezbedite da naziv nije prazan nakon saniranja
            if (string.IsNullOrWhiteSpace(sanitizedName))
            {
                return "Untitled";
            }

            return sanitizedName;
        }

    }
}
