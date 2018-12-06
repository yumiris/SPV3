namespace SPV3.Loader
{
    /// <summary>
    ///     Creates Parameters-type instances.
    /// </summary>
    public class ParametersParser
    {
        /// <summary>
        ///     Initialise ExecutableParameter based on the inbound string value.
        /// </summary>
        /// <param name="parameters">
        ///     String value representing representing HCE-compliant startup parameters.
        /// </param>
        /// <returns>
        ///     Parameters instance based on the inbound string value.
        /// </returns>
        public Parameters Parse(string parameters)
        {
            var result = new Parameters();

            if (parameters.Contains("-nosound"))
                result.DisableSound = true;

            if (parameters.Contains("-novideo"))
                result.DisableVideo = true;

            if (parameters.Contains("-nojoystick"))
                result.DisableJoystick = true;

            if (parameters.Contains("-nogamma"))
                result.DisableGamma = true;

            if (parameters.Contains("-safemode"))
                result.EnableSafeMode = true;

            if (parameters.Contains("-window"))
                result.EnableWindowMode = true;

            if (parameters.Contains("-screenshot"))
                result.EnableScreenshot = true;

            if (parameters.Contains("-console"))
                result.EnableConsole = true;

            if (parameters.Contains("-devmode"))
                result.EnableDeveloperMode = true;

            if (parameters.Contains("-useff"))
                result.CardType = CardType.FixedFunction;

            if (parameters.Contains("-use11"))
                result.CardType = CardType.Shaders11Card;

            if (parameters.Contains("-use14"))
                result.CardType = CardType.Shaders14Card;

            if (parameters.Contains("-use20"))
                result.CardType = CardType.Shaders20Card;

            // TODO: Parse argument-type parameters!

            return result;
        }
    }
}