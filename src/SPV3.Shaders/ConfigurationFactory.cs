using System;
using SPV3.Shaders.Options;
using SPV3.Shaders.PPEs;

namespace SPV3.Shaders
{
    /// <summary>
    ///     Returns Configuration instance based on the given identifier.
    /// </summary>
    public class ConfigurationFactory
    {
        /// <summary>
        ///     Returns Configuration instance based on the given identifier.
        /// </summary>
        /// <param name="globalVariable">
        ///     Integer that represents the encoded configuration as specified in the doc/global-variable.md document.
        /// </param>
        /// <returns>
        ///     Configuration whose property values have been inferred from the given identifier.
        /// </returns>
        public static Configuration Decode(GlobalVariable globalVariable)
        {
            var x = globalVariable.Value;
            var i = 1;

            var conf = new Configuration();

            while (i <= x)
            {
                if (Convert.ToBoolean(i & x))
                    conf = Assign(conf, new ShaderState(i));

                i <<= 1;
            }

            return conf;
        }

        /// <summary>
        ///     Assigns an option to the inbound Configuration-type instance.
        ///     The option that is assigned and its value is determined by the inbound state.
        /// </summary>
        /// <param name="configuration">
        ///     Configuration instance to mutate the properties of.
        /// </param>
        /// <param name="state">
        ///     Inbound state which conforms to the doc/global-variable.md specification.
        /// </param>
        /// <returns>
        ///     Mutated configuration instance.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Invalid state integer provided.
        /// </exception>
        private static Configuration Assign(Configuration configuration, ShaderState state)
        {
            switch (state.Value)
            {
                // ambient occlusion states
                case AmbientOcclusion.StateOff:
                    configuration.AmbientOcclusion.Level = Level.Off;
                    return configuration;
                case AmbientOcclusion.StateLow:
                    configuration.AmbientOcclusion.Level = Level.Low;
                    return configuration;
                case AmbientOcclusion.StateHigh:
                    configuration.AmbientOcclusion.Level = Level.High;
                    return configuration;

                // depth of field states
                case DepthOfField.StateOff:
                    configuration.DepthOfField.Level = Level.Off;
                    return configuration;
                case DepthOfField.StateLow:
                    configuration.DepthOfField.Level = Level.Low;
                    return configuration;
                case DepthOfField.StateHigh:
                    configuration.DepthOfField.Level = Level.High;
                    return configuration;

                // depth of field states
                case DynamicFlare.StateOff:
                    configuration.DynamicFlare.Toggle = Toggle.Off;
                    return configuration;
                case DynamicFlare.StateOn:
                    configuration.DynamicFlare.Toggle = Toggle.On;
                    return configuration;

                // lens dirt states
                case LensDirt.StateOff:
                    configuration.LensDirt.Toggle = Toggle.Off;
                    return configuration;
                case LensDirt.StateOn:
                    configuration.LensDirt.Toggle = Toggle.On;
                    return configuration;

                // eye adaption states
                case EyeAdaption.StateOff:
                    configuration.EyeAdaption.Toggle = Toggle.Off;
                    return configuration;
                case EyeAdaption.StateOn:
                    configuration.EyeAdaption.Toggle = Toggle.On;
                    return configuration;

                // anti aliasing states
                case AntiAliasing.StateOff:
                    configuration.AntiAliasing.Toggle = Toggle.Off;
                    return configuration;
                case AntiAliasing.StateOn:
                    configuration.AntiAliasing.Toggle = Toggle.On;
                    return configuration;

                // debanding states
                case Debanding.StateOff:
                    configuration.Debanding.Level = Level.Off;
                    return configuration;
                case Debanding.StateLow:
                    configuration.Debanding.Level = Level.Low;
                    return configuration;
                case Debanding.StateHigh:
                    configuration.Debanding.Level = Level.High;
                    return configuration;

                default:
                    throw new ArgumentException("Invalid state given.");
            }
        }
    }
}