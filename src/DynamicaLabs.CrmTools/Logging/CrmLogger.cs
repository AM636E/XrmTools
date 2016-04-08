using System;
using DynamicaLabs.Tools.Logging;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.CrmTools.Logging
{
    /// <summary>
    /// Logger with crm backend.
    /// </summary>
    public sealed class CrmLogger : BaseLogger
    {
        private readonly IOrganizationServiceFactory _osFactory;
        private readonly CrmLoggerSettings _settings;

        /// <summary>
        /// Initialize a new instance of CrmLogger.
        /// </summary>
        /// <param name="osFactory">Factory to get OrganizationService.</param>
        /// <param name="settings">Settings <see cref="CrmLoggerSettings"/></param>
        public CrmLogger(IOrganizationServiceFactory osFactory, CrmLoggerSettings settings)
        {
            _osFactory = osFactory;
            _settings = settings;
        }

        public override void Log(LogType logType, string message)
        {
            var service = _osFactory.CreateOrganizationService(null);

            var log = new Entity(_settings.LogEntityName)
            {
                [_settings.MessageField] = message
            };
            // Set log type to entity.
            _settings.LogTypeSetter?.Invoke(logType, log);
            service.Create(log);
            (service as IDisposable)?.Dispose();
        }
    }
}