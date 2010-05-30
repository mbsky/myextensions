using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace System.Web.Google
{
    /// <summary>
    /// Specifies the frequency with which a page's content changes.. 
    /// </summary>
    public enum ChangeFrequency
    {
        /// <summary>
        /// Page content aways changes.
        /// </summary>
        Always,
        /// <summary>
        /// Page content changes hourly.
        /// </summary>
        Hourly,
        /// <summary>
        /// Page content changes daily.
        /// </summary>
        Daily,
        /// <summary>
        /// Page content changes weekly.
        /// </summary>
        Weekly,
        /// <summary>
        /// Page content changes monthly.
        /// </summary>
        Monthly,
        /// <summary>
        /// Page content changes yearly.
        /// </summary>
        Yearly,
        /// <summary>
        /// Page content never changes.
        /// </summary>
        Never
    }

    /*
<url>
    <loc>http://example.com/sample.html</loc>
    <image:image>
    <image:loc>http://example.com/image.jpg</image:loc>
    </image:image>
    <image:image>
    <image:loc>http://example.com/photo.jpg</image:loc>
    </image:image>
</url>
     * 
     * 图片标记定义
标记 必需 说明 
<image:image> 是 包含单张图片的所有相关信息。每个网址 (<loc> tag) 可包含的 <image:image> 标记的数目上限为 1,000 个。 
<image:loc> 是 图片的网址。 
<image:caption>  可选 图片的说明。 
<image:geo_location> 可选 图片的地理位置。例如，<image:geo_location>Limerick, Ireland</image:geo_location>。 
<image:title> 可选 图片的标题。 
<image:license> 可选 指向图片许可的网址。 


     */

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>http://www.google.com/support/webmasters/bin/answer.py?hl=cn&answer=178636</remarks>
    public class MapImage
    {
        public string Loc { get; set; }

        public string Caption { get; set; }

        public string geo_location { get; set; }

        public string Title { get; set; }

        public string License { get; set; }
    }

    public class SitemapUrl
    {
        private DateTime _lastmod;

        private ChangeFrequency _changefreq;

        private string _priority;

        public string Loc { get; set; }

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
            }
        }

        public ChangeFrequency ChangeFreq
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

        public List<MapImage> MapImages { get; set; }

        public SitemapUrl()
        {
            //Constructor
            _changefreq = ChangeFrequency.Never;
            _priority = null;
        }

    }

    public class Sitemap
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
        public Sitemap(string filename)
        {
            this._filename = filename;
            _sitemapxml = new XmlTextWriter(_filename, UTF8Encoding.UTF8);

            _sitemapxml.WriteStartDocument();
            _sitemapxml.WriteStartElement("urlset");
            _sitemapxml.WriteAttributeString("xmlns", _xmlns);
            _sitemapxml.Flush();

        }

        public void Add(SitemapUrl _url)
        {
            _sitemapxml.WriteStartElement("url");
            _sitemapxml.WriteElementString("loc", _url.Loc);

            if (_url.LastModifiedString != "")
            {
                _sitemapxml.WriteElementString("lastmod", _url.LastModifiedString);
            }

            if (_url.ChangeFreq != ChangeFrequency.Never)
            {
                _sitemapxml.WriteElementString("changefreq", _url.ChangeFreq.ToString().ToLower());
            }

            if (_url.Priority != "")
            {
                _sitemapxml.WriteElementString("priority", _url.Priority);
            }

            if (_url.MapImages != null)
            {
                foreach (var img in _url.MapImages.Where(x => x.Loc.IsNullOrEmpty() == false))
                {
                    /*
    <image:image>
    <image:loc>http://example.com/image.jpg</image:loc>
    </image:image>

 <image:loc> 是 图片的网址。 
<image:caption>  可选 图片的说明。 
<image:geo_location> 可选 图片的地理位置。例如，<image:geo_location>Limerick, Ireland</image:geo_location>。 
<image:title> 可选 图片的标题。 
<image:license> 可选 指向图片许可的网址。 
                    
                     */
                    //image:image
                    _sitemapxml.WriteStartElement("image:image");

                    _sitemapxml.WriteStartElement("image:loc");
                    _sitemapxml.WriteString(img.Loc);
                    _sitemapxml.WriteEndElement();

                    if (img.Caption.IsNullOrEmpty() == false)
                    {
                        _sitemapxml.WriteStartElement("image:caption");
                        _sitemapxml.WriteString(img.Caption);
                        _sitemapxml.WriteEndElement();
                    }

                    if (img.geo_location.IsNullOrEmpty() == false)
                    {
                        _sitemapxml.WriteStartElement("image:geo_location");
                        _sitemapxml.WriteString(img.geo_location);
                        _sitemapxml.WriteEndElement();
                    }
                    if (img.Title.IsNullOrEmpty() == false)
                    {
                        _sitemapxml.WriteStartElement("image:title");
                        _sitemapxml.WriteString(img.Title);
                        _sitemapxml.WriteEndElement();
                    }
                    if (img.License.IsNullOrEmpty() == false)
                    {
                        _sitemapxml.WriteStartElement("image:license");
                        _sitemapxml.WriteString(img.License);
                        _sitemapxml.WriteEndElement();
                    }

                    _sitemapxml.WriteEndElement();
                }
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
