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

    }
}
