using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DY.Entity
{
    public class PaymentModuleInfo
    {
        private string _code;
        private string _desc;
        private int _is_cod;
        private int _is_online;
        private string _author;
        private string _website;
        private string _version;
        private IDictionary _config;

        public PaymentModuleInfo() { }

        public string code
        {
            get { return _code; }
            set { _code = value; }
        }
        public string desc
        {
            get { return _desc; }
            set { _desc = value; }
        }
        public int is_cod
        {
            get { return _is_cod; }
            set { _is_cod = value; }
        }
        public int is_online
        {
            get { return _is_online; }
            set { _is_online = value; }
        }
        public string author
        {
            get { return _author; }
            set { _author = value; }
        }
        public string website
        {
            get { return _website; }
            set { _website = value; }
        }
        public string version
        {
            get { return _version; }
            set { _version = value; }
        }
        public IDictionary config
        {
            get { return _config; }
            set { _config = value; }
        }
    }
}
