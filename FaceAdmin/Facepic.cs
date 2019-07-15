using System.Collections.Generic;

namespace FaceAdmin
{
    public class Facepic
    {
        public List<DataItem> data { get; set; }
        public string result;
        public string success;
    }
    public class DataItem
    {
        public string faceId;
        public string feature;
        public string featureKey;
        public string path;
        public string personId;
    }
}