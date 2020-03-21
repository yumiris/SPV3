using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUtils.WebUtils
{
    /// <summary>
    /// A helper class for parsing a Github README.MD file.
    /// </summary>
    public static class GithubReadmeParser
    {
        /// <summary>
        /// Attempts to download a string after a given prefix from a README.md. Throws WebException if anything goes awry/the request times out.
        /// </summary>
        /// <param name="ReadmeURL">The URL the README.MD is located at.</param>
        /// <param name="LinePrefix">The prefix to search for. This is not included in the result.</param>
        /// <param name="TimeOut">After this specified amount of time in seconds, the request will time out.</param>
        /// <returns>The downloaded string if successful, otherwise returns null.</returns>
        public static string GetLineFromReadme(string ReadmeURL, string LinePrefix, int TimeOut)
        {
            string readmeText = WebRequests.DownloadStringTimeout(ReadmeURL, TimeOut);

            if (readmeText != null)
            {
                using (StringReader sr = new StringReader(readmeText))
                {
                    string currentLine;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (currentLine.StartsWith(LinePrefix))
                        {
                            return currentLine.Substring(LinePrefix.Length);
                        }
                    }
                }
            }

            return null;
        }

        public static async Task<string> GetLineFromReadmeAsync(string ReadmeURL, string LinePrefix, int TimeOut, CancellationToken cancellationToken)
        {
            string readmeText = await WebRequests.DownloadStringTimeoutAsync(ReadmeURL, TimeOut, cancellationToken);

            if (readmeText != null)
            {
                using (StringReader sr = new StringReader(readmeText))
                {
                    string currentLine;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (currentLine.StartsWith(LinePrefix))
                        {
                            return currentLine.Substring(LinePrefix.Length);
                        }
                    }
                }
            }

            return null;
        }
    }
}
