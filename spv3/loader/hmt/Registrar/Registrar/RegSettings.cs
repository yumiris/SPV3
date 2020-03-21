using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Registrar
{
    /// <summary>
    /// Represents the settings in the registry.
    /// Contains an internal dictionary which maps option names to option objects.
    /// </summary>
    public class RegSettings
    {
        string _baseKey = null;
        string _rootKey = null;
        string _registryString = null;

        private Dictionary<string, RegOption> _settingsMapping = new Dictionary<string, RegOption>();

        /// <summary>
        /// Constructor for the settings instance.
        /// </summary>
        /// <param name="baseKey">The base key in the registry the root key will go under. EG: HKEY_CURRENT_USERS.</param>
        /// <param name="rootKey">The root key which is where all the keys the options use will fall under. 
        /// EG: passing 'RootKey' -> HKEY_CURRENT_USERS/RootKey in the registry.</param>
        public RegSettings(string baseKey, string rootKey)
        {
            _baseKey = baseKey;

            while (rootKey[0] == '/' || rootKey[0] == '\\')
            {
                rootKey = rootKey.Substring(1);
            }
            _rootKey = rootKey.Replace(@"/", @"\\");

            _registryString = $"{_baseKey}\\{_rootKey}";
        }

        /// <summary>
        /// Adds the option instance to the internal mapping of options. if the name is already in use.
        /// </summary>
        /// <param name="optionName">The name of the option to use in the registry. Can be different than the keyname.</param>
        /// <param name="option">The option instance.</param>
        public void RegisterSetting(string optionName, RegOption option)
        {
            if (_settingsMapping.ContainsKey(optionName) || option == null)
            {
                throw new RegOptionAssignmentException("The option being registered is already registered/option value can not be null.");
            }
            _settingsMapping.Add(optionName, option);
        }

        /// <summary>
        /// Retrieves the value associated with the option in the settings mapping (not the option object).
        /// </summary>
        /// <param name="optionName">The name of the option to get the value of.</param>
        /// <returns>The option value. Raises an exception of type OptionRetrievalException if the option is not registered.</returns>
        public T GetOption<T>(string optionName)
        {
            bool retrievalSuccessful = _settingsMapping.TryGetValue(optionName, out RegOption value);
            if (!retrievalSuccessful)
            {
                throw new RegOptionRetrievalException($"Failed to retrieve {optionName}: The option is not registered.");
            }
            return (T)value.OptionValue;
        }

        /// <summary>
        /// Attempts to set the value of an option in the settings mapping to the supplied value.
        /// If the option fails to get set, it will keep its default value.
        /// </summary>
        /// <param name="optionName">The name of the option in the mapping to be changed.</param>
        /// <param name="value">The value to attempt to set the option to.</param>
        /// <returns>Returns an error message detailing why it failed to be set, or null if it was sucessfully set.</returns>
        public void SetOption(string optionName, Object value)
        {
            ValidationResponse validationResult = _settingsMapping[optionName].SetOptionValue(value);
            if (!validationResult.Successful)
            {
                string exString = $"Failed to set option '{optionName}', reason: '{validationResult.Information}'. Option will keep its current value.";
                throw new RegOptionAssignmentException(exString);
            }
        }

        /// <summary>
        /// Check if the root key of the setting currently exists in the registry.
        /// </summary>
        /// <returns>True if the key does exist, false if it doesn't. Raises an exception of type InvalidOperationException if the base key was wrong.</returns>
        public bool RootKeyExists()
        {
            RegistryKey registryRoot;
            switch (_baseKey)
            {
                case RegBaseKeys.HKEY_CURRENT_USER:
                    registryRoot = Registry.CurrentUser.OpenSubKey(_rootKey, false);
                    break;
                case RegBaseKeys.HKEY_CLASSES_ROOT:
                    registryRoot = Registry.ClassesRoot.OpenSubKey(_rootKey, false);
                    break;
                case RegBaseKeys.HKEY_CURRENT_CONFIG:
                    registryRoot = Registry.CurrentConfig.OpenSubKey(_rootKey, false);
                    break;
                case RegBaseKeys.HKEY_LOCAL_MACHINE:
                    registryRoot = Registry.LocalMachine.OpenSubKey(_rootKey, false);
                    break;
                case RegBaseKeys.HKEY_USERS:
                    registryRoot = Registry.Users.OpenSubKey(_rootKey, false);
                    break;
                default:
                    throw new InvalidOperationException("Invalid Base Key given. Use the BaseKeys helper class.");
            }
            return registryRoot != null;
        }

        /// <summary>
        /// Attempts to load values out of the registry and set the option instance's values with the loaded values.
        /// </summary>
        /// <returns>Null if successful, or a string detailing which options failed and why.</returns>
        public void LoadSettings() // Load settings from the registry instance
        {
            string loadResult = null;
            foreach (KeyValuePair<string, RegOption> kvp in _settingsMapping)
            {
                string subKeys = kvp.Value.GetSubKeys();
                string keyPath = _registryString;

                if (subKeys != null)
                {
                    keyPath += subKeys;
                }

                Object keyValue;
                try
                {
                    keyValue = Registry.GetValue(keyPath, kvp.Value.GetKeyName(), kvp.Value.OptionValue);
                    if (keyValue == null)
                    {
                        loadResult += $"\r\nFailed loading option '{kvp.Value.GetKeyName()}': Option did not exist in the registry. " +
                            $"The value will use its default.";
                    }
                    else
                    {
                        ValidationResponse validation_result = kvp.Value.SetOptionValue(keyValue);
                        if (!validation_result.Successful)
                        {
                            loadResult += $"\r\nFailed when validating an option while loading: '{kvp.Value.GetKeyName()}' - " +
                                $"'{validation_result.Information}'. The value will use its default.";
                        }
                    }
                }
                catch (FormatException)
                {
                    loadResult += $"\r\nFailed when loading option '{kvp.Value.GetKeyName()}': Option was not formatted correctly. " +
                        "This usually occurs if someone manually" + "alters the entry in the registry. The value will use its default.";
                }
            }

            if (loadResult != null)
            {
                throw new RegLoadException(loadResult);
            }
        }

        /// <summary>
        /// Attempts to save the current values in the settings dictionary to the registry.
        /// </summary>
        /// <returns>Null if successful, or a string detailing which options failed and why.</returns>
        public void SaveSettings()
        {
            string saveResult = null;

            foreach (KeyValuePair<string, RegOption> kvp in _settingsMapping)
            {
                string subKeys = kvp.Value.GetSubKeys();
                string keyOut = _registryString;
                if (subKeys != null)
                {
                    keyOut += subKeys;
                }
                ValidationResponse validation_result = kvp.Value.Validate();
                if (!validation_result.Successful)
                {
                    saveResult += $"\r\nFailed when validating an option during saving: '{kvp.Value.GetKeyName()}' - " +
                        $"'{validation_result.Information}', this occurs if someone manually edits the registry " + "to use an invalid value. " +
                        "The value will use its default.";
                }
                else
                {
                    Registry.SetValue(keyOut, kvp.Value.GetKeyName(), kvp.Value.OptionValue);
                }
            }

            if (saveResult != null)
            {
                throw new RegSaveException(saveResult);
            }
        }
    }
}
