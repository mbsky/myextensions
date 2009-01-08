using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace System.Net
{
    /// <summary>
    /// FTP methods for related to local strings instead of files.
    /// </summary>
    /// <remarks>Pegasus Library</remarks>
    public partial class FTPHelper
    {
        /// <summary>
        /// Download a single remote file and returns it as a string.
        /// </summary>
        /// <param name="remoteFile">The remote file.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="deleteFile">if set to <c>true</c> [delete file].</param>
        /// <returns>The downloaded file.</returns>
        public static string DownloadString( string remoteFile, NetworkCredential credential, bool deleteFile )
        {
            return DownloadString( remoteFile, credential, deleteFile, 1 );
        }

        /// <summary>
        /// Download a single remote file and returns it as a string.
        /// </summary>
        /// <param name="remoteFile">The remote file.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="deleteFile">if set to <c>true</c> [delete file].</param>
        /// <param name="retryAmount">The amount of times to attempt the operation.</param>
        /// <returns>The downloaded file.</returns>
        public static string DownloadString( string remoteFile, NetworkCredential credential, bool deleteFile, int retryAmount )
        {
            int numTries = 0;
            string returnStr = string.Empty;
            while( true )
            {
                try
                {
                    // Set up the ftp request
                    FtpWebRequest request = (FtpWebRequest) FtpWebRequest.Create( remoteFile );
                    request.Credentials = credential;
                    request.KeepAlive = false;
                    request.UseBinary = true;
                    request.Method = WebRequestMethods.Ftp.DownloadFile;

                    // Write string from response
                    using( FtpWebResponse response = (FtpWebResponse) request.GetResponse() )
                    {
                        using( StreamReader reader = new StreamReader( response.GetResponseStream() ) )
                        {
                            returnStr = reader.ReadToEnd();
                        }
                    }
                    break;
                }
                catch( Exception )
                {
                    numTries++;
                    if( numTries >= retryAmount )
                    {
                        throw;
                    }
                }
            }
            if( deleteFile == true )
            {
                DeleteRemoteFile( remoteFile, credential, retryAmount );
            }

            return returnStr;
        }

        /// <summary>
        /// Upload a string to a remote file
        /// </summary>
        /// <param name="remoteFile">The remote file.</param>
        /// <param name="localString">The local string.</param>
        /// <param name="credential">The credential.</param>
        public static void UploadString( string remoteFile, string localString, NetworkCredential credential )
        {
            UploadString( remoteFile, localString, credential, 1 );
        }


        /// <summary>
        /// Upload a string to a remote file
        /// </summary>
        /// <param name="remoteFile">The remote file.</param>
        /// <param name="localString">The local string.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="retryAmount">The amount of times to attempt the operation.</param>
        public static void UploadString( string remoteFile, string localString, NetworkCredential credential, int retryAmount )
        {
            int numTries = 0;
            while( true )
            {
                try
                {
                    // Set up the ftp request
                    FtpWebRequest request = (FtpWebRequest) FtpWebRequest.Create( remoteFile );
                    request.Credentials = credential;
                    request.KeepAlive = false;
                    request.UseBinary = true;
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    // Write string into request
                    using( StreamWriter writer = new StreamWriter( request.GetRequestStream() ) )
                    {
                        writer.Write( localString );
                    }

                    // We don't care what we got back as long as it didn't throw.
                    using( FtpWebResponse response = (FtpWebResponse) request.GetResponse() ) { }
                    break;
                }
                catch( Exception )
                {
                    numTries++;
                    if( numTries >= retryAmount )
                    {
                        throw;
                    }
                }
            }
        }
    }
}
