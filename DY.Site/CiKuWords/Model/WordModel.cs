using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Site
{
    public class WordModel
    {
        public string keyword { get; set; }

        public int index { get; set; }

        public int pcindex { get; set; }

        public int mobileindex { get; set; }

        public int so360index { get; set; }

        public int sort { get; set; }

        public string aboutip { get; set; }

        public int pagenum { get; set; }

        public string title { get; set; }

        public string url { get; set; }
    }

    public class WordModelList : JsonBaseModel
    {
        private List<WordModel> _data = new List<WordModel>();

        public List<WordModel> data {
            get { return _data; }
            set { _data = value; }
        }
    }
}
