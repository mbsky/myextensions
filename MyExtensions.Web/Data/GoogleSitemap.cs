using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace System.Web.Data
{
    public class GoogleSitemapUrl
    {
         private string _loc;
        private DateTime _lastmod ;

        private string _changefreq;
        //always
        //hourly
        //daily
        //weekly
        //monthly
        //yearly
        //never

        private string _priority;

        private int _lastmodifiedday;
        private int _lastmodifiedmonth;
        private int _lastmodifiedyear;

        public string Loc
        {
            get
            {
                return _loc;
            }

            set
            {
                _loc = value;
            }
        }

        public DateTime LastModifiedDateTime
        {
            get
            {
                return _lastmod;
            }

            set
            {
                _lastmod = value;
            }
        }

        public string LastModifiedString
        {
            //Readonly Property
            get
            {   //2010-01-26T17:11:38+01:00
                //Get LastModifiedDate
                return _lastmod.ToString("yyyy-MM-dd") + "T" + _lastmod.ToString("HH:mm:ss") + "+01:00";
                //this._lastmodifiedday = _lastmod.Day;
                //this._lastmodifiedmonth = _lastmod.Month;
                //this._lastmodifiedyear = _lastmod.Year;
                //return string.Format("{0}-{1}-{2}", _lastmodifiedyear, _lastmodifiedmonth, _lastmodifiedday);
            }
        }

        public string ChangeFreq
        {
            get
            {
                return _changefreq;
            }

            set
            {
                _changefreq = value;
            }
        }

        public string Priority
        {
            get
            {
                return _priority;
            }

            set
            {
                _priority = value;
            }
        }

        public GoogleSitemapUrl()
        {
            //Constructor
            _loc = null;
            _changefreq = null;
            _priority = null;
        }

    }

    public class GoogleSitemap
    {
        private string _filename;
        private XmlTextWriter _sitemapxml;
        private string _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        public string Xmlns
        {
            get
            {
                return _xmlns;
            }

            set
            {
                _xmlns = value;
            }
        }

        //Constructor
        public GoogleSitemap(string filename)
        {
            this._filename = filename;
            _sitemapxml = new XmlTextWriter(_filename, UTF8Encoding.UTF8);

            _sitemapxml.WriteStartDocument();
            _sitemapxml.WriteStartElement("urlset");
            _sitemapxml.WriteAttributeString("xmlns", _xmlns);
            _sitemapxml.Flush();

        }

        public void Add(GoogleSitemapUrl _url)
        {
            _sitemapxml.WriteStartElement("url");
            _sitemapxml.WriteElementString("loc", _url.Loc);

            if (_url.LastModifiedString != "")
            {
                _sitemapxml.WriteElementString("lastmod", _url.LastModifiedString);
            }

            if (_url.ChangeFreq != "")
            {
                _sitemapxml.WriteElementString("changefreq", _url.ChangeFreq);
            }

            if (_url.Priority != "")
            {
                _sitemapxml.WriteElementString("priority", _url.Priority);
            }

            _sitemapxml.WriteEndElement();
            _sitemapxml.Flush();

        }

        public void Write()
        {
            _sitemapxml.WriteEndElement();
            _sitemapxml.WriteEndDocument();
            _sitemapxml.Close();
        }

    }
}
