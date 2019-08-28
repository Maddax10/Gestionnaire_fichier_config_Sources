using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestionnaire_fichier_config.Models
{
    public class ImageModel
    {
        public string Path
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public ImageModel(string path, string name)
        {
            Path = path;
            Name = name;
        }

    }
}
