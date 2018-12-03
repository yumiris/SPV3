using System;
using SPV3.Shaders.Options;
using SPV3.Shaders.PPEs;

namespace SPV3.Shaders
{
    /// <summary>
    ///     Returns GlobalVariable instance based on the given Configuration.
    /// </summary>
    public class GlobalVariableFactory
    {
        /// <summary>
        ///     Encodes an inbound Configuration to a Global Variable value.
        ///     The encoding mechanism is specified in the doc/global-variable.md documentation.
        /// </summary>
        /// <param name="configuration">
        ///    Configuration instance representing user preferences for post-processing effects.
        /// </param>
        /// <returns>
        ///    Global Variable instance whose Value property represents the encoded Configuration intsance.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///    One of the Configuration properties' values are invalid.
        /// </exception>
        public static GlobalVariable Encode(Configuration configuration)
        {
            var value = 0;

            // ambient occlusion state
            switch (configuration.AmbientOcclusion.Level)
            {
                case Level.Off:
                    value += AmbientOcclusion.StateOff;
                    break;
                case Level.Low:
                    value += AmbientOcclusion.StateLow;
                    break;
                case Level.High:
                    value += AmbientOcclusion.StateHigh;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // depth of field state
            switch (configuration.DepthOfField.Level)
            {
                case Level.Off:
                    value += DepthOfField.StateOff;
                    break;
                case Level.Low:
                    value += DepthOfField.StateLow;
                    break;
                case Level.High:
                    value += DepthOfField.StateHigh;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // dynamic flare states
            switch (configuration.DynamicFlare.Toggle)
            {
                case Toggle.Off:
                    value += DynamicFlare.StateOff;
                    break;
                case Toggle.On:
                    value += DynamicFlare.StateOn;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // lens dirt states
            switch (configuration.LensDirt.Toggle)
            {
                case Toggle.Off:
                    value += LensDirt.StateOff;
                    break;
                case Toggle.On:
                    value += LensDirt.StateOn;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // eye adaption states
            switch (configuration.EyeAdaption.Toggle)
            {
                case Toggle.Off:
                    value += EyeAdaption.StateOff;
                    break;
                case Toggle.On:
                    value += EyeAdaption.StateOn;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // debanding states
            switch (configuration.Debanding.Level)
            {
                case Level.Off:
                    value += Debanding.StateOff;
                    break;
                case Level.Low:
                    value += Debanding.StateLow;
                    break;
                case Level.High:
                    value += Debanding.StateHigh;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new GlobalVariable(value);
        }
    }
}