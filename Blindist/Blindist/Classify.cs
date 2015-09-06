using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blindist1
{
        public class Label
        {
            public string label_name { get; set; }
            public string label_score { get; set; }
        }

        public class Image
        {
            public string image_id { get; set; }
            public string image_name { get; set; }
            public List<Label> labels { get; set; }
        }

        public class RootObject
        {
            public List<Image> images { get; set; }
        }

        static public class Operations
        {
            static public List<Label> GetMatch(string json)
            {
                var result = JsonConvert.DeserializeObject<RootObject>(json);
                return result.images[0].labels;
            }

        }
    

}
