using System;

namespace Core.Facts
{
    using System.Collections.Specialized;
    using Xunit;

    public class AppSettingsConfigurationFacts
    {
        public class FakeSettings
        {
            public string PropertyOne { get; set; }
            public int PropertyTwo { get; set; }
        }


        public class FakeValueObjectSettings
        {
            public FakeValueObjectSettings(string propertyOne, int propertyTwo)
            {
                PropertyOne = propertyOne;
                PropertyTwo = propertyTwo;
            }

            public string PropertyOne { get; private set; }
            public int PropertyTwo { get; private set; }
        }


        public class Load
        {
            [Fact]
            public void throws_format_exception_for_properties_types_that_cannot_cast()
            {
                // Arrange            
                var appSettings = new NameValueCollection
                                      {
                                          {"FakeSettings:PropertyOne", "Test String"},
                                          {"FakeSettings:PropertyTwo", "This string should fail to load"}
                                      };
                var appSettingConfiguration = new AppSettingsConfiguration<FakeSettings>(appSettings);

                // Assert
                Assert.Throws<FormatException>(() =>
                {
                    // Act
                    appSettingConfiguration.Load();
                });
            }

            [Fact]
            public void throws_application_exception_for_appsettings_keys_without_a_matching_property()
            {
                // Arrange            
                var appSettings = new NameValueCollection
                                      {
                                          {"FakeSettings:PropertyThatDoesNotExist", "Test String"},
                                      };
                var appSettingConfiguration = new AppSettingsConfiguration<FakeSettings>(appSettings);

                // Assert
                Assert.Throws<ApplicationException>(() =>
                {
                    // Act
                    appSettingConfiguration.Load();
                });
            }

            [Fact]
            public void can_load_object()
            {
                // Arrange            
                var appSettings = new NameValueCollection
                                      {
                                          {"FakeSettings:PropertyOne", "Test String"},
                                          {"FakeSettings:PropertyTwo", "100"}
                                      };
                var appSettingConfiguration = new AppSettingsConfiguration<FakeSettings>(appSettings);

                // Act
                var settings = appSettingConfiguration.Load();

                // Assert
                Assert.True(settings.PropertyOne == "Test String");
                Assert.True(settings.PropertyTwo == 100);
            }

            [Fact]
            public void can_load_immutable_object()
            {
                // Arrange            
                var appSettings = new NameValueCollection
                                      {
                                          {"FakeValueObjectSettings:PropertyOne", "Test String"},
                                          {"FakeValueObjectSettings:PropertyTwo", "100"}
                                      };
                var appSettingConfiguration = new AppSettingsConfiguration<FakeValueObjectSettings>(appSettings);

                // Act
                var settings = appSettingConfiguration.Load();

                // Assert
                Assert.True(settings.PropertyOne == "Test String");
                Assert.True(settings.PropertyTwo == 100);
            }
        }
    }
}
