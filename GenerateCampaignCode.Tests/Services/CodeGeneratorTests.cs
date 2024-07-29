using GenerateCampaignCode.Application.Interfaces;
using GenerateCampaignCode.Application.Services;
using GenerateCampaignCode.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace GenerateCampaignCode.Tests.Services
{
    public class CodeGeneratorTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ICodeGenerator _codeGenerator;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly CampaignCodeSettings _settings;

        public CodeGeneratorTests()
        {
            var services = new ServiceCollection();
            _settings = new CampaignCodeSettings
            {
                Characters = "ACDEFGHKLMNPRTXYZ234579",
                Length = 8,
                PrivateKey = "MIICWgIBAAKBgHR6A1lznlaxFclFdgW1iZjYLBgxpZIV7wXW7eS4Dvinlg8uusRaFnkD" +
                                "/X9G/l2VDDyarK9kPMVvNqUAWo/DqGmOKSjBeL8DVvOTicGU9MDfdWgcxlFy"+
                                "OAacqiAcofT2l9kTTkamPU5C5wVygLVwphySm1vRoG8jDaRvQkIsdwstAgMBAAEC"+
                                "gYAwt+g2vhl4gVFvglI/SRNojuLCq+FpHSuA8clHZYU9lDs71nvgLR5BN94MIpG6"+
                                "auFXaArawb55hm8AzQkUIO6Lx0rhWXb4+v/cJkMk6Jdfk1tNfLEGOb846EqWQWUQ"+
                                "CRNR0iTllNC4yeowW8pVce+91rUmrSOFEHoDHhbxcLSnMQJBALmIzRMKzBRSXiJC"+
                                "5gNL/hoGdj8jBlVzGC1x9eWOZFB5a4IGa/nDh2FlzTQxtubyMg0siKGXD6rta1YI"+
                                "zi2MlRMCQQCgttcjr8jaPXwYaee/3ae8xvwZ8bar50+2uSscooNBadKQbvfoTF/d"+
                                "tDoEmx00Xu77JppioiNThpT+O/XZISa/AkAXjQzzENjM75OxZ6qI2pmbthxGcWy5"+
                                "Zg24nxGmnQeQy4jhDW2hW7eQnnqI2JKuCCpgT7ncQS+k89Q/LIj3cTPvAkBnqEZu"+
                                "8AweJxYJMRWWvMJZkgY8PZjSm2jgs+HIoFEEOdrj6Y7gN5KFjp71JY7anniJaMae"+
                                "43DhiKZErCvPlgBFAkB/0EIhnGUe5nxCCoBnvPN3BolkG7KLlkF2+PoLcAFbuVFG"+
                                "tixQSWv5KKSJNGrgIQ6JwYjZObpo8xjnQ4ADZcTN"
            };

            var optionsMock = new Mock<IOptions<CampaignCodeSettings>>();
            optionsMock.Setup(x => x.Value).Returns(_settings);

            _memoryCacheMock = new Mock<IMemoryCache>();
            services.AddLogging();
            services.AddSingleton(optionsMock.Object);
            services.AddSingleton(_memoryCacheMock.Object);
            services.AddSingleton<ICodeGenerator, CodeGenerator>();

            _serviceProvider = services.BuildServiceProvider();
            _codeGenerator = _serviceProvider.GetService<ICodeGenerator>();
        }

        [Fact]
        public void GenerateCodeWithHMACSHA256_ShouldReturnNewCode_WhenNotInCache()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var uniqueKey = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            object cachedCode = null;
            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCode)).Returns(false);

            // Act
            var result = _codeGenerator.GenerateCodeWithHMACSHA256(id, uniqueKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_settings.Length, result.Length);
            Assert.All(result, character => Assert.Contains(character, _settings.Characters));

        }

        [Fact]
        public void ValidateCodeWithHMACSHA256_ShouldReturnTrue_WhenCodeIsValid()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var uniqueKey = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var code = _codeGenerator.GenerateCodeWithHMACSHA256(id, uniqueKey);

            // Act
            var isValid = _codeGenerator.ValidateCodeHMACSHA256(id, uniqueKey, code);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateCodeWithHMACSHA256_ShouldReturnFalse_WhenCodeIsInvalid()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var uniqueKey = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var invalidCode = "INVALID_CODE";

            // Act
            var isValid = _codeGenerator.ValidateCodeHMACSHA256(id, uniqueKey, invalidCode);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void GenerateCodeWithHMACSHA256_ShouldReturnUniqueCodes()
        {
            // Arrange
            var uniqueKeys = new HashSet<string>();
            var codes = new HashSet<string>();
            var random = new Random();

            for (int i = 0; i < 1_000_000; i++)
            {
                uniqueKeys.Add(random.Next(0, int.MaxValue).ToString());
            }

            // Act
            foreach (var uniqueKey in uniqueKeys)
            {
                var id = Guid.NewGuid().ToString();
                var code = _codeGenerator.GenerateCodeWithHMACSHA256(id, uniqueKey);
                codes.Add(code);
            }

            // Assert
            int expectedCount = uniqueKeys.Count;
            int actualCount = codes.Count;
            double marginOfError = (double)(expectedCount - actualCount) / expectedCount;

            Assert.True(marginOfError <= 0.00001, $"Margin of error is too high: {marginOfError}");
        }

        [Fact]
        public void GenerateCodeWithSHA1_ShouldReturnNewCode_WhenNotInCache()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var uniqueKey = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            object cachedCode = null;
            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCode)).Returns(false);

            // Act
            var result = _codeGenerator.GenerateCodeSHA1(id, uniqueKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_settings.Length, result.Length);
            Assert.All(result, character => Assert.Contains(character, _settings.Characters));

        }

        [Fact]
        public void ValidateCodeWithSHA1_ShouldReturnTrue_WhenCodeIsValid()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var uniqueKey = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var code = _codeGenerator.GenerateCodeSHA1(id, uniqueKey);

            // Act
            var isValid = _codeGenerator.ValidateCodeSHA1(id, uniqueKey, code);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateCodeWithSHA1_ShouldReturnFalse_WhenCodeIsInvalid()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var uniqueKey = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var invalidCode = "INVALID_CODE";

            // Act
            var isValid = _codeGenerator.ValidateCodeHMACSHA256(id, uniqueKey, invalidCode);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void GenerateCodeWithSHA1_ShouldReturnUniqueCodes()
        {
            // Arrange
            var uniqueKeys = new HashSet<string>();
            var codes = new HashSet<string>();
            var random = new Random();

            for (int i = 0; i < 100000; i++)
            {
                uniqueKeys.Add(random.Next(0, int.MaxValue).ToString());
            }

            // Act
            foreach (var uniqueKey in uniqueKeys)
            {
                var id = Guid.NewGuid().ToString();
                var code = _codeGenerator.GenerateCodeSHA1(id, uniqueKey);
                codes.Add(code);
            }

            // Assert
            int expectedCount = uniqueKeys.Count;
            int actualCount = codes.Count;
            double marginOfError = (double)(expectedCount - actualCount) / expectedCount;

            Assert.True(marginOfError <= 0.0005, $"Margin of error is too high: {marginOfError}");
        }
    }
}
