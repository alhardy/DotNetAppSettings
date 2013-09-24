
namespace Core
{
    #region usings

    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    ///     Default AppSettingsConfiguration Implementation
    /// </summary>
    /// <typeparam name="T">The class type to populate from AppSettings</typeparam>
    public class AppSettingsConfiguration<T> : IAppSettingsConfiguration<T>
    {
        private readonly NameValueCollection _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppSettingsConfiguration{T}" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public AppSettingsConfiguration(NameValueCollection settings)
        {
            _settings = settings;
        }

        /// <summary>
        ///     Loads the specified type.
        /// </summary>
        /// <returns>A populated instance of T</returns>
        /// <exception cref="System.FormatException">Throw if a property of T cannot be cast from the configuration file</exception>
        public T Load()
        {
            var type = typeof(T);
            var settingsObj = FormatterServices.GetUninitializedObject(type);
            var settingsPrefix = type.Name + ":";
            foreach (var key in _settings.AllKeys.Where(x => x.StartsWith(settingsPrefix)))
            {
                var propertyName = key.Replace(settingsPrefix, string.Empty);
                var property = type.GetProperty(propertyName);
                if (property == null)
                    throw new ApplicationException(
                        string.Format(
                            "Failed to populate {0} from appSetting, the class does not have a property called {1}",
                            type, propertyName));
                try
                {
                    var propertyValue = Convert.ChangeType(_settings[key], property.PropertyType);
                    property.SetValue(settingsObj, propertyValue, null);
                }
                catch (FormatException)
                {
                    throw new FormatException(
                        string.Format(
                            "appSetting key {0} did not match corresponding property type of '{1}' in class {2}.", key,
                            property.PropertyType, type));
                }
            }

            return (T)settingsObj;
        }
    }
}