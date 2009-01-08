using System.IO;
using System.Reflection;
using System.Xml;
using System.Drawing;

namespace System.Resources
{
    /// <summary>
    /// This class is used to access resouces file that have been marked as "embedded".
    /// </summary>
    public class EmbeddedResource
    {
        // Local Instance Values
        private Assembly m_assembly;
        private string m_namespace;

        /// <summary>
        /// Default Constructor.  
        /// </summary>
        /// <param name="resourceAssembly">The assembly that holds the resources.</param>
        /// <param name="Namespace">The default namespace where the resouces are at.</param>
        public EmbeddedResource(Assembly resourceAssembly, string Namespace)
        {
            m_assembly = resourceAssembly;
            m_namespace = Namespace;
        }

        /// <summary>
        /// The assembly namespace where the resouce files are located.
        /// </summary>
        public string Namespace
        {
            get
            {
                return m_namespace;
            }

            set
            {
                m_namespace = value;
            }
        }

        /// <summary>
        /// Gets the give icon file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public Icon GetIcon(string filename)
        {
            Icon icon = null;

            string resouceName = string.Format("{0}.{1}", m_namespace, filename);

            Stream resStream = m_assembly.GetManifestResourceStream(resouceName);

            if (resStream == null)
            {
                throw new FileNotFoundException("Unable to find embedded resource file", resouceName);
            }

            try
            {
                icon = new Icon(resStream);
            }
            finally
            {
                resStream.Close();
            }

            return icon;
        }

        /// <summary>
        /// Gets the given bitmap file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        /// <remarks><b>WARNING:</b> Do not close the stream for the Bitmap.  For some reason it 
        /// has to remain open for the life of the bitmap. <see cref="System.Drawing.Bitmap">Bitmap Constructor with a Stream</see>
        /// </remarks>
        public Bitmap GetBitmap(string filename)
        {
            Bitmap bitmap = null;

            string resouceName = string.Format("{0}.{1}", m_namespace, filename);

            Stream resStream = m_assembly.GetManifestResourceStream(resouceName);
            if (resStream == null)
            {
                throw new FileNotFoundException("Unable to find embedded resource file", resouceName);
            }

            try
            {
                // WARNING: Do not close the stream for the Bitmap.  For some reason it 
                // has to remain open for the life of the bitmap.  See .NET documentation
                // the Bitmap() constructor for a stream object.
                bitmap = new Bitmap(resStream);
            }
            catch (Exception e)
            {
                // Only close the stream if we had a problem.
                resStream.Close();
                throw new Exception("Unable to create bitmap object", e);
            }

            return bitmap;
        }

        /// <summary>
        /// Gets the XML.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public string GetXml(string filename)
        {
            string resouceName = string.Format("{0}.{1}", m_namespace, filename);

            using (StreamReader resStream = new StreamReader(m_assembly.GetManifestResourceStream(resouceName)))
            {
                return resStream.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets an XML document object from the given xml resouce.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public XmlDocument GetXmlDocument(string filename)
        {
            XmlDocument xmlDoc = null;

            string resouceName = string.Format("{0}.{1}", m_namespace, filename);

            Stream resStream = m_assembly.GetManifestResourceStream(resouceName);
            if (resStream == null)
            {
                throw new FileNotFoundException("Unable to find embedded resource file", resouceName);
            }

            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(resStream);
            }
            finally
            {
                resStream.Close();
            }

            return xmlDoc;
        }
    }
}
