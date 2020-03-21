using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUtils.WebUtils
{
    /// <summary>
    /// A helper class for getting the latest release from a Github repo, and for
    /// checking if an update is available.
    /// </summary>
    public static class GithubReleaseParser
    {
        /// <summary>
        /// Checks the Json to see if a repo was found. Throws FormatException if it fails to parse.
        /// </summary>
        /// <param name="JsonString">The Json to parse and search for relevant Github repo information in.</param>
        /// <returns>True if it found the repo, false otherwise.</returns>
        internal static bool RepoFound(string JsonString)
        {
            Dictionary<string, object> parsedJson = WebRequests.ParseJson(JsonString);
            // If the Github API fails to be parsed, it will always contain a message key.
            // From what I have seen, this key isn't present anywhere else in API calls.
            if (parsedJson != null)
            {
                if (parsedJson.ContainsKey("message"))
                {
                    if (parsedJson["message"].ToString() == "Not Found")
                    {
                        return false;
                    }
                }
                return true;
            }

            throw new FormatException($"Failed to parse JSON: {JsonString}");
        }

        /// <summary>
        /// Attempt to get the latest release from the repo. Throws WebException if anything goes awry/request times out, or FormatException
        /// if the JSON from the url constructed using the arguments failed to be parsed.
        /// </summary>
        /// <param name="GithubUserName">The username which the repo can be found under.</param>
        /// <param name="GithubRepoName">The name of the repo to get the latest release from.</param>
        /// <param name="TimeOut">After this specified amount of time in seconds, the check times out.</param>
        /// <returns>The latest release tag if successful, otherwise null.</returns>
        public static string GetLatestRelease(string GithubUserName, string GithubRepoName, int TimeOut)
        {
            string fullURL = String.Format("https://api.github.com/repos/{0}/{1}/releases/latest", GithubUserName, GithubRepoName);
            string jsonData = WebRequests.DownloadStringTimeout(fullURL, TimeOut);

            if (RepoFound(jsonData))
            {
                Dictionary<string, object> parsedJson = WebRequests.ParseJson(jsonData);
                if (parsedJson != null)
                {
                    return parsedJson["tag_name"].ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Attempt to get the latest release from the repo. Throws WebException if anything goes awry/request times out, or FormatException
        /// if the JSON from the url constructed using the arguments failed to be parsed.
        /// </summary>
        /// <param name="GithubUserName">The username which the repo can be found under.</param>
        /// <param name="GithubRepoName">The name of the repo to get the latest release from.</param>
        /// <param name="TimeOut">After this specified amount of time in seconds, the check times out.</param>
        /// <param name="cancellationToken">The cancellation token to use download string task.</param>
        /// <returns>The latest release tag if successful, otherwise null.</returns>
        public static async Task<string> GetLatestReleaseAsync(string GithubUserName, string GithubRepoName, int TimeOut, CancellationToken cancellationToken)
        {
            string fullURL = String.Format("https://api.github.com/repos/{0}/{1}/releases/latest", GithubUserName, GithubRepoName);
            string jsonData = await WebRequests.DownloadStringTimeoutAsync(fullURL, TimeOut, cancellationToken);

            if (RepoFound(jsonData))
            {
                Dictionary<string, object> parsedJson = WebRequests.ParseJson(jsonData);
                if (parsedJson != null)
                {
                    return parsedJson["tag_name"].ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Attempt to check if an update is available. Throws WebException if anything goes awry/requset times out, or FormatException
        /// if the JSON from the request failed to be parsed.
        /// </summary>
        /// <param name="CurrentVersion">The current version of the application being checked.</param>
        /// <param name="GithubUsername">The username of the user who owns the repo to check.</param>
        /// <param name="RepoName">The name of the repo to check.</param>
        /// <param name="Timeout">After this specified amount of time in seconds, the request will time out.</param>
        /// <returns>True/False depending on if an update is availble or not.</returns>
        public static bool GetUpdateAvailable(string CurrentVersion, string GithubUsername, string RepoName, int Timeout)
        {
            string latestRelease = GetLatestRelease(GithubUsername, RepoName, Timeout);
            if (latestRelease != null)
            {
                return CurrentVersion != latestRelease;
            }

            return false;
        }

        /// <summary>
        /// Attempt to check if an update is available. Throws WebException if anything goes awry/requset times out, or FormatException
        /// if the JSON from the request failed to be parsed.
        /// </summary>
        /// <param name="CurrentVersion">The current version of the application being checked.</param>
        /// <param name="GithubUsername">The username of the user who owns the repo to check.</param>
        /// <param name="RepoName">The name of the repo to check.</param>
        /// <param name="Timeout">After this specified amount of time in seconds, the request will time out.</param>
        /// <param name="cancellationToken">The cancellation token to use download string task.</param>
        /// <returns>True/False depending on if an update is availble or not.</returns>
        public static async Task<bool> GetUpdateAvailableAsync(string CurrentVersion, string GithubUsername, string RepoName, int Timeout, CancellationToken cancellationToken)
        {
            string latestRelease = await GetLatestReleaseAsync(GithubUsername, RepoName, Timeout, cancellationToken);
            if (latestRelease != null)
            {
                return CurrentVersion != latestRelease;
            }

            return false;
        }
    }
}
