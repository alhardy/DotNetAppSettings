
namespace Core
{
    /// <summary>
    ///     Provides access to application settings configuration helpers
    /// </summary>
    /// <typeparam name="T">The type of configuration class</typeparam>
    public interface IAppSettingsConfiguration<out T>
    {
        /// <summary>
        ///     Loads the specified type from app.config.
        /// </summary>
        /// <returns>An instance of T populated with values from corresponding application setting key/values</returns>
        T Load();
    }
}
