using System.IO;
using System.Text;

namespace PackageDelivery.Services.Maps.Google.Common.Responses
{
    /// <summary>
    /// Content text response
    /// </summary>
    public abstract class ContentTextResponse : ContentResponse<string>
    {

        #region Overrides

        /// <summary>
        /// Save content
        /// </summary>
        /// <param name="path">Path</param>
        public override void SaveContent(string path)
        {

            // Save with default encoding
            SaveContent(path, Encoding.UTF8);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Save content with encoding
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="encoding">Encoding</param>
        public void SaveContent(string path, Encoding encoding)
        {

            File.WriteAllText(path, Content, encoding);

        } 

        #endregion
    }

}